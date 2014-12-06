using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NHibernate;
using PVK.Control.Presenter;
using PVK.Data;
using System.Linq;
using NLog;
using NCalc;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using MediaDataGetter;
using System.IO;
using System.Text.RegularExpressions;
using DevExpress.XtraCharts;
//using Vlc.DotNet.Core.Medias;
//using Vlc.DotNet.Core.Interops.Signatures.LibVlc.Media;
using System.Threading;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Structure;
using DevExpress.XtraEditors.Repository;

namespace PVK.Control.View
{
    public partial class WeighingForm : DevExpress.XtraEditors.XtraForm	, IMediaDataViewer
	{
		private readonly ISession _mainSession;
		private  MainForm _mainform;
		private  List<PVK_MODEL_VEHICLE> _model; 
		private  List<PVK_ATTRIBUTES> _attr;
		private  List<PVK_ATTR_VALUES> _attrValues;
		private  List<PVK_PERMISSION_AXIS_LOAD> _permissionLoad;
		private  List<PVK_DAMAGE_TS_VALUE> _damage;
		private   Scale _scale;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		private  DataTable _dataAxis;
		public  PVK_WEIGHING _weighing;
		public  string _note1;
		public  string _note2;
		public event EventHandler<EventArgs> UpdateMainForm;
		public MediaDataGetter.MediaDataGetter _mediadGetter;
		public PhotoView _photoOnFullScreen;
		private double _coefInfl = 1;
	    private bool editable = false;
        private EnmProtocol _protocol = EnmProtocol.Undefined;
        private bool bIsWeighting = false;
	    private Calculator _calc = new Calculator();
        private List<WeightRecord> _weightArray = new List<WeightRecord>();
        private List<WeightRecord> _arrWeightTemp = new List<WeightRecord>();
        private Dictionary<Int32, List<WeightRecord>> _newArrWeightTemp = new Dictionary<Int32, List<WeightRecord>>();
        private string _folderNameWeigingSave = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\PVK\\Weighing\\" ;

        PictureBox VideoOutputPicture { get; set; }

        //private MediaBase _media ;

        public delegate void UpdateDispWeight(string val);

