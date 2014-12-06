using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using System.IO;
using System.Runtime.InteropServices;
using NLog;
using TRLibrary.Commons;
using NHibernate;
using NHibernate.Linq;
using System.Data;
using PVK.Data;
using PVK.Control.View;
using System.Linq;
using NHibernate.Criterion;
using PVK.Control.Presenter;
using MediaDataGetter;
//using Vlc.DotNet.Core.Medias;
//using Vlc.DotNet.Core;
//using Vlc.DotNet.Forms;

namespace PVK.Control
{
	public sealed partial class MainForm : XtraForm,  IMainView
	{

		private bool _enableSaveSettings = true;
		private readonly ISession _mainSession;
		private  DataTable _weighingDataTable;
		private  DataTable _modelVehicleDataTable;
		private  DataTable _attributesDataTable;
		private  DataTable _attributesValueDataTable;
		private  DataTable _scalesDataTable;
		private  DataTable _pvkDataTable;
		private  DataTable _permissionLoadDataTable;
		private  DataTable _damageDataTable;
		private  IQueryable<PVK_MODEL_VEHICLE> _modelVehicle;
		private  IQueryable<PVK_ATTRIBUTES> _attributes;
		private  IQueryable<PVK_DAMAGE_TS_VALUE> _damage;
		private  IQueryable<PVK_ATTR_VALUES> _attributesValues;
		private  IQueryable<PVK_SCALES> _scales;
		private  IQueryable<PVK.Data.PVK> _pvk;
		private  IQueryable<PVK_WEIGHING> _weighings;
		private  IQueryable<PVK_PERMISSION_WEIGH> _permissionWeigh;
		private  IQueryable<PVK_PERMISSION_AXIS_LOAD> _permissionLoads;
		private  IQueryable<PVK_PERIOD_CALCULATE_DAMAGE_TS> _periodCalcDamegeLoads;

		private  List<PVK_ATTRIBUTES> _attributesList;
		private  List<PVK_DAMAGE_TS_VALUE> _damageList;
		private  List<PVK_ATTR_VALUES> _attributesValuesList;
		public	 List<PVK_SCALES> _scalesList;
		private  List<PVK.Data.PVK> _pvkList;
		private  List<PVK_WEIGHING> _weighingsList;
		private  List<PVK_PERMISSION_AXIS_LOAD> _permissionLoadsList;
		private  List<PVK_MODEL_VEHICLE> _modelVehicleList;
		private  List<PVK_PERMISSION_WEIGH> _permissionWeighList;
		private  List<PVK_PERIOD_CALCULATE_DAMAGE_TS> _periodCalcDamegeList;

		public Scale _scale;
        public  PVK.Data.PVK _currentPVK;
		public  PVK_SCALES _currentScale;

		private const string NameFileHelp = "/../TRHelp 2010-08-13.chm";
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();
		[DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		private static extern int AllocConsole();
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AttachConsole(int dwProcessId);
        private readonly IMediaDataViewer _mediaDataViewer;
        //private MediaBase media;
        //private VlcControl vlcControl1;

		public MainForm()
		{
			try
			{
				string[] args = Environment.GetCommandLineArgs();
				_logger.Trace("Start");
				foreach (var commandLineArg  in args)
				{
					if (commandLineArg.ToLower().StartsWith("/console"))
					{
						if (AllocConsole() == 0)
						{
							Console.SetBufferSize(50, 32600);
							Console.SetWindowSize(50, 50);
						}
						else
						{
							AttachConsole(-1);
							Console.SetBufferSize(150, 32600);
							Console.SetWindowSize(50, 50);
						}
					}
				}
				_logger.Trace("Инициализация");
				InitializeComponent();
				dateTimePicker1.Value = DateTime.Now.Date;
				dateTimePicker2.Value = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
				if (SessionHelper.TrConf == null)
				{
					var conf = new TrConf();
					conf.Populate();
					conf.Trc.Nsi.User = CentralModule.Cnf.Trc.Nsi.User;
					conf.Trc.Nsi.Pwd = CentralModule.Cnf.Trc.Nsi.Pwd;
					SessionHelper.TrConf = conf;
				}
				_mainSession =  SessionHelper.OpenSession();
				_weighingDataTable=new DataTable("WeighingTable");
				_weighingDataTable = pvKds1.Tables["PVK_WEIGHING"];
				_modelVehicleDataTable = pvKds1.Tables["PVK_MODEL_VEHICLE"];
				_attributesDataTable = pvKds1.Tables["PVK_ATTRIBUTES"];
				_attributesValueDataTable = pvKds1.Tables["PVK_ATTR_VALUES"];
				_scalesDataTable = pvKds1.Tables["PVK_SCALES"];
				_pvkDataTable = pvKds1.Tables["PVK"];

				UpdateWeighing();
				UpdateModeVehicle();
				UpdateAttributes();
				UpdateAttrValues();																 
				UpdateScales();
				UpdatePVK();
				UpdateDamage();
				UpdatePermissionLoad();
				UpdatePermissionWeigh();
				_currentPVK = _pvkList.FirstOrDefault();
			    ConnectToCamera();

			}
			catch (Exception ex)
			{
					_logger.Error(ex.Message);
			}

		}

        private void ConnectToCamera()
        {
            try
            {
                var orDefault = _attributesValues.FirstOrDefault(t => t.Abbr == "CameraUser");
                string cameraLogin = "";
                if (orDefault != null)
                {
                    cameraLogin = orDefault.Value;
                }
                var @default = _attributesValues.FirstOrDefault(t => t.Abbr == "CameraPassword");
                string cameraPass = "";
                if (@default != null)
                {
                    cameraPass = @default.Value;
                }
                var values = _attributesValues.FirstOrDefault(t => t.Abbr == "CameraAddress");
                string cameraUrl = "";
                if (values != null)
                {
                    cameraUrl = values.Value;
                }
                //if (cameraUrl.Substring(1, 1) == ":")
                //    media = new PathMedia(cameraUrl);
                //else
                //{
                //    if (cameraLogin != "")
                //        cameraUrl = cameraUrl.Substring(0, 7) + cameraLogin + ":" + cameraPass + "@" + cameraUrl.Substring(7);
                //    media = new LocationMedia(cameraUrl);
                //}
                //var option = _attributesValues.FirstOrDefault(t => t.Abbr == "ParamVideo").Value;
                _mediadGetter = new MediaDataGetter.MediaDataGetter(cameraUrl);
                //media.AddOption(option);
                //vlcControl1 = new VlcControl();
                //vlcControl1.Media = media;
                //vlc.VideoOutput = pictureBox1;
               
            }
            catch (Exception ex)
            {
                _logger.Trace(ex.Message,ex);
            }
        }	

		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if(!_enableSaveSettings)
				return;
			//Properties.Settings.Default.DockManager1LayoutsSaved = ViewsHelper.LayoutToXmlString(dockManager1.SaveLayoutToStream);
			//Properties.Settings.Default.Save();
		}

