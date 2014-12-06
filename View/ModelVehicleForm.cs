using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NHibernate;
using System.Linq;
using PVK.Data;
using NLog;

namespace PVK.Control.View
{
	public partial class ModelVehicleForm : DevExpress.XtraEditors.XtraForm
	{
		private readonly ISession _mainSession; 
		private readonly MainForm _mainForm;
		private int _typeModel;
		private readonly List<PVK_MODEL_VEHICLE> _model; 
		public event EventHandler<EventArgs> UpdateModel;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        DataTable tempDataTable ;

		public ModelVehicleForm()
		{
			InitializeComponent();
			_mainSession =  SessionHelper.OpenSession();
		}
		public ModelVehicleForm(List<PVK_MODEL_VEHICLE> model,MainForm mainF, ISession mainSession,int typeModel) : this ()
		{
			_mainForm = mainF;
			_mainSession = mainSession;
			_model = model;
            //pVKMODELVEHICLEBindingSource.DataSource   = mainF.UpdateModeVehicle(_model);
            tempDataTable = mainF.UpdateModeVehicle(_model);
		    pVKMODELVEHICLEBindingSource.DataSource = tempDataTable;
			_typeModel = typeModel;
			if (_typeModel == 1)
			{
				this.colLINK_LENGTH.Visible = false;
				this.Text = "Марки автомобилей";
			}
			else
				this.Text = "Марки прицепов";
		}

		private void gridControl1_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
		{
			try
			{
				var row = gridView1.GetFocusedDataRow();
				PVK_MODEL_VEHICLE tt = null;
				if (row != null && !(row["ID"] is System.DBNull) && Convert.ToInt32(row["ID"]) != 0)
					tt = _mainSession.Get<PVK_MODEL_VEHICLE>(Convert.ToInt32(row["ID"]));
				if (e.Button.ButtonType == NavigatorButtonType.Append)
				{
					var dsRow = gridView1.GetFocusedRow();
				}
				else if (e.Button.ButtonType == NavigatorButtonType.Remove)
				{
                    if (ViewsHelper.ShowWarn("Вы действительно хотите удалить запись?", LookAndFeel) == DialogResult.No)
                    {
                        e.Handled = true;
                        return;
                    }
					_mainSession.Delete(tt);
					_mainSession.Flush();
				}
				else if (e.Button.ButtonType == NavigatorButtonType.EndEdit)
				{
					row[gridView1.FocusedColumn.FieldName] = gridView1.EditingValue;
					if (!(row["ID"] is System.DBNull) && Convert.ToInt32(row["ID"]) != 0)
					{
						GetModelVehicle(tt,row);
						var k = _model.Where(t => t.Id == Convert.ToInt32(row["ID"])).First();
							_model.Remove(k);
						_model.Add(tt);
						_mainSession.Save(tt);
						_mainSession.Flush();
					}
					else
					{
						tt = new PVK_MODEL_VEHICLE();
						GetModelVehicle(tt,row);
						_mainSession.SaveOrUpdate(tt);
						_model.Add(tt);
						_mainSession.Flush();
					}
                    tempDataTable.BeginLoadData();
                    tempDataTable.Clear();
                    foreach (var _model1 in _model)
                    {
                        tempDataTable.Rows.Add(_model1.Id, _model1.Mark, _model1.Model,
                            _model1.Type, _model1.CountAxis, _model1.AxisLength1, _model1.AxisLength2, _model1.AxisLength3,
                                                  _model1.AxisLength4, _model1.AxisLength5,
                                                  _model1.AxisLength6, _model1.AxisLength7,
                                                  _model1.AxisLength8, _model1.AxisLength9, _model1.AxisLength10, _model1.AxisLength11, _model1.AxisLength12, _model1.LinkLength);
                    }
                    tempDataTable.AcceptChanges();
                    tempDataTable.EndLoadData();
				}
			}
			catch (Exception ex)
			{
				
				_logger.ErrorException("",ex.InnerException);
			}

		}

		private void GetModelVehicle(PVK_MODEL_VEHICLE tt ,DataRow row)
		{
			try
			{
					tt.Model = (string)row["MODEL"];
					tt.AxisLength1 = row["AXIS_LENGTH_1"] is System.DBNull ? 0 :Convert.ToDouble(row["AXIS_LENGTH_1"].ToString().Replace(".",","));
					tt.AxisLength2 = row["AXIS_LENGTH_2"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_2"].ToString().Replace(".",","));
					tt.AxisLength3 = row["AXIS_LENGTH_3"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_3"].ToString().Replace(".",","));
					tt.AxisLength4 = row["AXIS_LENGTH_4"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_4"].ToString().Replace(".",","));
					tt.AxisLength5 = row["AXIS_LENGTH_5"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_5"].ToString().Replace(".",","));
					tt.AxisLength6 = row["AXIS_LENGTH_6"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_6"].ToString().Replace(".",","));
					tt.AxisLength7 = row["AXIS_LENGTH_7"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_7"].ToString().Replace(".",","));
					tt.AxisLength8 = row["AXIS_LENGTH_8"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_8"].ToString().Replace(".",","));
					tt.AxisLength9 = row["AXIS_LENGTH_9"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_9"].ToString().Replace(".",","));
					tt.AxisLength10 = row["AXIS_LENGTH_10"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_10"].ToString().Replace(".",","));
					tt.AxisLength11 = row["AXIS_LENGTH_11"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_11"].ToString().Replace(".",","));
					tt.AxisLength12 = row["AXIS_LENGTH_12"] is System.DBNull ? 0 : Convert.ToDouble(row["AXIS_LENGTH_12"].ToString().Replace(".",","));
					tt.LinkLength = row["LINK_LENGTH"] is System.DBNull ? 0 : Convert.ToDouble(row["LINK_LENGTH"].ToString().Replace(".",","));
					tt.CountAxis = row["COUNT_AXIS"] is System.DBNull ? 0 : Convert.ToInt32(row["COUNT_AXIS"]);	   
					tt.Type = _typeModel;
			}
			catch (Exception ex)
			{
				_logger.TraceException("Ошибка",ex);
			}

		}

		private void ModelVehicleForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (UpdateModel!=null)
				UpdateModel(this, EventArgs.Empty);
		}
	}
}