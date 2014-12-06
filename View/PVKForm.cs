using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using PVK.Data;
using TRLibrary.Commons;
using NHibernate;
using System.Linq;
using NLog;

namespace PVK.Control.View
{
	public partial class PVKForm : DevExpress.XtraEditors.XtraForm
	{
		private readonly ISession _mainSession; 
		private readonly MainForm _mainForm;
		private readonly List<PVK.Data.PVK> _pvk;
        public readonly List<PVK_ATTR_VALUES> _attrValues;
        public readonly List<PVK_ATTRIBUTES> _attr; 
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public PVKForm()
		{
			InitializeComponent();
			_mainSession =  SessionHelper.OpenSession();
            var version = CentralModule.ProductVersionGet();
            this.Text += " " + version.Version + "."  + version.Bild + " "+ version.Date;
		}

		public PVKForm(List<PVK.Data.PVK> pvk,MainForm mainF) : this()
		{
			_mainForm = mainF;
			_pvk = pvk;
			bindingSource1.DataSource   = mainF.UpdatePvk(_pvk);
            _attrValues = mainF.GetAttrValues();
            _attr = mainF.GetAttributes();
		}

		public PVKForm(List<PVK.Data.PVK> pvk,MainForm mainF, ISession mainSession) : this(pvk,mainF)
		{
			mainF.Enabled = false;
			_mainSession =  mainSession;   
		}
		private void button1_Click(object sender, EventArgs e)
		{
			//bindingSource1.DataSource   = _mainForm.UpdatePvk(_pvk);
		    
			_mainForm._currentPVK =_pvk.FirstOrDefault(t => t.Id == Convert.ToInt32(((DataRowView)gridView1.GetFocusedRow()).Row["Id"])) ;
            var _scale = _mainForm._currentPVK.Scale;
		    _mainForm._currentPVK.Scale = _scale;
		    SetPVKComPort();
            this.Close();
		}
        