		public WeighingForm()
		{
			InitializeComponent();
			_mainSession =  SessionHelper.OpenSession();
            labelControl6.Text = @"-1.0";
		}
		/// <summary>
		/// Создает новый экземпляр объекта взвешивания, основной для всех остальных
		/// </summary>
		/// <param name="model">Модели автомобилей и прицепов</param>
		/// <param name="attr">Существующие атрибуты (в принципе не используются)</param>
		/// <param name="attrValues">Значение атрибутов(реквизиты,типы грузов,типы нарушений...)</param>
		/// <param name="mainF">ссылка на главвную форму</param>
		public WeighingForm(List<PVK_MODEL_VEHICLE> model,List<PVK_ATTRIBUTES> attr,List<PVK_ATTR_VALUES> attrValues,MainForm mainF) : this ()
		{
		    try
		    {
		        _mainform = mainF;
			    _weighing = new PVK_WEIGHING {Pvk = mainF._currentPVK};
		        _attr = attr;
			    _model = model;
			    _attrValues = attrValues;
                new BindingSource { DataSource = mainF.UpdateAttrValues(attrValues.Where(t => t.Attribute.Abbr == "ROUTE").ToList()) };
                pVKATTRVALUESBindingSource.DataSource = mainF.UpdateAttrValues(attrValues.Where(t => t.Attribute.Abbr == "ROUTE").ToList()); //маршрут
                pVKATTRVALUESBindingSource1.DataSource = mainF.UpdateAttrValues(attrValues.Where(t => t.Attribute.Abbr == "NARUSHENIE").ToList()); //нарушение
                pVKATTRVALUESBindingSource2.DataSource = mainF.UpdateAttrValues(attrValues.Where(t => t.Attribute.Abbr == "CARGO_TYPE").ToList()); //характер груза
                pVKATTRVALUESBindingSource3.DataSource = mainF.UpdateAttrValues(attrValues.Where(t => t.Attribute.Abbr == "ROUTE_TYPE").ToList()); //тип маршрута
                pVKATTRVALUESBindingSource4.DataSource = mainF.UpdateAttrValues(attrValues.Where(t => t.Attribute.Abbr == "DETAIL_PAY").ToList()); //рекизиты
                AttrValuesTypeCargo.DataSource = mainF.UpdateAttrValues(attrValues.Where(t => t.Attribute.Abbr == "TYPE_BODY").ToList()); //рекизиты
		        var firstOrDefault = attrValues.FirstOrDefault(t => t.Attribute.Abbr == "DETAIL_PAY1");
		        if (firstOrDefault != null)
		            _note1 =  firstOrDefault.Value;
		        _note2 = "";
		        var pvkAttrValues = attrValues.FirstOrDefault(t => t.Attribute.Abbr == "COEF_DAMAGE");
		        if (pvkAttrValues != null)
		            _coefInfl =  Convert.ToDouble(pvkAttrValues.Value.Replace(".", ","));
		        pVKMODELVEHICLEBindingSource4.DataSource = 	   mainF.UpdateModeVehicle(model.Where(t=> t.Type == 1).ToList());
			    pVKMODELVEHICLEBindingSource5.DataSource = 	   mainF.UpdateModeVehicle(model.Where(t=> t.Type == 2).ToList());
			    _dataAxis = pVKds.Tables["DataAxis"];
			    _dataAxis.Rows.Add("Расстояние между осями",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
			    _dataAxis.Rows.Add("Допустимые нагрузки на ось",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
			    _dataAxis.Rows.Add("Фактические нагрузки на ось",0.0,0.0,0.0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0);
			    dataAxisBindingSource.DataSource = _dataAxis;
			    dateTimePicker1.Value = _weighing.DateWeighing <= DateTime.MinValue || _weighing.DateWeighing == null ? DateTime.Now : _weighing.DateWeighing;
			    textEdit2.Text = _mainform._currentPVK == null ? "":_mainform._currentPVK.Adress;
			    textEdit1.Text = _weighing.OwnerVehicle ;
                textEdit5.Text = _weighing.AddressOwner;
		        if (_mainform._currentPVK != null)
		        {
		            textEdit12.Text = _mainform._currentPVK.Specialist_pvk;
		            textEdit13.Text = _mainform._currentPVK.Inspector_pvk;
		            textEdit14.Text = _mainform._currentPVK.Inspector_gbdd;
		        }
		        dateEdit2.EditValue = DateTime.Now.Date;
		        var orDefault = _attrValues.FirstOrDefault(t => t.Abbr == "CameraUser");
		        string cameraLogin="";
		        if (orDefault != null)
		        {
		            cameraLogin = orDefault.Value;
		        }
		        var @default = _attrValues.FirstOrDefault(t => t.Abbr == "CameraPassword");
		        string cameraPass="";
		        if (@default != null)
		        {
		            cameraPass = @default.Value;
		        }
		        var values = _attrValues.FirstOrDefault(t => t.Abbr == "CameraAddress");
		        string cameraUrl="";
		        if (values != null)
		        {
		            cameraUrl = values.Value;
		        }
                _calc.MinCountPoint4Intraval = Convert.ToInt32(_attrValues.FirstOrDefault(t => t.Abbr == "COUNT_POINT_FILTER").Value);
                _calc.TrimMinWeight =Convert.ToDouble(_attrValues.FirstOrDefault(t => t.Abbr == "MIN_WEIGHT").Value)/1000;
                //(new System.Threading.Thread(() => _mediadGetter.GetVideoStreamFromUrl(cameraUrl, cameraLogin, cameraPass, 1, -1))).Start();
                //_mediadGetter.GetVideoStreamFromUrl(_mainform._currentPVK.CameraUrl, _mainform._currentPVK.CameraLogin, _mainform._currentPVK.CameraPass);
                //var option = _attrValues.FirstOrDefault(t => t.Abbr == "ParamVideo").Value;
			    panelControl3.Enabled = false;

			    for (int i = 1; i < 25; i++)
			    {
				    gridView1.Columns[i.ToString()].Visible = false;
			    }
                _protocol = (EnmProtocol)Convert.ToInt32(_attrValues.Where(t => t.Abbr == "Protocol").FirstOrDefault().Value.ToString());
                if (_protocol == EnmProtocol.Protocol1)
                {
                    button4.Visible = true;
                    button4.BringToFront();
                    button6.Visible = true;
                    button7.Visible = true;
                }
                else
                {
                    button6.Visible = true;
                    button6.BringToFront();
                    button4.Visible = false;
                    button7.Visible = true;
                }
		    }
		    catch (Exception ex)
		    {
		        _logger.TraceException(ex.Message,ex);
		    }
	
		}
		/// <summary>
		///  Открывает существующую форму взвешивания,
		/// </summary>
		/// <param name="weiging">Объект взвешивания который нужно загрузить на форму</param>
		/// <param name="model">Модели автомобилей и прицепов</param>
		/// <param name="attr">Существующие атрибуты (в принципе не используются)</param>
		/// <param name="attrValues">Значение атрибутов(реквизиты,типы грузов,типы нарушений...)</param>
		/// <param name="permissionLoad">Допустимые нагрузки на оси</param>
		/// <param name="mainF">ссылка на главвную форму</param>
		/// <param name="mainSession">ссылка на главвную сессию  Nhibernate</param>
		/// <param name="scale">Используемые весы </param>
		/// <param name="damage">Размуры ущербов</param>
		public WeighingForm(PVK_WEIGHING weiging,List<PVK_MODEL_VEHICLE> model,List<PVK_ATTRIBUTES> attr,List<PVK_ATTR_VALUES> attrValues,List<PVK_PERMISSION_AXIS_LOAD> permissionLoad,MainForm mainF,ISession mainSession,Scale scale,List<PVK_DAMAGE_TS_VALUE> damage):this (model,attr,attrValues,mainF)
		{
			try
			{
				_damage = damage;
				_permissionLoad = permissionLoad;
				_scale = scale;
				_scale.UpdateDisplayValue += UpdateDisplayWeigth;
			    _scale.UpdateWeightAxis += button4_Click;
				_scale.WfForm = this;
				WeighingLoad(weiging);
                panelControl2.Enabled = false;
                panelControl3.Enabled = false;
                gridControl1.Enabled = false;
                //button5.Enabled = false;
                button4.Enabled = false;
                button1.Enabled = false;
                checkEdit5.Enabled = false;
			}
			catch (Exception ex)
			{
				
				_logger.TraceException("",ex);
			}

		//_mainSession = mainSession;
		}
		/// <summary>
		/// Конструктор что бы загружать используемые весы
		/// </summary>
		/// <param name="model">Модели автомобилей и прицепов</param>
		/// <param name="attr">Существующие атрибуты (в принципе не используются)</param>
		/// <param name="attrValues">Значение атрибутов(реквизиты,типы грузов,типы нарушений...)</param>
		/// <param name="permissionLoad">Допустимые нагрузки на оси</param>
		/// <param name="mainF">ссылка на главвную форму</param>
		/// <param name="mainSession">ссылка на главвную сессию  Nhibernate</param>
		/// <param name="scale">Используемые весы</param>
		public WeighingForm(List<PVK_MODEL_VEHICLE> model,List<PVK_ATTRIBUTES> attr,List<PVK_ATTR_VALUES> attrValues,List<PVK_PERMISSION_AXIS_LOAD> permissionLoad,MainForm mainF,ISession mainSession,Scale scale)	: this (model,attr,attrValues,mainF)
		{
			try
			{
				_permissionLoad = permissionLoad;
				_scale = scale;
				if (_scale != null)
				{
					_scale.UpdateDisplayValue += UpdateDisplayWeigth;
                   // _scale.UpdateWeightAxis += button4_Click;
					_scale.WfForm = this;
				}
				//_mainSession = mainSession;	
			}
			catch (Exception ex)
			{
				_logger.TraceException(ex.Message,ex);
			}

		}

		public WeighingForm(List<PVK_MODEL_VEHICLE> model,List<PVK_ATTRIBUTES> attr,List<PVK_ATTR_VALUES> attrValues,List<PVK_PERMISSION_AXIS_LOAD> permissionLoad,MainForm mainF,ISession mainSession,Scale scale, List<PVK_DAMAGE_TS_VALUE> damage)	: this (model,attr,attrValues,permissionLoad,mainF,mainSession,scale)
		{
			_damage = damage;
            _calc.UpdateWeightAxis += SetNextWeightAxis;
			//_mainSession = mainSession;	
		}
		/// <summary>
		/// Кнопка сохранить объект в базе данных
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
  				Save(_weighing);
				_logger.Info("Сохранено успешно");
			}
			catch (Exception ex)
			{
				_logger.ErrorException("Ошибка", ex);
			}

		}
		/// <summary>
		/// Закрыть  форму взвешивания
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
            _scale.UpdateDisplayValue -= UpdateDisplayWeigth;
			this.Close();
            //vlcControl1.Stop();
            //vlcControl1 = null;
		}
		/// <summary>
		/// Закгрузить существующий объект взешивания
		/// </summary>
		/// <param name="weiging">Объект взвешивания</param>
		public void WeighingLoad(PVK_WEIGHING weiging)
		{
			try
			{
				_weighing = weiging;
				_weighing.CountAxisTrailer =    _weighing.ModelTrailer==null ? 0  : _weighing.ModelTrailer.CountAxis;
				_weighing.CountAxisVehicle =  _weighing.ModelVehicle==null ? 0 :   _weighing.ModelVehicle.CountAxis;
				//_weighing.Pvk = _mainform._currentPVK;
				 dateTimePicker1.Value = _weighing.DateWeighing;
				textEdit2.Text = _weighing.PvkAdress;
				textEdit1.Text = _weighing.OwnerVehicle ;
                textEdit5.Text = _weighing.AddressOwner;
				//searchLookUpEdit2.EditValue =  _weighing.ModelVehicle==null ? null : (int?)_weighing.ModelVehicle.Id;
				gridLookUpEdit5.EditValue =  _weighing.ModelVehicle==null ? null : (int?)_weighing.ModelVehicle.Id;
				textEdit4.Text = _weighing.VehicleRegNumber;
				//checkEdit3.Checked =   _weighing.TrailerPodveska  == 1 || _weighing.TrailerPodveska  == 2   ? true : false; 
				checkEdit3.Checked =   _weighing.TrailerPodveska  == 3 || _weighing.TrailerPodveska  == 1  ? true : false;
				//searchLookUpEdit3.EditValue =  _weighing.ModelTrailer==null ?null : (int?)_weighing.ModelTrailer.Id;  
				gridLookUpEdit7.EditValue =  _weighing.ModelTrailer==null ?null : (int?)_weighing.ModelTrailer.Id;  
				textEdit6.Text  = _weighing.TrailerRegNumber ;
                textEdit8.Text = _weighing.Permission;
				//checkEdit2.Checked =   _weighing.VehiclePodveska  == 1 || _weighing.VehiclePodveska  == 2   ? true : false;
                checkEdit2.Checked = false;
				checkEdit4.Checked =   _weighing.VehiclePodveska  == 3 || _weighing.VehiclePodveska  == 1   ? true : false;	
				textEdit3.Text = _weighing.RouteName ;
				textEdit7.Text = (_weighing.CountAxisTrailer + _weighing.CountAxisVehicle).ToString();
                textEdit5.Text = _weighing.AddressOwner;
                gridLookUpEdit4.EditValue = _weighing.CargoType == null ? null : (int?)_weighing.CargoType.Id; 
				gridView1.FocusedRowHandle = 1;
				gridView1.FocusedColumn = gridView1.Columns[1];
				if (_weighing.Photo != null && _weighing.Photo.Count() > 0)
				{
					//pictureEdit1.Image = (Image) new ImageConverter().ConvertFrom(_weighing.Photo);	

					pictureEdit1.Image = (Image ) new ImageConverter().ConvertFrom(Convert.FromBase64String(Convert.ToBase64String(_weighing.Photo)));
				}
				for (int ii = 0; ii < 3; ii++)
				{
					for (int i = 0; i < 24; i++)
					{
						_dataAxis.Rows[ii][i+1]   = ((double[])_weighing.arrayType[ii])[i];
					}
				}

			    if (_weighing.ActNomer != null && _weighing.ActNomer != "0" && _weighing.ActNomer != "")
			    {
			        panelControl3.Enabled = true;
			        LoadAct();
			        checkEdit5.Checked = true;
			    }
			    else
			    {
			        panelControl3.Enabled = false;
                    textEdit12.Text = _weighing.ActSpecialistPvk;
                    textEdit13.Text = _weighing.ActInspectorPvk;
                    textEdit14.Text = _weighing.ActInspectorGIBDD;
			    }
			}
			catch (Exception ex)
			{
				_logger.ErrorException("Ошибка",ex); 
			}

		}

        /// <summary>
        /// Воспроизвести
        /// </summary>
        /// <param name="weiging"></param>
        public void PlayVideo()
        {
            try
            {
                //vlcControl1.Media = _media;
                //vlcControl1.Play();
            }
            catch (Exception ex)
            {
                _logger.TraceException(ex.Message, ex);
            }
        }

