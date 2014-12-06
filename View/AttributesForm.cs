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
	public partial class AttributesForm : DevExpress.XtraEditors.XtraForm
	{
		private readonly ISession _mainSession; 
		private readonly MainForm _mainForm;
		public readonly List<PVK_ATTR_VALUES> _attrValues; 
		public readonly List<PVK_ATTRIBUTES> _attr; 
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public AttributesForm()
		{
			InitializeComponent();
		}

		public AttributesForm(List<PVK_ATTR_VALUES> attrValues,List<PVK_ATTRIBUTES> attr,MainForm mainF, ISession mainSession) : this()
		{
			_mainSession =  mainSession;
			_mainForm = mainF;
			_attrValues = attrValues;
			_attr = attr;
			pVKATTRVALUESBindingSource.DataSource   = mainF.UpdateAttrValues(_attrValues);
			pVKATTRIBUTESBindingSource.DataSource   = mainF.UpdateAttributes(_attr);
		}

		private void gridControl1_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
		{
			if (e.Button.ButtonType == NavigatorButtonType.Append)
			{
                _logger.Info("Добавление или удаление строк в этом справочнике недоступно.");
                //var row = gridView1.GetFocusedDataRow();
                //var dsRow = gridView1.GetFocusedRow();
			}
			else if (e.Button.ButtonType == NavigatorButtonType.Remove)
			{
                _logger.Info("Добавление или удаление строк в этом справочнике недоступно.");
			}
			else if (e.Button.ButtonType == NavigatorButtonType.EndEdit)
			{
					var row = gridView1.GetFocusedDataRow();
					row[gridView1.FocusedColumn.FieldName] = gridView1.EditingValue;
					if (!(row["ID"] is System.DBNull) && Convert.ToInt32(row["ID"]) != 0)
					{
						var tt = _mainSession.Get<PVK_ATTR_VALUES>(Convert.ToInt32(row["ID"]));
						UpdateAttributesValues(tt, row);
						_mainSession.Save(tt);
						_mainSession.Flush();
					}
					else
					{
						var tt = GetAttributesValues(row);
						_mainSession.SaveOrUpdate(tt);
						_mainSession.Flush();
					}
					_mainForm.UpdateAttrValues();
			}
		}
		private PVK_ATTR_VALUES GetAttributesValues(DataRow row)
		{
				var tt = new PVK_ATTR_VALUES();
				UpdateAttributesValues(tt, row);
				return tt;
		}

		private void UpdateAttributesValues(PVK_ATTR_VALUES tt,DataRow row)
		{
			try
			{
                tt.Name = row["NAME"] is System.DBNull ? "" : (string)row["NAME"];
				tt.Value = row["VALUE"] is System.DBNull ? "" : (string)row["VALUE"];
				tt.Description =  row["DESCRIPTION"] is System.DBNull ? "" : (string)row["DESCRIPTION"];
                //tt.Attribute = _attr.Where(t => t.Abbr == )
                //tt.Abbr =_attrValues.First().Abbr;
                //tt.AttrId = _attrValues.First().AttrId;
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);		
			}
		}

        private void AttributesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_attr.Count(t => t.Abbr == "SETTINGS_EUIP") > 0 && _attrValues.Where(t => t.Abbr == "COM1").Count() > 0)
                {var com1 = _attrValues.Where(t => t.Abbr == "COM1").FirstOrDefault().Value;
                    var com2 = _attrValues.Where(t => t.Abbr == "COM2").FirstOrDefault().Value;
                    var protocol = _attrValues.Where(t => t.Abbr == "Protocol").FirstOrDefault().Value;
                    if (protocol == "1")
                    {
                        if (_mainForm._currentPVK.Scale == null)
                            _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2], com1.Split(',')[3], com1.Split(',')[4]);
                        else
                        {
                            if (_mainForm._scale != null)
                                _mainForm._scale.FreeComPort();
                            _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2], com1.Split(',')[3], com1.Split(',')[4]);

                        }
                    }
                    else if (protocol == "2")
                    {
                        if (_mainForm._currentPVK.Scale == null)
                            _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2], com1.Split(',')[3], com1.Split(',')[4],
                                                        com2.Split(',')[0], com2.Split(',')[1], com2.Split(',')[2], com2.Split(',')[3], com2.Split(',')[4]);else
                        {
                            if (_mainForm._scale != null)
                                _mainForm._scale.FreeComPort();
                            _mainForm._scale = new Scale(com1.Split(',')[0], com1.Split(',')[1], com1.Split(',')[2], com1.Split(',')[3], com1.Split(',')[4],
                                                        com2.Split(',')[0], com2.Split(',')[1], com2.Split(',')[2], com2.Split(',')[3], com2.Split(',')[4]);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                _logger.TraceException("",ex);
            }


        }
	}
}