		private void gridControl1_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
		{
			try
			{
		  			if (e.Button.ButtonType == NavigatorButtonType.Append)
					{
						var row = gridView1.GetFocusedDataRow();
						var dsRow = gridView1.GetFocusedRow();
					}
					else if (e.Button.ButtonType == NavigatorButtonType.Remove)
					{
                        if (ViewsHelper.ShowWarn("Вы действительно хотите удалить запись?", LookAndFeel) == DialogResult.No)
                        {
                            e.Handled = true;
                            return;
                        }
						var row = gridView1.GetFocusedDataRow();
						var tt = _mainSession.Get<PVK.Data.PVK>(Convert.ToInt32(row["ID"]));
						_mainSession.Delete(tt);
						_mainSession.Flush();
					}
					else if (e.Button.ButtonType == NavigatorButtonType.EndEdit)
					{
							var row = gridView1.GetFocusedDataRow();
							row[gridView1.FocusedColumn.FieldName] = gridView1.EditingValue;
							if (!(row["ID"] is System.DBNull) && Convert.ToInt32(row["ID"]) != 0)
							{
								var tt = _mainSession.Get<PVK.Data.PVK>(Convert.ToInt32(row["ID"]));;
								UpdatePvk(tt, row);
									var k = _pvk.Where(t => t.Id == Convert.ToInt32(row["ID"])).First();
								 _pvk.Remove(k);
								_pvk.Add(tt);
								_mainSession.Save(tt);
								_mainSession.Flush();
							}
							else
							{
								var tt = GetPvk(row);
								_mainSession.SaveOrUpdate(tt);
								_pvk.Add(tt);
								_mainSession.Flush();
							}
							bindingSource1.DataSource   = _mainForm.UpdatePvk(_pvk);
					}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);		
			}

		}
		private PVK.Data.PVK GetPvk(DataRow row)
		{
				var tt = new PVK.Data.PVK();
				UpdatePvk(tt,row);
				return tt;
		}

		
		private void UpdatePvk(PVK.Data.PVK tt,DataRow row)
		{
			try
			{
				tt.Adress = row["ADRESS"] is DBNull ? "" :(string)row["ADRESS"];  
				tt.Inspector_pvk = row["INSPECTOR_PVK"] is DBNull ? "" : (string)row["INSPECTOR_PVK"];
				tt.Nomer = row["NOMER"] is DBNull ? "" : (string)row["NOMER"];
				tt.Organizations = row["ORGANIZATIONS"] is DBNull ? "" : (string)row["ORGANIZATIONS"];
				tt.Specialist_pvk = row["SPECIALIST_PVK"] is DBNull ? "" : (string)row["SPECIALIST_PVK"];
				tt.Inspector_gbdd = row["INSPECTOR_GIBDD"] is DBNull ? "" : (string)row["INSPECTOR_GIBDD"];
				tt.Spring_limit = row["SPRING_LIMIT"] is DBNull ? 0 : Convert.ToInt32(row["SPRING_LIMIT"]);
				tt.NumberComPort = row["NUMBER_COM_PORT"] is DBNull ? "" : (string)row["NUMBER_COM_PORT"];
				tt.CameraUrl = row["CAMERA_URL"] is DBNull ? "" : (string)row["CAMERA_URL"];
				tt.CameraPass = row["CAMERA_PASS"] is DBNull ? "" : (string)row["CAMERA_PASS"];
				tt.CameraLogin = row["CAMERA_LOGIN"] is DBNull ? "" :  (string)(row["CAMERA_LOGIN"]);
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);		
			}

		}

	    public void  SetPVKComPort()
	    {
	        try
	        {
	            var com1 = _attrValues.Where(t => t.Abbr == "COM1").FirstOrDefault().Value;
	            var com2 = _attrValues.Where(t => t.Abbr == "COM2").FirstOrDefault().Value;
	            var protocol = _attrValues.Where(t => t.Abbr == "Protocol").FirstOrDefault().Value;
	            if (protocol == "1")
	            {
	                if (_mainForm._currentPVK.Scale == null)
	                    _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2],
	                        com1.Split(',')[3], com1.Split(',')[4]);
	                else
	                {
	                    if (_mainForm._scale != null)
	                        _mainForm._scale.FreeComPort();
	                    _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2],
	                        com1.Split(',')[3], com1.Split(',')[4]);
	                }
	            }
	            else if (protocol == "2")
	            {
	                if (_mainForm._currentPVK.Scale == null)
	                    _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2],
	                        com1.Split(',')[3], com1.Split(',')[4],
	                        com2.Split(',')[0], com2.Split(',')[1], com2.Split(',')[2], com2.Split(',')[3],
	                        com2.Split(',')[4]);
	                else
	                {
	                    if (_mainForm._scale != null)
	                        _mainForm._scale.FreeComPort();
	                    _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2],
	                        com1.Split(',')[3], com1.Split(',')[4],
	                        com2.Split(',')[0], com2.Split(',')[1], com2.Split(',')[2], com2.Split(',')[3],
	                        com2.Split(',')[4]);

	                }
	            }
	        }
	        catch (Exception ex)
	        {

	            _logger.Trace(ex.Message);
	        }
	    }

		private void PVKForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				_mainForm.Enabled = true;
			   /* var com1 = _attrValues.Where(t => t.Abbr == "COM1").FirstOrDefault().Value;
                var com2 = _attrValues.Where(t => t.Abbr == "COM2").FirstOrDefault().Value;
                var protocol = _attrValues.Where(t => t.Abbr == "Protocol").FirstOrDefault().Value;
			    if (protocol == "1")
			    {
			        if (_mainForm._currentPVK.Scale == null)
			            _mainForm._scale = new Scale(com1.Split(',')[0],com1.Split(',')[1],com1.Split(',')[2],com1.Split(',')[3],com1.Split(',')[4]);
			        else{
			            if (_mainForm._scale != null)
			                _mainForm._scale.FreeComPort();
			            _mainForm._scale = new Scale(com1.Split(',')[0],com1.Split(',')[1],com1.Split(',')[2],com1.Split(',')[3],com1.Split(',')[4]);
                        }
			    }
			    else if (protocol == "2")
			    {
			        if (_mainForm._currentPVK.Scale == null)
                        _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2], com1.Split(',')[3], com1.Split(',')[4],
                                                    com2.Split(',')[0], com2.Split(',')[1], com2.Split(',')[2], com2.Split(',')[3], com2.Split(',')[4]);
			        else
			        {
			            if (_mainForm._scale != null)
			                _mainForm._scale.FreeComPort();
                        _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2], com1.Split(',')[3], com1.Split(',')[4],
                                                    com2.Split(',')[0], com2.Split(',')[1], com2.Split(',')[2], com2.Split(',')[3], com2.Split(',')[4]);

			        }
			    }*/            
			}
			catch (Exception ex)
			{
				_logger.TraceException("",ex);
            }
        }
	}
}