        /// <summary>
		 /// Сохранить объкт взвешивания в базе
		 /// </summary>
		 /// <param name="weiging"></param>
		public void Save(PVK_WEIGHING weiging)
		{
			try
			{
					var _weiging1 = GetPVK_WEIGHING();
					//_mainSession.Evict(_weiging1);
					if (weiging != null && weiging.Id != 0)
					{
						_weiging1.Id = weiging.Id;
						_mainSession.Update(_weiging1);
					}
					else
						_mainSession.Save(_weiging1);
					this._weighing = _weiging1;
					_mainSession.Flush();
					if 	(UpdateMainForm != null)
						UpdateMainForm(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				 _logger.ErrorException("Ошибка",ex); 
			}

		}

		public void CreateWeighing( )
		{

		}
		/// <summary>
		/// Считать значения с формы и вернуть новый обект взвешивания
		/// </summary>
		/// <returns>Оъект взвешивания</returns>
		private PVK_WEIGHING GetPVK_WEIGHING()
		{
			try
			{
					//DataRowView row = (DataRowView)searchLookUpEdit2.GetSelectedDataRow();
					PVK_WEIGHING _wg = new PVK_WEIGHING();
					if (this._weighing != null)
						_wg = this._weighing;
					object[] temp = new object[3];
					double[] dt ;
                    temp[0] = new double[25];
                    temp[1] = new double[25];
                    temp[2] = new double[25];
					for (int ii = 0; ii < 3; ii++)
					{
						dt = new double[25];
						for (int i = 0; i < 24; i++)
						{
							dt[i] = _dataAxis.Rows[ii][i+1] is DBNull ? 0 : (double)_dataAxis.Rows[ii][i+1];
						}
						temp[ii] = dt;
					}
					_wg.arrayType = temp; 
					//_wg.CountAxisVehicle = searchLookUpEdit2.EditValue == null  ||searchLookUpEdit2.EditValue.ToString() == "" ? 0 : _model.Where(t=> t.Id == Convert.ToInt32(searchLookUpEdit2.EditValue)).FirstOrDefault().CountAxis;
					_wg.CountAxisVehicle = gridLookUpEdit5.EditValue == null  ||gridLookUpEdit5.EditValue.ToString() == "" ? 0 : _model.Where(t=> t.Id == Convert.ToInt32(gridLookUpEdit5.EditValue)).FirstOrDefault().CountAxis;
					//_wg.CountAxisTrailer =  searchLookUpEdit3.EditValue == null || searchLookUpEdit3.EditValue.ToString() == "" ? 0 : _model.Where(t=> t.Id == Convert.ToInt32(searchLookUpEdit3.EditValue)).FirstOrDefault().CountAxis;
					_wg.CountAxisTrailer =  gridLookUpEdit7.EditValue == null || gridLookUpEdit7.EditValue.ToString() == "" ? 0 : _model.Where(t=> t.Id == Convert.ToInt32(gridLookUpEdit7.EditValue)).FirstOrDefault().CountAxis;
					_wg.DateWeighing = dateTimePicker1.Value;
					_wg.TrailerPodveska = checkEdit3.Checked ? 1 : 2   ;
					_wg.VehiclePodveska = checkEdit4.Checked ? 1 : 2  ;
					_wg.Pvk = _mainform._currentPVK;
					_wg.PvkAdress = textEdit2.Text == "" ? null : textEdit2.Text;
					_wg.VehicleRegNumber = textEdit4.Text== "" ? null: textEdit4.Text ;
					_wg.TrailerRegNumber = textEdit6.Text== "" ? null: textEdit6.Text ;
					_wg.ActNomer = textEdit9.Text == "" ? null : textEdit9.Text;
					_wg.ActCargo = _attrValues.FirstOrDefault(t => t.Id == Convert.ToInt32(gridLookUpEdit2.EditValue));
					//_wg.ModelTrailer = searchLookUpEdit3.EditValue == null ||searchLookUpEdit3.EditValue.ToString() == "" ? null  : _model.FirstOrDefault(t => t.Id ==  Convert.ToInt32(searchLookUpEdit3.EditValue));
					_wg.ModelTrailer = gridLookUpEdit7.EditValue == null ||gridLookUpEdit7.EditValue.ToString() == "" ? null  : _model.FirstOrDefault(t => t.Id ==  Convert.ToInt32(gridLookUpEdit7.EditValue));
					//_wg.ModelVehicle = searchLookUpEdit2.EditValue == null  ||searchLookUpEdit2.EditValue.ToString() == "" ? null : _model.FirstOrDefault(t => t.Id ==  Convert.ToInt32(searchLookUpEdit2.EditValue));
					_wg.ModelVehicle = gridLookUpEdit5.EditValue == null  ||gridLookUpEdit5.EditValue.ToString() == "" ? null : _model.FirstOrDefault(t => t.Id ==  Convert.ToInt32(gridLookUpEdit5.EditValue));
					_wg.ActDamadge = textEdit11.Text == "" ? 0.0 : double.Parse(textEdit11.Text);
					_wg.ActExplanationDriver = textEdit18.Text == "" ? null : textEdit18.Text;
					_wg.ActProtokolNumber = textEdit17.Text == "" ? null : textEdit17.Text;
					_wg.ActProtocol_date = dateEdit2.DateTime;
                    //_wg.ActDetailsPay = _attrValues.FirstOrDefault(t => t.Id == Convert.ToInt32(gridLookUpEdit4.EditValue));
                    //_wg.ActNote = memoEdit2.Text == "" ? null:  memoEdit2.Text;
					_wg.ActDriver = textEdit15.Text== "" ? null:  textEdit15.Text;
					_wg.OwnerVehicle = textEdit1.Text== "" ? null:  textEdit1.Text;
                    _wg.AddressOwner = textEdit5.Text == "" ? null : textEdit5.Text;
					_wg.ActNarushenie = _attrValues.FirstOrDefault(t => t.Id == Convert.ToInt32(gridLookUpEdit3.EditValue));
					_wg.ActRoute = _wg.RouteName;
                    _wg.Permission = textEdit8.Text ;
					 //_wg.ActSubscribe  = textEdit19.Text== "" ? null:  textEdit19.Text;
					_wg.ActAsseccory  = textEdit10.Text== "" ? null:  textEdit10.Text;	 
					_wg.ActInspectorPvk  =     textEdit12.Text == "" ? null: textEdit12.Text ;
					_wg.ActSpecialistPvk = textEdit13.Text== "" ? null:  textEdit13.Text;
					_wg.ActInspectorGIBDD = textEdit14.Text == "" ? null: textEdit14.Text ;
					_wg.RouteName = textEdit3.Text == "" ? null : textEdit3.Text;
                   _wg.CargoType = gridLookUpEdit4.EditValue == null  ||gridLookUpEdit4.EditValue.ToString() == "" ? null : _attrValues.Where(t => t.Id ==  Convert.ToInt32(gridLookUpEdit4.EditValue)).FirstOrDefault() ; 
					return _wg;
			}
			catch (Exception ex)
			{
				_logger.ErrorException("Ошибка",ex);
                return null;
			}

		}
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="sender"></param>
		 /// <param name="e"></param>
		private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
		{

		}
		/// <summary>
		/// Печать акта
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{
			try
			{
				Save(_weighing);
			//печать акт
			var reportAct =new  ActReport();
			var ds = new PVKds();
			ds.Tables["PVK"].ImportRow(_mainform.UpdatePvk(_weighing.Pvk).Rows[0]) ;
			ds.Tables["PVK_WEIGHING"].ImportRow(_mainform.UpdateWeighing(_weighing).Rows[0]) ;
			ds.Tables["PVK_SCALES"].ImportRow(_mainform.UpdateScales(_mainform._scalesList).Rows[0]) ;
			double fullWeigth = 0.0;
			double permissionWeigth = 0.0;
			string permissionAxisLoadAuto = "";
			string factAxisLoadAuto = "";  
			string permissionAxisLoadTrailer = "";
			string factAxisLoadTrailer = "";
			string distanceAxis = "";
			string str;
            reportAct.pictureBox1.Image = _weighing.Photo == null ? null:(Image)new ImageConverter().ConvertFrom(Convert.FromBase64String(Convert.ToBase64String(_weighing.Photo)));
			reportAct.Parameters["ActNote"].Value = _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "ACT_NOTE") != null ? _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "ACT_NOTE").Value: "";
			for (int i = 1; i <= _weighing.CountAxis; i++)
			{
				if (i > _weighing.CountAxisVehicle)
				{
						 permissionAxisLoadTrailer += "- " + _weighing.LoadAxis[i -1];
						  factAxisLoadTrailer +=  "- "+ _weighing.FactLoadAxis[i -1];
				}
				else
				{
					  	permissionAxisLoadAuto += "- " + _weighing.LoadAxis[i -1];
						factAxisLoadAuto +=  "- "+ _weighing.FactLoadAxis[i -1];
				}
				if (i == 1 || i == _weighing.CountAxis)
					str = "";
				else
					str = " - ";
				distanceAxis += str +  _weighing.DistanceAxis[i -1].ToString() ;
			}
            fullWeigth = _weighing.FullWeigth;
            permissionWeigth = _weighing.PermissionWeigth;
			reportAct.Parameters["FactAxisLoad"].Value = String.Format("Авт: {0} Прицеп: {1}", factAxisLoadAuto,factAxisLoadTrailer);
            reportAct.Parameters["routeName"].Value = String.Format("{0}(S={1} км)", ds.Tables["PVK_WEIGHING"].Rows[0]["ROUTE_NAME"], ds.Tables["PVK_WEIGHING"].Rows[0]["ACT_ASSECCORY"]);
			reportAct.Parameters["PermissionAxisLoad"].Value = String.Format("Авт: {0} Прицеп: {1}", permissionAxisLoadAuto,permissionAxisLoadTrailer);
			reportAct.Parameters["FullWeigth"].Value = String.Format("{0}", fullWeigth);
			reportAct.Parameters["PermisionWeigth"].Value = String.Format("{0}", permissionWeigth);	
			reportAct.Parameters["distanceAxis"].Value = String.Format("{0}", distanceAxis);
			reportAct.Parameters["CurrDate"].Value = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
			reportAct.Parameters["DateProtocol"].Value = _weighing.ActProtocol_date.ToString("dd.MM.yyyy");
			reportAct.Parameters["ActNote1"].Value = _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "DETAIL_PAY1") != null ? _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "DETAIL_PAY1").Value: "";
            reportAct.Parameters["ActNote2"].Value = _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "DETAIL_PAY") != null ? _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "DETAIL_PAY").Value : "";
            reportAct.Parameters["ActNote"].Value = _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "DETAIL_PAY4") != null ? _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "DETAIL_PAY4").Value : "";
            reportAct.Parameters["Note"].Value = _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "DETAIL_PAY3") != null ? _attrValues.FirstOrDefault(t => t.Attribute.Abbr == "DETAIL_PAY3").Value : ""; ;
			reportAct.DataSource = ds;
			reportAct.ShowPreviewDialog(LookAndFeel);
			}
			catch (Exception ex)
			{
				_logger.ErrorException("Ошибка",ex);
			}

		}
		
	 	/// <summary>
	 	/// Обновления расстояний между осями, вызывать когда меняться модель авто
	 	/// </summary>
		private void UpdateDistanceAxis()
		{
			try
			{
				DataRow drDistnace  = _dataAxis.NewRow();
				int countAxis = 0;
					
				if 	(_weighing.ModelVehicle != null)
				{
					for (int i = 1; i < _weighing.ModelVehicle.CountAxis + 1; i++)
					{
						drDistnace[i.ToString()] = _weighing.ModelVehicle.AxisLength[i - 1];
					}
					countAxis += _weighing.ModelVehicle.CountAxis;
				}
				if 	 (_weighing.ModelTrailer != null && _weighing.ModelVehicle != null &&  _weighing.ModelVehicle.CountAxis > 0 )
				{
					drDistnace[( _weighing.ModelVehicle.CountAxis).ToString()] = _weighing.ModelTrailer.LinkLength;
					for (int i = 1 +  _weighing.ModelVehicle.CountAxis; i < _weighing.ModelTrailer.CountAxis + 1 + _weighing.ModelVehicle.CountAxis; i++)
					{
						drDistnace[i.ToString()] = _weighing.ModelTrailer.AxisLength[i - 1 -  _weighing.ModelVehicle.CountAxis];
					}
					countAxis += _weighing.ModelTrailer.CountAxis;
				}
				else if (_weighing.ModelTrailer != null && _weighing.ModelVehicle == null)
					_logger.ErrorException("Ошибка",new Exception("Необходимо сначала выбрать тягач"));
				for (int i = 1; i < 25; i++) 
					gridView1.Columns[i.ToString()].Visible = false;
				gridView1.Columns[0].VisibleIndex = 0;
				gridView1.Columns["FullWeigth"].Visible = false;
				for (int i = 1; i <   1+ countAxis ; i++)
					gridView1.Columns[i.ToString()].Visible = true;
				for (int i = 1; i < countAxis + 1 ; i++)
						gridView1.Columns[i.ToString()].VisibleIndex = i;
				for (int ii = 1; ii < 25 ; ii++ )
						_dataAxis.Rows[0][ii.ToString()] = drDistnace[ii.ToString()];
				AdjustGridControlSize();
				if (_weighing.CountAxis > 0)
				{
					gridView1.Columns["FullWeigth"].VisibleIndex = countAxis + 1;
					gridView1.Columns["FullWeigth"].Visible = true;
				}
					
				_weighing.CountAxisTrailer = _weighing.ModelTrailer == null ? 0 : _weighing.ModelTrailer.CountAxis;
				_weighing.CountAxisVehicle =  _weighing.ModelVehicle == null ? 0 : _weighing.ModelVehicle.CountAxis; 
				textEdit7.Text = (_weighing.CountAxisTrailer + _weighing.CountAxisVehicle).ToString();
				CalcPermissionLoad();
				AdjustGridControlSize();
				gridView1.FocusedColumn = gridView1.Columns[1];
				gridView1.FocusedRowHandle = 2;
			}
			catch (Exception ex)
			{
				_logger.ErrorException("Ошибка",ex);
			}

		}


        /// <summary>
        /// Обновления расстояний между осями, вызывать когда меняться модель авто
        /// </summary>
        private void UpdateDistanceAxisNew()
        {
            try
            {
                DataRow drDistnace = _dataAxis.NewRow();
                int countAxis = _weighing.ModelVehicle == null ? 0 : _weighing.ModelVehicle.CountAxis + (_weighing.ModelTrailer == null ? 0 : _weighing.ModelTrailer.CountAxis);
                for (int i = 1; i < 25; i++)
                    gridView1.Columns[i.ToString()].Visible = false;
                gridView1.Columns[0].VisibleIndex = 0;
                gridView1.Columns["FullWeigth"].Visible = false;
                for (int i = 1; i < 1 + countAxis; i++)
                    gridView1.Columns[i.ToString()].Visible = true;
                for (int i = 1; i < countAxis + 1; i++)
                    gridView1.Columns[i.ToString()].VisibleIndex = i;
                //for (int ii = 1; ii < 25; ii++)
                //    _dataAxis.Rows[0][ii.ToString()] = drDistnace[ii.ToString()];
                //AdjustGridControlSize();
                if (_weighing.CountAxis > 0)
                {
                    gridView1.Columns["FullWeigth"].VisibleIndex = countAxis + 1;
                    gridView1.Columns["FullWeigth"].Visible = true;
                }
                _weighing.CountAxisTrailer = _weighing.ModelTrailer == null ? 0 : _weighing.ModelTrailer.CountAxis;
                _weighing.CountAxisVehicle = _weighing.ModelVehicle == null ? 0 : _weighing.ModelVehicle.CountAxis;
                textEdit7.Text = (_weighing.CountAxisTrailer + _weighing.CountAxisVehicle).ToString();
                CalcPermissionLoad();
                AdjustGridControlSize();
                gridView1.FocusedColumn = gridView1.Columns[1];
                gridView1.FocusedRowHandle = 2;
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Ошибка", ex);
            }
        }

        private void UpdateDistanceAxisVehicle()
        {
            try
            {
                DataRow drDistnace = _dataAxis.NewRow();
                int countAxis = 0;

                if (_weighing.ModelVehicle != null)
                {
                    for (int i = 1; i < _weighing.ModelVehicle.CountAxis + 1; i++)
                    {
                        drDistnace[i.ToString()] = _weighing.ModelVehicle.AxisLength[i - 1];
                    }
                    countAxis += _weighing.ModelVehicle.CountAxis;
                }
                for (int ii = 1; ii <= countAxis; ii++)
                    _dataAxis.Rows[0][ii.ToString()] = drDistnace[ii.ToString()];
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Ошибка", ex);
            }
        }

        private void UpdateDistanceAxisTrailer()
        {
            try
            {
                DataRow drDistnace = _dataAxis.NewRow();
                int countAxis = _weighing.ModelVehicle == null ? 0 : _weighing.ModelVehicle.CountAxis;
                if (_weighing.ModelTrailer != null && _weighing.ModelVehicle != null && _weighing.ModelVehicle.CountAxis > 0)
                {
                    drDistnace[(_weighing.ModelVehicle.CountAxis).ToString()] = _weighing.ModelTrailer.LinkLength;
                    for (int i = 1 + _weighing.ModelVehicle.CountAxis; i < _weighing.ModelTrailer.CountAxis + 1 + _weighing.ModelVehicle.CountAxis; i++)
                    {
                        drDistnace[i.ToString()] = _weighing.ModelTrailer.AxisLength[i - 1 - _weighing.ModelVehicle.CountAxis];
                    }
                    countAxis += _weighing.ModelTrailer.CountAxis;
                }
                else if (_weighing.ModelTrailer != null && _weighing.ModelVehicle == null)
                    _logger.ErrorException("Ошибка", new Exception("Необходимо сначала выбрать тягач"));

                if (_weighing.ModelVehicle == null)
                    return;

                for (int ii = _weighing.ModelVehicle.CountAxis; ii <= countAxis; ii++)
                    _dataAxis.Rows[0][ii.ToString()] = drDistnace[ii.ToString()];
            }
            catch (Exception ex)
            {
                _logger.ErrorException("Ошибка", ex);
            }
        }

		private void searchLookUpEdit1_AddNewValue(object sender, DevExpress.XtraEditors.Controls.AddNewValueEventArgs e)
		{

		}

		private void searchLookUpEdit2_AddNewValue(object sender, DevExpress.XtraEditors.Controls.AddNewValueEventArgs e)		
		{
			var model = new ModelVehicleForm(_model.Where(t =>t.Type == 1).ToList(),_mainform,_mainSession,1);
			model.UpdateModel += UpdateModels;
			model.Show();
			model.Activate();
		}



		/// <summary>
		/// Рассчет допустимых нагрузок
		/// </summary>
		public void CalcPermissionLoad()
		{
			try
			{
					if (_permissionLoad == null)
						return;
					double dist = 0.0;
					for (int i = 1; i < _weighing.CountAxis + 1; i++)
					{
						//определяем минимальное расстояние до ближайшей оси
						if (i == 1)
							dist = _dataAxis.Rows[0][(i).ToString()] is DBNull ? 0 : Convert.ToDouble(_dataAxis.Rows[0][(i).ToString()]);
						else if (i == _weighing.CountAxis)
							dist = _dataAxis.Rows[0][(i - 1).ToString()] is DBNull ? 0 : Convert.ToDouble(_dataAxis.Rows[0][(i - 1).ToString()]);
						else
							dist = _dataAxis.Rows[0][(i).ToString()] is DBNull ? 0 : Convert.ToDouble(_dataAxis.Rows[0][(i).ToString()]) < Convert.ToDouble(_dataAxis.Rows[0][(i - 1).ToString()]) ? Convert.ToDouble(_dataAxis.Rows[0][(i).ToString()]) : Convert.ToDouble(_dataAxis.Rows[0][(i - 1).ToString()]);
						//определяем допустимую нагрузку
						if ( _mainform._currentPVK.Spring_limit > 0)
							 _dataAxis.Rows[1][(i).ToString()]= _permissionLoad.Where(t => t.DistanceMin <= dist && dist <=t.DistanceMax).Select(tt => tt.PermissionValue3).FirstOrDefault();
						else if (_weighing.TrailerPodveska == 1 && i > _weighing.ModelVehicle.CountAxis  && i <= _weighing.CountAxis)
							 _dataAxis.Rows[1][(i).ToString()]= _permissionLoad.Where(t => t.DistanceMin <= dist && dist <=t.DistanceMax).Select(tt => tt.PermissionValue2).FirstOrDefault();
						else  if (_weighing.VehiclePodveska == 1 && i <= _weighing.ModelVehicle.CountAxis)
							 _dataAxis.Rows[1][(i).ToString()]= _permissionLoad.Where(t => t.DistanceMin <= dist && dist <=t.DistanceMax).Select(tt => tt.PermissionValue2).FirstOrDefault();
						else
							 _dataAxis.Rows[1][(i).ToString()]= _permissionLoad.Where(t => t.DistanceMin <= dist && dist <=t.DistanceMax).Select(tt => tt.PermissionValue1).FirstOrDefault();
					}
				if (_mainform._currentPVK.Spring_limit > 0)
				{
                    if (_weighing.ModelVehicle != null && _weighing.ModelVehicle.CountAxis > 0 && _weighing.ModelTrailer != null && _weighing.ModelTrailer.CountAxis > 0)
						_dataAxis.Rows[1]["FullWeigth"] = _mainform.PermissionWeighList.Where(t => t.CountAxis <= _weighing.CountAxis && ((DateTime.Now > t.Period.BegDt && DateTime.Now < t.Period.EndDt) || (DateTime.Now > t.Period.BegDt && t.Period.EndDt == null))).OrderByDescending(t => t.ValueTrailer).Select(t => t.ValueTrailer).FirstOrDefault();
					else if (_weighing.ModelVehicle.CountAxis > 0)
						_dataAxis.Rows[1]["FullWeigth"] = _mainform.PermissionWeighList.Where(t => t.CountAxis <= _weighing.CountAxis && ((DateTime.Now > t.Period.BegDt && DateTime.Now < t.Period.EndDt) || (DateTime.Now > t.Period.BegDt && t.Period.EndDt == null))).OrderByDescending(t => t.ValueAuto).Select(t => t.ValueAuto).FirstOrDefault();
				}
				else
				{
                    if (_weighing.ModelVehicle != null && _weighing.ModelVehicle.CountAxis > 0 && _weighing.ModelTrailer != null && _weighing.ModelTrailer.CountAxis > 0)
						_dataAxis.Rows[1]["FullWeigth"] = _mainform.PermissionWeighList.Where(t => t.CountAxis <= _weighing.CountAxis && ((DateTime.Now > t.Period.BegDt && DateTime.Now < t.Period.EndDt) || (DateTime.Now > t.Period.BegDt && t.Period.EndDt == null))).OrderByDescending(t => t.ValueTrailer).Select(t => t.ValueTrailer).FirstOrDefault();
                    else if (_weighing.ModelVehicle != null &&  _weighing.ModelVehicle.CountAxis > 0)
						_dataAxis.Rows[1]["FullWeigth"] = _mainform.PermissionWeighList.Where(t => t.CountAxis <= _weighing.CountAxis && ((DateTime.Now > t.Period.BegDt && DateTime.Now < t.Period.EndDt) || (DateTime.Now > t.Period.BegDt && t.Period.EndDt == null))).OrderByDescending(t => t.ValueAuto).Select(t => t.ValueAuto).FirstOrDefault();
				}
				_weighing.PermissionWeigth =_dataAxis.Rows[1]["FullWeigth"] is DBNull ? 0: Convert.ToDouble(_dataAxis.Rows[1]["FullWeigth"]);
			}
			catch (Exception ex)
			{
				_logger.ErrorException("Ошибка",ex);
			}
		}

		private void gridLookUpEdit4_EditValueChanged(object sender, EventArgs e)
		{

		}

		private void textEdit3_EditValueChanged(object sender, EventArgs e)
		{

		}
		/// <summary>
		///  Обновление показаний весов
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpdateDisplayWeigth(object sender, EventArgs e)
		{
            try
            {
                WeightRecord t = new WeightRecord();
                t.Weight = ((Scale) sender).currValue;
                t.Dt = DateTime.Now;
                lock (_weightArray)
                {
                    _weightArray.Add(t);
                }
                //digitalGauge1.Text = t.Weight.ToString();
                if (this.InvokeRequired && !this.Disposing)
                    Invoke(new UpdateDispWeight(UpdateDisplWeigth), t.Weight.ToString());
            }
            catch (Exception ex)
            {
                _logger.TraceException("",ex);
            }
		}

        private void UpdateDisplWeigth(string value)
        {
            try
            {
                labelControl6.Text = value;
                //digitalGauge1.Text = value;
            }
            catch (Exception ex)
            {
                
            }
        }

		private void WeighingForm_FormClosed(object sender, FormClosedEventArgs e)
		{
		    try
		    {
                if (_mediadGetter != null)
                    _mediadGetter.DisconnectFromVideo(this);
                if (ViewsHelper.ShowWarn("Сохранить взвешивание?", LookAndFeel) == DialogResult.Yes)
                {
                    Save(_weighing);
                }
                _scale.UpdateDisplayValue -= UpdateDisplayWeigth;
                _scale = null;
                //vlcControl1.Stop();
		    }
		    catch (Exception ex)
		    {
		        _logger.Trace(ex.Message);
		    }

		}
		/// <summary>
		/// Добаление новой модели автомобиля
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchLookUpEdit3_AddNewValue(object sender, DevExpress.XtraEditors.Controls.AddNewValueEventArgs e)
		{
			var model = new ModelVehicleForm(_model.Where(t =>t.Type == 2).ToList(),_mainform,_mainSession,2);
			model.UpdateModel += UpdateModels;
			this.SendToBack();
			this.Enabled = false;
			model.Show();
			model.Activate(); 
		}

		private void UpdateModels(object sender, EventArgs e)
		{
			pVKMODELVEHICLEBindingSource4.DataSource = _mainform.UpdateModeVehicle(_mainform.ModelVehicle.Where(t => t.Type == 1).ToList());
			pVKMODELVEHICLEBindingSource5.DataSource = _mainform.UpdateModeVehicle(_mainform.ModelVehicle.Where(t => t.Type == 2).ToList());
			this._model = _mainform.GetModeVehicle();
            _weighing.ModelTrailer = _weighing.ModelTrailer==null ? null : _model.FirstOrDefault(t => t.Id == _weighing.ModelTrailer.Id);
            _weighing.ModelVehicle = _weighing.ModelVehicle == null ? null : _model.FirstOrDefault(t => t.Id == _weighing.ModelVehicle.Id);
            UpdateDistanceAxis();
			CalcPermissionLoad();
			this.Enabled = true;
			this.Activate();
		}

		private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
		{
			UpdateFullWeigth();
		}

		private void UpdateFullWeigth()
		{
			double fullWeiging = 0;
			//double permissionWeiging = 0;
			//for (int i = 0; i < 19; i++)
			//{
			//    permissionWeiging += _dataAxis.Rows[1][i + 1] is DBNull ? 0 : (double)_dataAxis.Rows[1][i + 1];
			//}
			//_dataAxis.Rows[1]["FullWeigth"] = permissionWeiging;
			for (int i = 0; i < 19; i++)
			{
				fullWeiging += _dataAxis.Rows[2][i + 1] is DBNull ? 0 : (double)_dataAxis.Rows[2][i + 1];
			}
			_dataAxis.Rows[2]["FullWeigth"] =  fullWeiging;
			gridView1.RefreshData();
            textEdit11.Text = Math.Round((CalcDamageValue() * (Convert.ToDouble(textEdit10.Text == "" ? "100" : textEdit10.Text) / 100.0)), 2).ToString();
		}

		private void AdjustGridControlSize()
          {
               int width = 0;
               foreach (GridColumn col in this.gridView1.VisibleColumns)
               {
                   if (col.Name != "colType")
                        col.BestFit();
                   width += col.Width;
               }
               gridControl1.Width = width - 5 ;
               //gridView1.BestFitColumns();
			   //col.Width = 100;
          }

		private void checkEdit5_CheckedChanged(object sender, EventArgs e)
		{
			panelControl3.Enabled = checkEdit5.Checked;
			if (panelControl3.Enabled)
				LoadAct();
		}


		public void LoadAct()
		{
			try
			{
				if (_weighing.ActNomer == null || _weighing.ActNomer == "0" || _weighing.ActNomer == "")
				{
					if (_weighing.Id == 0)
						Save(_weighing);
                    //_weighing.ActNomer = _mainform._currentPVK.Nomer + (_weighing.Id % 100000).ToString("00000");
                    _weighing.ActNomer = _mainform._currentPVK.Nomer + GetNewNumberProtocol(_weighing.ActProtocol_date).ToString("0000");
					textEdit12.Text = _mainform._currentPVK.Specialist_pvk;
					textEdit13.Text = _mainform._currentPVK.Inspector_pvk  ;
					textEdit14.Text = _mainform._currentPVK.Inspector_gbdd  ;
				}
				else
				{
					textEdit12.Text = _weighing.ActSpecialistPvk;
					textEdit13.Text = _weighing.ActInspectorPvk  ;
					textEdit14.Text = _weighing.ActInspectorGIBDD  ;
				}
                //if(_weighing.ActProtokolNumber == null || _weighing.ActProtokolNumber == "0" || _weighing.ActProtokolNumber == "" )
                //    _weighing.ActProtokolNumber = (_weighing.Id % 10000).ToString("00000");
                //if (_weighing.ActProtokolNumber == null)
                //     _weighing.ActProtokolNumber = GetNewNumberProtocol(_weighing.ActProtocol_date).ToString("00000");
				textEdit9.Text = _weighing.ActNomer;
				gridLookUpEdit3.EditValue =  _weighing.ActNarushenie==null ?null : (int?) _weighing.ActNarushenie.Id;
				gridLookUpEdit2.EditValue =  _weighing.ActCargo==null ?null : (int?) _weighing.ActCargo.Id;
				textEdit10.Text = _weighing.ActAsseccory ;
				textEdit11.Text = _weighing.ActDamadge.ToString();	
				textEdit15.Text = _weighing.ActDriver ;
				//textEdit19.Text = _weighing.ActSubscribe ;
				textEdit18.Text = _weighing.ActExplanationDriver ;
				textEdit17.Text = _weighing.ActProtokolNumber ;
				dateEdit2.DateTime = _weighing.ActProtocol_date ;
                //memoEdit2.Text = _weighing.ActNote ; 
                //gridLookUpEdit4.EditValue =  _weighing.ActDetailsPay==null ? _attrValues.Where(t => t.Attribute.Abbr == "DETAIL_PAY").FirstOrDefault().Id : (int?) _weighing.ActDetailsPay.Id;

				textEdit11.Text = Math.Round((CalcDamageValue() * (Convert.ToDouble(textEdit10.Text == "" ? "100" : textEdit10.Text) / 100.0)),2).ToString();
				//panelControl3.Refresh();
				UpdateFullWeigth();
			}
			catch (Exception ex)
			{
				
				_logger.ErrorException("", ex);
			}

		}

		public double  CalcDamageValue()
		{
			try
			{
					//Расчет ущерба
				double damageFullWeigth = 0.0;
				double[] damageAxis = new double[24];
				double fullDamage = 0.0;
				double overweigth =  _weighing.FullWeigth - _weighing.PermissionWeigth;
				string formula;
				string pattern = "[(].*?[)]...";
				Regex rgx = new Regex(pattern,RegexOptions.IgnorePatternWhitespace);
				if (overweigth > 0)
				{
					//overweigth = (overweigth / _weighing.PermissionWeigth) * 100;
					formula = _damage.Where(t => t.OverMax > overweigth && t.OverMin <= overweigth && t.Type == "М").Select(tt => tt.Formula).First();
					formula = formula.Replace(",", ".");
					if (formula.Contains("М"))
					{
						foreach (var mat in rgx.Matches(formula))
						{
							if (mat.ToString().Contains("**"))
							{
								formula = formula.Replace(mat.ToString(), "Pow(" + mat.ToString().Remove(mat.ToString().ToList().Count - 3, 3) + "," + mat.ToString().ToList().Last() + ")");
							}
						}
						formula = formula.Replace("#М#**2", "#М#*#М#");
						formula = formula.Replace("#М#**3","#М#*#М#*#М#");
                        formula = formula.Replace("#М#", overweigth.ToString(CultureInfo.InvariantCulture).Replace(",", "."));
						damageFullWeigth = Convert.ToDouble(new Expression(formula).Evaluate());
						//damageFullWeigth  = Convert.ToDouble(new DataTable().Compute(formula.Replace(",","."), null));
					}
					else
					{
						  damageFullWeigth  = Convert.ToDouble(formula);
					}
					fullDamage += damageFullWeigth * _coefInfl; 
				}
				for (int i = 0; i < _weighing.CountAxis ; i++)
				{
                    overweigth = (double)_dataAxis.Rows[2][(i + 1).ToString(CultureInfo.InvariantCulture)] - (double)_dataAxis.Rows[1][(i + 1).ToString(CultureInfo.InvariantCulture)];
                    if (overweigth > 0 && _damage.Count > 0)
					{
                        if ((double)_dataAxis.Rows[1][(i + 1).ToString()] == 0)
                            continue;
                        overweigth = (overweigth / (double)_dataAxis.Rows[1][(i + 1).ToString()]) * 100;
						if ( _mainform._currentPVK.Spring_limit > 0)
                            formula = _damage.Where(t => t.OverMax > overweigth && t.OverMin <= overweigth && t.Type == "Н" && t.Factor == 3.ToString(CultureInfo.InvariantCulture)).Select(tt => tt.Formula).First();
						else if (_weighing.TrailerPodveska == 1 && i >= _weighing.ModelVehicle.CountAxis  && i <= _weighing.CountAxis)
                            formula = _damage.Where(t => t.OverMax > overweigth && t.OverMin <= overweigth && t.Type == "Н" && t.Factor == 2.ToString(CultureInfo.InvariantCulture)).Select(tt => tt.Formula).First();
						else  if (_weighing.VehiclePodveska == 1 && i < _weighing.ModelVehicle.CountAxis)
                            formula = _damage.Where(t => t.OverMax > overweigth && t.OverMin <= overweigth && t.Type == "Н" && t.Factor == 2.ToString(CultureInfo.InvariantCulture)).Select(tt => tt.Formula).First();
						else
                            formula = _damage.Where(t => t.OverMax > overweigth && t.OverMin <= overweigth && t.Type == "Н" && t.Factor == 1.ToString(CultureInfo.InvariantCulture)).Select(tt => tt.Formula).First();
						if (formula.Contains("Н"))
						{
                            formula = formula.Replace("#Н#**2", "#Н#*#Н#");
                            formula = formula.Replace("#Н#**3", "#Н#*#Н#*#Н#");
							formula = formula.Replace("#Н#",overweigth.ToString(CultureInfo.InvariantCulture));
                            damageAxis[i] = Convert.ToDouble(new Expression(formula).Evaluate());
						}
						else
						{
							damageAxis[i] = Convert.ToDouble(formula);
						}
						fullDamage += damageAxis[i] * _coefInfl; 
					}
				}
				return fullDamage ;
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				return 0;
			}

		}

        private void SaveImage()
        {
            try
            {
                if (true)
				{
                    if (File.Exists(@".\\snapshot.jpeg"))
                    {
                        File.Delete(@".\\snapshot.jpeg");
                    }
                    MemoryStream ms = new MemoryStream();
                    var cloneImage = pictureEdit2.Image.Clone();
                    pictureEdit1.Image = (Image)cloneImage;
                    pictureEdit1.Image.Save(ms,System.Drawing.Imaging.ImageFormat.Bmp);
                    _weighing.Photo = ms.ToArray();
                    pictureEdit1.Image = (Image)new ImageConverter().ConvertFrom(Convert.FromBase64String(Convert.ToBase64String(_weighing.Photo)));

	                ms.Close();
				}								  
            }
            catch (Exception ex)
            {
                _logger.TraceException("Ошибка", ex);
            }
        }

        private void SetNextWeightAxis(object sender, EventArgs e)
        {
            try
            {
                var weight = ((Calculator)sender).lastWeigth;
                if (gridView1.FocusedColumn.AbsoluteIndex == 0)
                {
                    gridView1.FocusedColumn = gridView1.Columns[gridView1.FocusedColumn.AbsoluteIndex + 1];
                    gridView1.FocusedRowHandle = 2;
                }
                if (gridView1.FocusedColumn.AbsoluteIndex == 1)
                {
                }
                if (gridView1.FocusedColumn.AbsoluteIndex >= _weighing.CountAxis + 1)
                    gridView1.FocusedColumn = gridView1.Columns[1];
                if ((gridView1.FocusedColumn.ToString() != "Тип данных" || gridView1.FocusedColumn.ToString() != "FullWeigth") && gridView1.FocusedColumn.AbsoluteIndex < _weighing.CountAxis + 1)
                {
                    gridView1.SetRowCellValue(2, gridView1.FocusedColumn, weight);
                    gridView1.FocusedColumn = gridView1.Columns[gridView1.FocusedColumn.AbsoluteIndex + 1];
                    gridView1.FocusedRowHandle = 2;
                }
                UpdateFullWeigth();
            }
            catch (Exception ex)
            {

                _logger.TraceException("", ex);
            }

        }

		private void button4_Click(object sender, EventArgs e)
		{
            try
            {
			    if (gridView1.FocusedColumn.AbsoluteIndex == 0)
			    {
				    gridView1.FocusedColumn = gridView1.Columns[gridView1.FocusedColumn.AbsoluteIndex + 1];
				    gridView1.FocusedRowHandle = 2;
			    }
			    if (gridView1.FocusedColumn.AbsoluteIndex == 1)
			    {
				    try
				    {
                        SaveImage();
				    }
				    catch (Exception ex)
				    {
					    _mediadGetter.CloseVideoStream();
					    _logger.TraceException("Ошибка",ex); 
				    }	  
				    if (_weighing.Photo != null && _weighing.Photo.Count() > 0)
					    pictureEdit1.Image = (Image) new ImageConverter().ConvertFrom(_weighing.Photo);
			    }
			    if	(gridView1.FocusedColumn.AbsoluteIndex >=  _weighing.CountAxis + 1)
				       gridView1.FocusedColumn = gridView1.Columns[1];
			    if ((gridView1.FocusedColumn.ToString() != "Тип данных" || gridView1.FocusedColumn.ToString() != "FullWeigth") && gridView1.FocusedColumn.AbsoluteIndex < _weighing.CountAxis + 1 )
			    {
                    gridView1.SetRowCellValue(2, gridView1.FocusedColumn, Convert.ToDouble(labelControl6.Text));
				    gridView1.FocusedColumn = gridView1.Columns[gridView1.FocusedColumn.AbsoluteIndex + 1];
				    gridView1.FocusedRowHandle = 2;
			    }
			    UpdateFullWeigth();
            }
            catch (Exception ex)
            {
                
                _logger.TraceException("", ex);
            }

		}

		private void gridLookUpEdit5_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
		    if (e.Button.Index != 1) return;
		    var model = new ModelVehicleForm(_model.Where(t =>t.Type == 1).ToList(),_mainform,_mainSession,1);
		    model.UpdateModel += UpdateModels;
		    Enabled = false;
		    model.Show();
		    model.Activate();
		}

		private void gridLookUpEdit5_EditValueChanged(object sender, EventArgs e)
		{
		    if (_dataAxis != null) _dataAxis.NewRow();
		    var row = (DataRowView)gridLookUpEdit5.GetSelectedDataRow();
		    if (gridLookUpEdit5.EditValue == null)
                _weighing.ModelVehicle = null;
		    _weighing.ModelVehicle =  _model.FirstOrDefault(t => t.Id == Convert.ToInt32(gridLookUpEdit5.EditValue));
            UpdateDistanceAxisVehicle();
		    UpdateDistanceAxisNew();
		}

		private void gridLookUpEdit6_EditValueChanged(object sender, EventArgs e)
		{
		    if (_dataAxis != null) _dataAxis.NewRow();
		    if (gridLookUpEdit7.EditValue == null)
                _weighing.ModelTrailer = null;
		    _weighing.ModelTrailer =  _model.FirstOrDefault(t => t.Id == Convert.ToInt32(gridLookUpEdit7.EditValue));
            UpdateDistanceAxisTrailer();
		    UpdateDistanceAxisNew();
		}

		private void gridLookUpEdit6_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			if(e.Button.Index == 1)
			{
				var model = new ModelVehicleForm(_model.Where(t =>t.Type == 2).ToList(),_mainform,_mainSession,2);
				model.UpdateModel += UpdateModels;
				Enabled = false;
				model.Show();
				model.Activate();				
			}
		}


        public void UpdateMediaFile(Image i)
        {
            try
            {
                Invoke(new SetNewPicture(SetNewImage), i);
            }
            catch (Exception)
            {
                _mediadGetter.CloseVideoStream();
            }
        }

		public void UpdateEmguImage(IImage i)
		{
			try
			{
				imageBox1.Image = i;
				//Invoke(new SetNewPicture(SetNewImage), i);
			}
			catch (Exception)
			{
				_mediadGetter.CloseVideoStream();
			}
		}	  

		delegate void SetNewPicture(Image i);

		delegate void DelegateSetNewImage(IImage i);

        void SetNewImageEmgu(IImage i)
        {
			 imageBox1.Image = (IImage)i;
        }

		void SetNewImage(Image i)
		{
			pictureEdit2.Image = i;
		}

        public event EventHandler<MediaFileCahngedEventArgs> MediaFileChanged;
        public event EventHandler<EventArgs> GetNextPhoto;
        public event EventHandler<EventArgs> GetPrevPhoto;
        public int SelectedPhotoIndex { get; set; }
        public short OffsetKm { get; set; }	
        public IDictionary<int, string> MediaFiles	  {	  get ;	set ;	}
		public Image MediaFile	  {	  get ;	set ;	}
		public bool PrevButtonEnabled   {	  get ;	set ;	}	   
        public bool NextButtonEnabled   {	  get ;	set ;	}

        //public MediaBase Media
        //{
        //    get { return _media; }
        //    set { _media = value; }
        //}

        private void textEdit10_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				textEdit11.Text = Math.Round((CalcDamageValue() * (Convert.ToDouble(textEdit10.Text == "" ? "100" : textEdit10.Text) / 100.0)),2).ToString();
			}
			catch (Exception ex)
			{
				_logger.ErrorException("Ошибка", ex);
			}
			  
		}

		private void pictureEdit1_DoubleClick(object sender, EventArgs e)
		{
			pictureEdit1.Show();
			var picBox =  new PhotoView {Photo = pictureEdit1.Image};
		    picBox.Show();
		}

		private void pictureEdit2_DoubleClick(object sender, EventArgs e)
		{
            //_photoOnFullScreen =  new PhotoView {Photo = pictureEdit2.Image};
            //_photoOnFullScreen.Show();
		}

		private void gridView1_CellValueChanged_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
		{
			UpdateFullWeigth();
			CalcPermissionLoad();
		}

		private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			try
			{
				var currLoadaxis = 0.0;
				var permissionLoadaxis = 0.0;
				var view = sender as GridView;
				if (e.Column.ToString() != "Тип данных" && e.RowHandle == 2)
				{
				    if (view != null)
				    {
				        currLoadaxis =  view.GetRowCellValue(2, e.Column) is DBNull ? 0 : Convert.ToDouble(view.GetRowCellValue(2, e.Column));
				        permissionLoadaxis = view.GetRowCellValue(1, e.Column) is DBNull ? 0 : Convert.ToDouble(view.GetRowCellValue(1, e.Column));
				    }
				    //if (e.Column != gridView1.FocusedColumn)

					if (currLoadaxis > permissionLoadaxis && (e.Column != gridView1.FocusedColumn || e.RowHandle != gridView1.FocusedRowHandle))
					{
						e.Appearance.BackColor = Color.Coral;
						e.Appearance.BackColor2 = Color.Coral;
					}
				}
			}
			catch (Exception ex)
			{
				_logger.ErrorException("Ошибка", ex);
			}
		}

		private void checkEdit1_CheckedChanged(object sender, EventArgs e)
		{
			_weighing.TrailerPodveska = checkEdit3.Checked ? 1 : 2;
            CalcPermissionLoad();
			textEdit11.Text = Math.Round((CalcDamageValue() * (Convert.ToDouble(textEdit10.Text == "" ? "100" : textEdit10.Text) / 100.0)), 2).ToString();
		}

		private void checkEdit4_CheckedChanged(object sender, EventArgs e)
		{
			_weighing.VehiclePodveska = checkEdit4.Checked ? 1 : 2;
            CalcPermissionLoad();
			textEdit11.Text = Math.Round((CalcDamageValue() * (Convert.ToDouble(textEdit10.Text == "" ? "100" : textEdit10.Text) / 100.0)), 2).ToString();
		}

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            //редактировать
            if (checkEdit2.Checked)
            {
                panelControl2.Enabled = true;
                panelControl3.Enabled = checkEdit5.Checked;
                gridControl1.Enabled = true;
                //button5.Enabled = true;
                button4.Enabled = true;
                button1.Enabled = true;
                checkEdit5.Enabled = true;
            }
            else
            {
                panelControl2.Enabled = false;
                panelControl3.Enabled = false;
                gridControl1.Enabled = false;
                //button5.Enabled = false;
                button4.Enabled = false;
                button1.Enabled = false;
                checkEdit5.Enabled = false;
            }
        }

        private void textEdit17_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (checkEdit5.Checked)
            {
                _weighing.ActNomer = _mainform._currentPVK.Nomer + GetNewNumberProtocol((DateTime)((DateEdit)sender).EditValue).ToString("0000"); 
                textEdit9.Text = _weighing.ActNomer;
            }
        }

        private int GetNewNumberProtocol(DateTime date)
        {
            try
            {
                string sqlLastId = string.Format("SELECT MAX(ACT_NOMER) FROM PVK_WEIGHING where ACT_PROTOCOL_DATE BETWEEN '{0}-01-01 00:00:00' and '{1}-01-01 00:00:00'", date.Year, date.Year + 1);
				object lastIdObject = _mainSession.CreateSQLQuery(sqlLastId).UniqueResult();
                if (lastIdObject == null)
                    return 1;
                int lastId = Convert.ToInt32(lastIdObject.ToString().Substring(lastIdObject.ToString().Length - 4,4));
                return lastId + 1;
            }
            catch (Exception ex)
            {
                _logger.TraceException("",ex);
                return 0;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        void DisplayFactAxisLoad()
        {
            try
            {
                int i  = _weighing.CountAxis + 1;
                for (; i < 25; i++)
                    gridView1.Columns[i.ToString()].Visible = false;
                i = 1;
                for (; i < 25; i++)
                    _dataAxis.Rows[2][i.ToString()] = 0;
                i = 1;
                foreach (List<WeightRecord> item in _newArrWeightTemp.Values)
                {
                    if (i >= 24)
                        break;
                    var weight = item.Max(t => t.Weight);
                    _dataAxis.Rows[2][i.ToString()] = weight;
                    gridView1.Columns[i.ToString()].VisibleIndex = i;
                    gridView1.Columns[i.ToString()].Visible = true;
                    i+=1;
                }
                gridView1.Columns["FullWeigth"].VisibleIndex = _weighing.CountAxis == 0 || _weighing.CountAxis < i ? i : _weighing.CountAxis + 2;
                gridView1.Columns["FullWeigth"].Visible = true;
                UpdateFullWeigth();
                AdjustGridControlSize();
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (bIsWeighting)
            {
                button6.Text = @"Начать взвешивание";
                bIsWeighting = false;
                _arrWeightTemp.Clear();
                lock (_weightArray)
                {
                    foreach (var st in _weightArray)
                        _arrWeightTemp.Add((WeightRecord)st.Clone());
                }
                _calc.UpdateWeightAxis -= SetNextWeightAxis;
                _calc.Calculate(_arrWeightTemp);
                _newArrWeightTemp = _calc.NewArrWeight;
                this.Invoke(new Action(() => DisplayFactAxisLoad()));
            }
            else 
            {

                button6.Text = @"Завершить взвешивание";
                lock (_weightArray)
                {
                    _weightArray.Clear();
                }
                bIsWeighting = true;
                SaveImage();
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            // Create a new Chart control.
            var chart = new ChartControl {Location = new Point(10, 10)};
            List<Series> listSeries = new List<Series>();
            // Set the chart's location.
            var series1 = new Series("Вес от времени", ViewType.Line);
            foreach (var st in _arrWeightTemp)
            {
                series1.Points.Add(new SeriesPoint(st.Dt.ToString("hh--mm--ss..ffff"), st.Weight));
            }
            //series1.Points.Add(new SeriesPoint("aa",_arrWeightTemp));
            // Perform any other initialization here.
            // ...
            // ...
             foreach (var st in _newArrWeightTemp.Values)
            {
                var series2 = new Series("Вес от времени новый", ViewType.Line);
                foreach (var weightRecord in st)
                {
                    series2.Points.Add(new SeriesPoint(weightRecord.Dt.ToString("hh--mm--ss..ffff"), weightRecord.Weight));   
                }
                listSeries.Add(series2);
            }

            // Add the chart to the Form.
            // Add the series to the chart.
            chart.Series.Add(series1);
            foreach (var series in listSeries)
            {
                chart.Series.Add(series);   
            }
            series1.ArgumentScaleType = ScaleType.Auto;
           // ((SplineSeriesView)series1.View).LineTensionPercent = 90;
            ((XYDiagram)chart.Diagram).EnableAxisXZooming = true;
            // Hide the legend (if necessary).
            chart.Legend.Visible = false;
            // Add a title to the chart (if necessary).
            var chartTitle1 = new ChartTitle {Text = @"Вес от времени"};
            chart.Titles.Add(chartTitle1);

            // Add the chart to the form.
            chart.Dock = DockStyle.Fill;
            var ff = new XtraForm();
            ff.Controls.Add(chart);
            ff.Show();
            //this.Controls.AddRange(new Control[] { chart });
        }


        private void vlcControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                XmlSerializer serializer;
                Directory.CreateDirectory(_folderNameWeigingSave);
                _logger.Trace("start save ");
                PVK_WEIGHING __weighing = GetPVK_WEIGHING();
                _logger.Trace("test");
                FileStream fileStream = File.Open(_folderNameWeigingSave + DateTime.Now.ToString("yyyyMMdd_hhmmss_") +  ".xml", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                _logger.Trace("__weighing " );
                WeightFile wf = new WeightFile() { Weighing = __weighing, Records = new List<WeightRecord>(_arrWeightTemp) };
                _logger.Trace("__weighing 2" );
                using (StreamWriter fw = new StreamWriter(fileStream))
                {
                    _logger.Trace("__weighing 3" );
                    serializer = new XmlSerializer(typeof(WeightFile));
                    serializer.Serialize(fw, wf);
                    //foreach (WeightRecord weightRecord in _arrWeightTemp)
                    //{
                    //    fw.WriteLine(weightRecord.Dt.ToString("dd.MM.yyyy hh.mm.ss.ffff") + ": " + weightRecord.Weight.ToString(CultureInfo.InvariantCulture));
                    //}
                }  
                fileStream.Close();
            }
            catch (Exception ex)
            {
                _logger.TraceException("",ex);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.InitialDirectory = _folderNameWeigingSave;
                DialogResult res = openFileDialog1.ShowDialog();
                DateTime? dtlast = null;
                DateTime? dt = null;
                Double? weight = null;
                XmlSerializer serializer;
                WeightFile file;
                if (res == DialogResult.OK)
                {
                    FileStream fileStream = File.Open(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                    lock (_weightArray)
                    {
                        _weightArray.Clear();
                    }
                    using (StreamReader fw = new StreamReader(fileStream))
                    {

                        serializer = new XmlSerializer(typeof(WeightFile));
                        file =  (WeightFile)serializer.Deserialize(fw);
                    }
                    fileStream.Close();
                    _arrWeightTemp.Clear();
                    WeighingLoad(file.Weighing);
                    foreach (var st in file.Records)
                        _arrWeightTemp.Add((WeightRecord)st.Clone());
                    WeighingLoad(file.Weighing);
                    _calc.Calculate(_arrWeightTemp);
                    _newArrWeightTemp = _calc.NewArrWeight;
                }
            }
            catch (Exception ex)
            {
                _logger.TraceException("", ex);
            }
        }



        PictureBox IMediaDataViewer.VideoOutputPicture
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void UpdateMediaFileOpenCv(Emgu.CV.Image<Bgr, byte> mediaFile)
        {
            try
            {
                //imageBox1.Image = mediaFile;
                //imageBox1.Update();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void pictureEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureEdit2_DoubleClick_1(object sender, EventArgs e)
        {
            if (pictureEdit2.Dock == DockStyle.Fill)
                pictureEdit2.Dock = DockStyle.None;
            else
                pictureEdit2.Dock = DockStyle.Fill;
        }


        public void UpdateImage(object sender, MediaDataGetter.UpdateImageArgs args)
        {
            try
            {
                if (this.WindowState != FormWindowState.Minimized)
                {
                    //pictureEdit2.Image = args.mediaFile;
                    //pictureEdit2.Update();
                    //pictureEdit2.Refresh();
                    //SetNewImage(args.mediaFile);


					//Invoke(new SetNewPicture(SetNewImage), args.mediaFile);
                }

            }
            catch (Exception)
            {
                _mediadGetter.CloseVideoStream();
            }
        }

		public void UpdateEmguImage(object sender, MediaDataGetter.UpdateImageArgs args)
		{
			try
			{
				if (this.WindowState != FormWindowState.Minimized)
				{
					//pictureEdit2.Image = args.mediaFile;
					//pictureEdit2.Update();
					//pictureEdit2.Refresh();
					//SetNewImage(args.mediaFile);
					imageBox1.Image = args.imageOpenCV;

					//Invoke(new SetNewPicture(SetNewImage), args.mediaFile);
				}

			}
			catch (Exception)
			{
				_mediadGetter.CloseVideoStream();
			}
		}

        public void PlayVideo(IMediaDataViewer weighingForm)
        {
            throw new NotImplementedException();
        }

        private void repositoryItemSpinEdit1_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {

        }

        private void repositoryItemSpinEdit1_Enter(object sender, EventArgs e)
        {
			if (((SpinEdit)sender).Text.Contains("_,__"))
				((SpinEdit)sender).SelectedText = "";
            //((RepositoryItemSpinEdit)sender).Value = null;
        }

		private void repositoryItemSpinEdit1_Click(object sender, EventArgs e)
		{

		}
    }

    public class WeightRecord : ICloneable
    {
        private DateTime _dt;

        public DateTime Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }

        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }



        private double _weight;

        public object Clone()
        {
            var newThis = new WeightRecord
            {
                _dt = _dt,
                _weight = _weight
            };
            //newThis.FactorForPerformance = Me.FactorForPerformance
            return newThis;
        }

    }
    [Serializable()]
    public class WeightFile
    {
        private PVK_WEIGHING _weighing;
        private List<WeightRecord> _records;

        public List<WeightRecord> Records
        {
            get { return _records; }
            set { _records = value; }
        }

        public PVK_WEIGHING Weighing
        {
            get { return _weighing; }
            set { _weighing = value; }
        }
    }
}