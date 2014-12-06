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
	public partial class ScalesSettingsForm : DevExpress.XtraEditors.XtraForm
	{
		private readonly ISession _mainSession; 
		private readonly MainForm _mainForm;
		private readonly List<PVK_SCALES> _scales; 
		private readonly PVK_SCALES _scale; 
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public ScalesSettingsForm()
		{
			InitializeComponent();
		}

		public ScalesSettingsForm(List<PVK_SCALES> scale,MainForm mainF, ISession mainSession)
		{
			InitializeComponent();
			_mainSession =  mainSession;
			_mainForm = mainF;
			_scales = scale;
			pVKSCALESBindingSource.DataSource   = mainF.UpdateScales(_scales);
		}
		
		private PVK_SCALES GetScale(DataRow row)
		{
				var tt = new PVK_SCALES();
				UpdateScales(tt,row);
				return tt;
		}

		
		private void UpdateScales(PVK_SCALES tt,DataRow row)
		{
			try
			{
				tt.Name = row["NAME"] is DBNull ? "" :(string)row["NAME"];  
				tt.Nomer = row["NOMER"] is DBNull ? "" : (string)row["NOMER"];
				tt.DateCkeking = row["DateChecking"] is DBNull ? DateTime.MinValue : (DateTime)row["DateChecking"];
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);		
			}

		}

		private void gridControl1_EmbeddedNavigator_ButtonClick_1(object sender, NavigatorButtonClickEventArgs e)
		{
			try
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
                        //var row = gridView1.GetFocusedDataRow();
                        //var tt = _mainSession.Get<PVK.Data.PVK>(Convert.ToInt32(row["ID"]));
                        //_mainSession.Delete(tt);
                        //_mainSession.Flush();
					}
					else if (e.Button.ButtonType == NavigatorButtonType.EndEdit)
					{
							var row = gridView1.GetFocusedDataRow();
							row[gridView1.FocusedColumn.FieldName] = gridView1.EditingValue;
							if (!(row["ID"] is System.DBNull) && Convert.ToInt32(row["ID"]) != 0)
							{
								var tt = _mainSession.Get<PVK_SCALES>(Convert.ToInt32(row["ID"]));;
								UpdateScales(tt, row);
									var k = _scales.Where(t => t.Id == Convert.ToInt32(row["ID"])).First();
								 _scales.Remove(k);
								_scales.Add(tt);
								_mainSession.Save(tt);
								_mainSession.Flush();
							}
							else
							{
								var tt = GetScale(row);
								_mainSession.SaveOrUpdate(tt);
								_scales.Add(tt);
								_mainSession.Flush();
							}
							pVKSCALESBindingSource.DataSource   = _mainForm.UpdateScales(_scales);
					}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);		
			}
		}
	}
}