		private void MainFormLoad(object sender, EventArgs e)
		{
			PrepareDockPanels();
			DockManagerPropertyLoad();
            var version = CentralModule.ProductVersionGet();
            this.Text += " " + version.Version + "." + version.Bild + " " + version.Date;
		}

		private void DockManagerPropertyLoad()
		{
			//if (Properties.Settings.Default.DockManager1LayoutsSaved != string.Empty)
			//    dockManager1.RestoreLayoutFromStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Properties.Settings.Default.DockManager1LayoutsSaved)));
		}

		private void PrepareDockPanels()
		{

		}

		void PrepareDockPanel(DockPanel dockPanel)
		{

		}

		private void BarButtonItem1ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
					splashScreenManager1.ShowWaitForm();
					new SyncWithServer().Synchronize(_weighingsList,_modelVehicleList,_attributesValuesList,_permissionLoadsList,_damageList);
					SaveSyncValue(_permissionLoadsList,_damageList);
					 splashScreenManager1.CloseWaitForm();
					_logger.Info("Синхронизация выполнена");
			}
			catch (Exception ex)
			{
				splashScreenManager1.CloseWaitForm();
				_logger.Error(ex.Message);
			}
			//PVK.Control.Properties.Settings.Default.Reset();
			//PVK.Control.Properties.Settings.Default.Save();
			//_enableSaveSettings = false;
			//Application.Restart();

		}

		public void SaveSyncValue(List<PVK_PERMISSION_AXIS_LOAD> _permissionLoadsList,List<PVK_DAMAGE_TS_VALUE> _damageList)
		{
			try
			{	
				Invoke(new DelegateSaveSyncValues(SaveSyncValues), _permissionLoadsList,_damageList);
			}
			catch (Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
			}
		}				  

		delegate void DelegateSaveSyncValues(List<PVK_PERMISSION_AXIS_LOAD> _permissionLoadsList,List<PVK_DAMAGE_TS_VALUE> _damageList);

		void SaveSyncValues(List<PVK_PERMISSION_AXIS_LOAD> _permissionLoadsList,List<PVK_DAMAGE_TS_VALUE> _damageList)
		{
			SaveDamage(_damageList);
			SavePermissionLoad(_permissionLoadsList);
			_mainSession.Flush();
		}

		private void SavePermissionLoad(List<PVK_PERMISSION_AXIS_LOAD> _permissionLoadsList)
		{
			foreach (PVK_PERMISSION_AXIS_LOAD pvkPermissionAxisLoad in _permissionLoadsList)
			{
				try
				{	
					_mainSession.SaveOrUpdate(pvkPermissionAxisLoad);
				}
				catch (Exception ex)
				{
					_logger.TraceException("Ошибка", ex);
				}

			}
		}

		private void SaveDamage(List<PVK_DAMAGE_TS_VALUE> _damageList)
		{
			foreach (PVK_DAMAGE_TS_VALUE pvkDamageTsValue in _damageList)
			{
				try
				{	
					 _mainSession.SaveOrUpdate(pvkDamageTsValue);
				}
				catch (Exception ex)
				{
					_logger.TraceException("Ошибка", ex);
				}

			}
		}			  

		private void BarButtonItem4ItemClick(object sender, ItemClickEventArgs e)
		{
			Close();
		}

		private void BarButtonItem2ItemClick(object sender, ItemClickEventArgs e)
		{
			if (File.Exists(Application.StartupPath + NameFileHelp))
			{
				Help.ShowHelp(this, Application.StartupPath + NameFileHelp, HelpNavigator.Topic, "welcometracereports.htm");
			}
			else
			{
				MessageBox.Show(@"Справка не найдена", @"Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void MainFormFormClosed(object sender, FormClosedEventArgs e)
		{
			if (Closed != null)
			{
				Closed(this, EventArgs.Empty);
			}
		}

		public string MainText
		{
			get { return Text; }
			set { Text = value; }
		}

		public List<PVK_PERMISSION_WEIGH> PermissionWeighList
		{
			get { return _permissionWeighList; }
			set { _permissionWeighList = value; }
		}

		public List<PVK_PERIOD_CALCULATE_DAMAGE_TS> PeriodCalcDamegeList
		{
			get { return _periodCalcDamegeList; }
			set { _periodCalcDamegeList = value; }
		}

		public IQueryable<PVK_MODEL_VEHICLE> ModelVehicle
		{
			get { return _modelVehicle; }
			set { _modelVehicle = value; }
		}

		public new event EventHandler<EventArgs> Closed;
        private MediaDataGetter.MediaDataGetter _mediadGetter;

		public void ShowView(object view)
		{
			var userControl = view as XtraUserControl;
			if (userControl != null)
			{
				var dockPanel = userControl.Parent.Parent as DockPanel;
				if (dockPanel != null)
				{
					dockPanel.Show();
				}
			}
		}

		private void simpleButton2_Click(object sender, EventArgs e)
		{
			try
			{
                GC.Collect();
                GC.WaitForPendingFinalizers();
                var weighingForm = new WeighingForm(_modelVehicleList, _attributesList, _attributesValuesList, _permissionLoadsList, this, _mainSession, _scale, _damageList);
                if (_mediadGetter != null)
                {
                    _mediadGetter.ConnectToVideo(weighingForm);
                    weighingForm._mediadGetter = _mediadGetter;
                }
                //weighingForm.panel1.Controls.Add(vlcControl1);
                //vlcControl1.Media = media;
                //vlcControl1.Play();
                //weighingForm.Media = media;
                //weighingForm.PlayVideo();
				weighingForm.UpdateMainForm += UpdateMainFormWeighing;
				weighingForm.Show();
			}
			catch (Exception ex)
            {
					_logger.ErrorException("Ошибка",ex);
			}

		}

		private void simpleButton1_Click(object sender, EventArgs e)
        {
			try{
					UpdateWeighing();
			}
			catch (Exception ex)
			{
					_logger.ErrorException("Ошибка",ex);
			}

		}

		private void bindingSource1_CurrentChanged(object sender, EventArgs e)
		{

		}

		private void UpdatePermissionLoad()
		{
            try
            {
                _permissionLoads = _mainSession.Query<PVK_PERMISSION_AXIS_LOAD>();
                _permissionLoadsList = _permissionLoads.ToList();
                _permissionLoadDataTable = UpdatePermissionLoad(_permissionLoadsList);
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message,ex);
            }
		}
		
		public DataTable UpdatePermissionLoad(List<PVK_PERMISSION_AXIS_LOAD> _modelVehicle)
		{
			try
			{
				DataTable dt = pvKds1.Tables["PVK_PERMISSION_AXIS_LOAD"];
				dt.BeginLoadData();
				dt.Clear();
				foreach (var permission in _modelVehicle)
				{
					dt.Rows.Add(permission.Id, permission.IdAdd , permission.DistanceMin,
						permission.DistanceMax , permission.PermissionValue1,
						permission.PermissionValue2, permission.PermissionValue3);
				}
				dt.AcceptChanges();
				dt.EndLoadData();
				return dt;
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
                return null;
			}
		}

		private void UpdateWeighing()
		{
            try
            {
                _mainSession.Clear();
                _weighings = _mainSession.Query<PVK_WEIGHING>().Where(t => t.DateWeighing >= dateTimePicker1.Value && t.DateWeighing <= dateTimePicker2.Value);
                _weighingsList = _weighings.ToList();
                _weighingDataTable = UpdateWeighing(_weighingsList);
                gridControl1.DataSource = _weighingDataTable;
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message, ex);
            }
		}

		private void UpdatePermissionWeigh()
		{
            try
            {
                _mainSession.Clear();
			    _permissionWeigh = _mainSession.Query<PVK_PERMISSION_WEIGH>();
			    PermissionWeighList = _permissionWeigh.ToList();
			    //_weighingDataTable = UpdateWeighing(_weighingsList);
			    //gridControl1.DataSource = _weighingDataTable;
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message, ex);
            }

		}

		private void UpdatePeriodDamageTs()
		{
            try
            {
			    _mainSession.Clear();
			    _periodCalcDamegeLoads = _mainSession.Query<PVK_PERIOD_CALCULATE_DAMAGE_TS>();
			    PeriodCalcDamegeList = _periodCalcDamegeLoads.ToList();
			    //_weighingDataTable = UpdateWeighing(_weighingsList);
			    //gridControl1.DataSource = _weighingDataTable;
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message, ex);
            }

		}


		void UpdateMainFormWeighing(object sender, EventArgs e)
		{
            try
            {
			    var tmp = ((WeighingForm)sender)._weighing;
			    //var tt ;
			    //=  _weighings.Where(t => t.Id == tmp.Id).FirstOrDefault();
			    //_mainSession.Clear();
			    //_mainSession.Evict(tmp);
			     //tt = _mainSession.CreateCriteria<PVK_WEIGHING>().Add(Restrictions.Eq("Id", tmp.Id)).List<PVK_WEIGHING>().FirstOrDefault();
			    //tt = _mainSession.Get<PVK_WEIGHING>(tmp.Id);
			    UpdateWeighing();
				     //.SetParameter("ID__",tmp.Id.ToString()).List<PVK_WEIGHING>().FirstOrDefault();	
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message, ex);
            }

		}

		public DataTable UpdateWeighing(PVK_WEIGHING _modelVehicle)
		{
            try
            {
			    var listWeiging = new List<PVK_WEIGHING>();
			    listWeiging.Add(_modelVehicle);
			    return UpdateWeighing(listWeiging);
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message, ex);
                return null;
            }

		}

		public DataTable UpdateWeighing(List<PVK_WEIGHING> _modelVehicle)
		{
            try
            {
			    DataTable dt = pvKds1.Tables["PVK_WEIGHING"].Copy();
			    dt.BeginLoadData();
			    dt.Clear();
			    foreach (var weighing in _modelVehicle)
			    {
				    dt.Rows.Add(weighing.Id, weighing.PvkAdress ,weighing.DateWeighing ,weighing.OwnerVehicle,
					    weighing.ModelVehicle == null ? "" : weighing.ModelVehicle.Model , weighing.VehicleRegNumber,
					    weighing.VehiclePodveska ==  1  || weighing.VehiclePodveska ==  2  , weighing.ModelTrailer == null ? "" : weighing.ModelTrailer.Model , 
										      weighing.TrailerRegNumber, weighing.TrailerPodveska  ==  1 || weighing.TrailerPodveska  ==  2  ,
										      weighing.RouteName, weighing.CountAxisVehicle,
										      weighing.CountAxisTrailer,
										    weighing.DistanceAxis1,
										    weighing.DistanceAxis2,
										    weighing.DistanceAxis3,
										    weighing.DistanceAxis4,
										    weighing.DistanceAxis5,
										    weighing.DistanceAxis6,
										    weighing.DistanceAxis7,
										    weighing.DistanceAxis8,
										    weighing.DistanceAxis9, 
										    weighing.DistanceAxis10,
										    weighing.DistanceAxis11,
										    weighing.DistanceAxis12,
										    weighing.DistanceAxis13,
										    weighing.DistanceAxis14,
										    weighing.DistanceAxis15,
										    weighing.DistanceAxis16,
										    weighing.DistanceAxis17,
										    weighing.DistanceAxis18,
										    weighing.DistanceAxis19,
										    weighing.DistanceAxis20,
										    weighing.DistanceAxis21,
										    weighing.DistanceAxis22,
										    weighing.DistanceAxis23,
									       weighing.DistanceAxis24,
									       weighing.ActNomer,
									       weighing.ActNarushenie == null ? "" : weighing.ActNarushenie.Value,
									       weighing.ActNarushenie  == null ? 0 :  weighing.ActNarushenie.Id,
									       weighing.ActCargo        == null ? 0 : weighing.ActCargo.Id,
									       weighing.ActCargo      == null ? "" :weighing.ActCargo.Value,
									       weighing.ActExplanationDriver,
									       weighing.ActDriver,
									       weighing.ActAsseccory,
									       weighing.ActProtokolNumber,
									       weighing.ActProtocol_date,
									       weighing.ActSubscribe,
									       weighing.ActDamadge,
									       weighing.ActDetailsPay  == null ? "" :  weighing.ActDetailsPay.Value,
										    weighing.LoadAxis10,
										    weighing.LoadAxis11,
										    weighing.LoadAxis12,
										    weighing.LoadAxis13,
										    weighing.LoadAxis14,
										    weighing.LoadAxis15,
										    weighing.LoadAxis16,
										    weighing.LoadAxis17,
										    weighing.LoadAxis18,
										    weighing.LoadAxis19,
										    weighing.LoadAxis20,
										    weighing.LoadAxis21,
										    weighing.LoadAxis22,
										    weighing.LoadAxis23,
										    weighing.LoadAxis24,
										    weighing.LoadAxis1,
										    weighing.LoadAxis2,
										    weighing.LoadAxis3,
										    weighing.LoadAxis4,
										    weighing.LoadAxis5,
										    weighing.LoadAxis6,
										    weighing.LoadAxis7,
										    weighing.LoadAxis8,
										    weighing.LoadAxis9, 									
										    weighing.FactLoadAxis10,
										    weighing.FactLoadAxis11,
										    weighing.FactLoadAxis12,
										    weighing.FactLoadAxis13,
										    weighing.FactLoadAxis14,
										    weighing.FactLoadAxis15,
										    weighing.FactLoadAxis16,
										    weighing.FactLoadAxis17,
										    weighing.FactLoadAxis18,
										    weighing.FactLoadAxis19,
										    weighing.FactLoadAxis20,
										    weighing.FactLoadAxis21,
										    weighing.FactLoadAxis22,
										    weighing.FactLoadAxis23,
										    weighing.FactLoadAxis24,
										    weighing.FactLoadAxis1,
										    weighing.FactLoadAxis2,
										    weighing.FactLoadAxis3,
										    weighing.FactLoadAxis4,
										    weighing.FactLoadAxis5,
										    weighing.FactLoadAxis6,
										    weighing.FactLoadAxis7,
										    weighing.FactLoadAxis8,
										    weighing.FactLoadAxis9, 										  
										    weighing.ActNote 
                                            ,weighing.ActInspectorPvk
                                            ,weighing.ActSpecialistPvk
                                            ,weighing.ActInspectorGIBDD
										    ,weighing.AddressOwner
                                            , weighing.PermissionWeigth + "/" + Environment.NewLine + weighing.FullWeigth
                                            ,weighing.FactLoadAxis1
                                            ,weighing.Permission
                                            ,weighing.ModelVehicle ==null? "" : weighing.ModelVehicle.Model
                                            , weighing.ModelTrailer ==null? "" : weighing.ModelTrailer.Model);
			    }
			    dt.AcceptChanges();
			    dt.EndLoadData();
			    return dt;
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message, ex);
                return null;
            }

		}

		private void gridView1_DoubleClick(object sender, EventArgs e)
		{
            try
            {
                if (gridView1.FocusedRowHandle >= 0)
                {
                    var weighingForm = new WeighingForm(_weighingsList.FirstOrDefault(t => t.Id == Convert.ToInt32(gridView1.GetDataRow(gridView1.FocusedRowHandle)["Id"])), _modelVehicleList, _attributesList, _attributesValuesList, _permissionLoadsList, this, _mainSession, _scale, _damageList);
                    weighingForm.UpdateMainForm += UpdateMainFormWeighing;
                    weighingForm.Show();
                }
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message, ex);
            }

        }

		public void UpdateAttrValues()
		{
		    try
		    {
			    _attributesValues = _mainSession.Query<PVK_ATTR_VALUES>();
			    _attributesValuesList = _attributesValues.ToList();
			    _attributesValueDataTable.BeginLoadData();
			    _attributesValueDataTable.Clear();
			    foreach (var attributesValues in _attributesValuesList)
			    {
				    _attributesValueDataTable.Rows.Add(attributesValues.Id, attributesValues.Attribute ==null? 0: attributesValues.Attribute.Id,attributesValues.Attribute ==null? "": attributesValues.Abbr != null? attributesValues.Abbr : attributesValues.Attribute.Abbr, attributesValues.Value,
					    attributesValues.Description, attributesValues.Name);
			    }
			    _attributesValueDataTable.AcceptChanges();
                _attributesValueDataTable.EndLoadData();
		    }
		    catch (Exception ex)
		    {
		        
		        _logger.TraceException("",ex.InnerException);
		    }
		}

		public DataTable UpdateAttrValues(List<PVK_ATTR_VALUES> _modelVehicle)
		{
            try
            {
			    DataTable dt = pvKds1.Tables["PVK_ATTR_VALUES"].Copy();
			    dt.BeginLoadData();
			    dt.Clear();
			    foreach (var attributesValues in _modelVehicle)
			    {
                    dt.Rows.Add(attributesValues.Id, attributesValues.Attribute.Id, attributesValues.Abbr != null ? attributesValues.Abbr : attributesValues.Attribute.Abbr, attributesValues.Value,
					    attributesValues.Description, attributesValues.Name);
			    }
			    dt.AcceptChanges();
			    dt.EndLoadData();
			    return dt;
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message, ex);
                return null;
            }

		}

		private void UpdateModeVehicle()
		{
            try
            {
			    ModelVehicle = _mainSession.Query<PVK_MODEL_VEHICLE>();
			    _modelVehicleList = ModelVehicle.ToList();
			    _modelVehicleDataTable.BeginLoadData();
			    _modelVehicleDataTable.Clear();
			    foreach (var _model in _modelVehicleList)
			    {
				    _modelVehicleDataTable.Rows.Add(_model.Id, _model.Mark, _model.Model,
					    _model.Type , _model.CountAxis,_model.AxisLength1,  _model.AxisLength2, _model.AxisLength3,
										      _model.AxisLength4, _model.AxisLength5,
										      _model.AxisLength6, _model.AxisLength7,
										      _model.AxisLength8,_model.AxisLength9,_model.AxisLength10,_model.AxisLength11,_model.AxisLength12);
			    }
			    _modelVehicleDataTable.AcceptChanges();
			    _modelVehicleDataTable.EndLoadData();
            }
            catch (Exception ex)
            {
                
                _logger.TraceException(ex.Message, ex);
            }

		}

		public DataTable UpdateModeVehicle(List<PVK_MODEL_VEHICLE> _modelVehicle)
		{
		    try
		    {
			    DataTable dt = pvKds1.Tables["PVK_MODEL_VEHICLE"].Copy();
			    dt.BeginLoadData();
			    dt.Clear();
			    foreach (var _model in _modelVehicle)
			    {
				    dt.Rows.Add(_model.Id, _model.Mark, _model.Model,
					    _model.Type , _model.CountAxis,_model.AxisLength1,  _model.AxisLength2, _model.AxisLength3,
										      _model.AxisLength4, _model.AxisLength5,
										      _model.AxisLength6, _model.AxisLength7,
										      _model.AxisLength8,_model.AxisLength9,_model.AxisLength10,_model.AxisLength11,_model.AxisLength12,_model.LinkLength);
			    }
			    dt.AcceptChanges();
			    dt.EndLoadData();
			    return dt;
		    }
		    catch (Exception ex)
		    {
		        _logger.TraceException("",ex);
		        return null;
		    }

		}

		public List<PVK_MODEL_VEHICLE> GetModeVehicle()
		{
				try
				{
					_mainSession.Clear();
					ModelVehicle = _mainSession.Query<PVK_MODEL_VEHICLE>();
					_modelVehicleList = ModelVehicle.ToList();
					return _modelVehicleList;
				}
				catch (System.Exception ex)
				{
					_logger.TraceException(ex.Message, ex);
                    return null;
				}
		}

		private void UpdateAttributes()
		{
			try
			{
				_attributes = _mainSession.Query<PVK_ATTRIBUTES>();
				_attributesList = _attributes.ToList();
				_attributesDataTable.BeginLoadData();
				_attributesDataTable.Clear();
				foreach (var attributes in _attributesList)
				{
					_attributesDataTable.Rows.Add(attributes.Id,  attributes.Name,attributes.Abbr, 	attributes.Type);
				}
				_attributesDataTable.AcceptChanges();
				_attributesDataTable.EndLoadData();
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
			}
		} 

		public DataTable UpdateAttributes(List<PVK_ATTRIBUTES> _modelVehicle)
		{
			try
			{
				DataTable dt = pvKds1.Tables["PVK_ATTRIBUTES"];
				dt.BeginLoadData();
				dt.Clear();
				foreach (var attributes in _modelVehicle)
				{
					_attributesDataTable.Rows.Add(attributes.Id,  attributes.Name,attributes.Abbr, 	attributes.Type);
				}
				dt.AcceptChanges();
				dt.EndLoadData();
				return dt;
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
                return null;
			}
		}

		private void UpdateScales()
		{
			try
			{
				_scales = _mainSession.Query<PVK_SCALES>();
				_scalesList = _scales.ToList();
				_scalesDataTable.BeginLoadData();
				_scalesDataTable.Clear();
				foreach (var scales in _scalesList)
				{
					_scalesDataTable.Rows.Add(scales.Id,scales.Nomer, scales.Name,scales.DateCkeking == null ? new DateTime() : scales.DateCkeking);
				}
				_scalesDataTable.AcceptChanges();
				_scalesDataTable.EndLoadData();
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
			}
		} 

		public DataTable UpdateScales(List<PVK_SCALES> _modelVehicle)
		{
		    try
		    {
			    UpdateScales();
				DataTable dt = pvKds1.Tables["PVK_SCALES"];
				dt.BeginLoadData();
				dt.Clear();
				foreach (var scales in _modelVehicle)
				{
					_scalesDataTable.Rows.Add(scales.Id, scales.Nomer, scales.Name, 
						scales.DateCkeking == null ? new DateTime() : scales.DateCkeking);
				}
				dt.AcceptChanges();
				dt.EndLoadData();
				return dt;
		    }
		    catch (System.Exception ex)
		    {
		    	_logger.TraceException(ex.Message, ex);
                return null;
		    }
		}

		private void UpdateDamage()
		{
			try
			{
				_damage = _mainSession.Query<PVK_DAMAGE_TS_VALUE>();
				_damageList = _damage.Where(t=> t.IdReCalc.BegDt != null && t.IdReCalc.EndDt == null).ToList();
				_damageDataTable = UpdateDamage(_damageList);
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
			}
		}

		public DataTable UpdateDamage(PVK_DAMAGE_TS_VALUE _pvk)
		{
			try
			{
				var listWeiging = new List<PVK_DAMAGE_TS_VALUE>();
				listWeiging.Add(_pvk);
				return UpdateDamage(listWeiging);
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
                return null;
			}
		}

		public DataTable UpdateDamage(List<PVK_DAMAGE_TS_VALUE> _modelVehicle)
		{
			try
			{
				DataTable dt = pvKds1.Tables["PVK_DAMAGE_TS_VALUE"];
				dt.BeginLoadData();
				dt.Clear();
				foreach (var pvk in _modelVehicle)
				{
					_pvkDataTable.Rows.Add(pvk.Id, pvk.IdReCalc, pvk.OverMin,
						pvk.OverMax , pvk.Type,
						pvk.Factor,pvk.Formula);
				}
				dt.AcceptChanges();
				dt.EndLoadData();
				return dt;
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
                return null;
			}
		}

		private void UpdatePVK()
		{
			try
			{
				_pvk = _mainSession.Query<PVK.Data.PVK>();
				_pvkList = _pvk.ToList();
				_pvkDataTable = UpdatePvk(_pvkList);
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
			}
		}

		public DataTable UpdatePvk(PVK.Data.PVK _pvk)
		{
			try
			{
				var listWeiging = new List<PVK.Data.PVK>();
				listWeiging.Add(_pvk);
				return UpdatePvk(listWeiging);
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
                return null;
			}
		}

        public DataTable UpdatePvk(List<PVK.Data.PVK> _modelVehicle)
        {
            try
            {
                DataTable dt = pvKds1.Tables["PVK"];
                dt.BeginLoadData();
                dt.Clear();
                foreach (var pvk in _modelVehicle)
                {
                    _pvkDataTable.Rows.Add(pvk.Id, pvk.Adress, pvk.Nomer,
                        pvk.Organizations, pvk.Specialist_pvk,
                        pvk.Inspector_gbdd, pvk.Inspector_pvk, pvk.Scale == null ? 0 : pvk.Scale.Id, pvk.Spring_limit == 1, pvk.CameraUrl, pvk.CameraLogin, pvk.CameraPass, pvk.NumberComPort);
                }
                dt.AcceptChanges();
                dt.EndLoadData();
                return dt;
            }
            catch (System.Exception ex)
            {
                _logger.TraceException(ex.Message, ex);
                return null;
            }
        }

		private void gridControl1_Click(object sender, EventArgs e)
		{

		}

		private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				var model = new ModelVehicleForm(ModelVehicle.Where(t =>t.Type == 1).ToList(),this,_mainSession,1);
				model.Show();
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
			}
		}

		private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
		{
            try
            {
	            var attr = new AttributesForm(_attributesValues.Where(t => t.Attribute.Abbr == "NARUSHENIE").ToList(), _attributes.ToList(), this, _mainSession);
	            attr.Text = "Характер нарушений";
				attr.Show();
            }
            catch (System.Exception ex)
            {
            	_logger.TraceException(ex.Message, ex);
            }
		} 

		private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
		{
            try
           {
	           var attr = new AttributesForm(_attributesValues.Where(t => t.Attribute.Abbr == "CARGO_TYPE").ToList(), _attributes.ToList(), this, _mainSession);
				attr.Text = "Характер грузов";
				attr.Show();
           }
           catch (System.Exception ex)
           {
           	_logger.TraceException(ex.Message, ex);
           }
		}

		private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				var model = new ModelVehicleForm(ModelVehicle.Where(t =>t.Type == 2).ToList(),this,_mainSession,2);
				model.Show();
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
			}
		}

		private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				var pvk = new PVK.Control.View.PVKForm(_pvk.ToList(),this,_mainSession);
				pvk.Show();
			}
			catch (System.Exception ex)
			{
				_logger.TraceException(ex.Message, ex);
			}
		}

		private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
		{
            var attr = new AttributesForm(_attributesValues.Where(t => t.Attribute.Abbr == "DETAIL_PAY" || t.Attribute.Abbr == "DETAIL_PAY1" || t.Attribute.Abbr == "DETAIL_PAY2" || t.Attribute.Abbr == "COEF_DAMAGE").ToList(), _attributes.ToList(), this, _mainSession);
			attr.Text = "Реквизиты и константы";
			attr.Show();
		}

		private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
		{
			var attr = new ScalesSettingsForm(_scales.ToList(),this,_mainSession);
			attr.Show();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
				var pvk = new PVK.Control.View.PVKForm(_pvkList,this,_mainSession);
				pvk.Show();
		}

		private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
		{
            var attr = new AttributesForm(_attributesValues.Where(t => t.Attribute.Abbr == "DETAIL_PAY1" || t.Attribute.Abbr == "DETAIL_PAY2").ToList(), _attributes.ToList(), this, _mainSession);
			attr.Text = "Реквизиты";
			attr.Show();
		}

		private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				string sqlLastId = "SELECT MAX(ID) FROM PVK_WEIGHING ";
				object lastIdObject = _mainSession.CreateSQLQuery(sqlLastId).UniqueResult();
				int lastId = Convert.ToInt32(lastIdObject);
				if (lastId >= int.Parse(_currentPVK.Nomer) * 1000000 && lastId < int.Parse(_currentPVK.Nomer+1)  * 1000000 )
				{
					return;
				}
				string sqlPVK_MODEL_VEHICLE = "ALTER TABLE pvk.PVK_MODEL_VEHICLE AUTO_INCREMENT=" + _currentPVK.Nomer + "000000" ;
				string sqlPVK_WEIGHING = "ALTER TABLE pvk.PVK_WEIGHING AUTO_INCREMENT=" + _currentPVK.Nomer + "000000" ;
				_mainSession.CreateSQLQuery(sqlPVK_MODEL_VEHICLE).ExecuteUpdate();
				_mainSession.CreateSQLQuery(sqlPVK_WEIGHING).ExecuteUpdate();
			_mainSession.Flush();
			}
			catch (Exception ex)
			{
				
				_logger.TraceException("",ex);
			}

		}

        private void barButtonItem12_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            try
            {
	            var attr = new AttributesForm(_attributesValues.Where(t => t.Attribute.Abbr == "SETTINGS_EUIP").ToList(), _attributes.ToList(), this, _mainSession);
	            attr.Text = _attributes.FirstOrDefault(t => t.Abbr == "SETTINGS_EUIP").Name;
	            attr.Show();
            }
            catch (System.Exception ex)
            {
            	_logger.TraceException(ex.Message, ex);
            }
        }

        internal List<PVK_ATTR_VALUES> GetAttrValues()
        {
            var _attrV = _mainSession.Query<PVK_ATTR_VALUES>();
			return  _attrV.ToList();
        }

        internal List<PVK_ATTRIBUTES> GetAttributes()
        {
            var	_attr = _mainSession.Query<PVK_ATTRIBUTES>();
			return _attr.ToList();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
	            var attr = new AttributesForm(_attributesValues.Where(t => t.Attribute.Abbr == "SETTINGS_FILTER").ToList(), _attributes.ToList(), this, _mainSession);
	            attr.Text = _attributes.FirstOrDefault(t => t.Abbr == "SETTINGS_FILTER").Name;
	            attr.Show();
            }
            catch (System.Exception ex)
            {
            	_logger.TraceException(ex.Message, ex);
            }
        }

	    private void simpleButton3_Click(object sender, EventArgs e)
	    {
	        try
	        {
	            //печать журнала ППВК
	            var ppvkReport = new PPVKReport();
	            var ds = new PVKds();
	            double fullWeigth = 0.0;
	            double permissionWeigth = 0.0;
	            string permissionAxisLoadAuto = "";
	            string factAxisLoadAuto = "";
	            string permissionAxisLoadTrailer = "";
	            string factAxisLoadTrailer = "";
	            string distanceAxis = "";
	            string str;
	            foreach (DataRow rr in UpdateWeighing(_weighingsList).Rows)
	            {
                    fullWeigth = 0.0;
                    permissionWeigth = 0.0;
                    permissionAxisLoadAuto = "";
                    factAxisLoadAuto = "";
                    permissionAxisLoadTrailer = "";
                    factAxisLoadTrailer = "";
                    distanceAxis = "";
                    for (int i = 1; i <= (int)rr["COUNT_AXIS_VEHICLE"] + (int)rr["COUNT_AXIS_TRAILER"]; i++) 
	                {
                        if (i == 1)
                            str = "";
                        else
                            str = " - ";
                        if (i > (int)rr["COUNT_AXIS_VEHICLE"])
	                    {
                            permissionAxisLoadTrailer += str + (double)rr["LOAD_AXIS_" + (i).ToString()];
                            factAxisLoadTrailer += str + (double)rr["FACT_LOAD_AXIS_" + (i).ToString()];
	                    }
	                    else
	                    {
                            permissionAxisLoadAuto += str + (double)rr["LOAD_AXIS_" + i.ToString()];
                            factAxisLoadAuto += str + (double)rr["FACT_LOAD_AXIS_" + i.ToString()];
	                    }
                    //distanceAxis += str + (double)rr["DISTANCE_AXIS_" + i.ToString()];
	                }
	                rr["OWNER_VEHICLE"] = rr["OWNER_VEHICLE"] + ", " + rr["ADDRESS_OWNER"];
                    rr["AXIS_LOAD"] = permissionAxisLoadAuto + permissionAxisLoadTrailer + "/" + Environment.NewLine + factAxisLoadAuto + factAxisLoadTrailer;
	                ds.Tables["PVK_WEIGHING"].ImportRow(rr);
	            }
	            ppvkReport.DataSource = ds.PVK_WEIGHING;
	            ppvkReport.ShowPreviewDialog(LookAndFeel);
	        }
            catch (Exception ex)
            {
                _logger.TraceException(ex.Message, ex);
            }
        }
    }

}
