using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace PVK.Control.View
{
    static class ViewsHelper
    {
        public static void ShowError(string message, UserLookAndFeel userLookAndFeel)
        {
            XtraMessageBox.Show(userLookAndFeel, message, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ShowWarn(string message, UserLookAndFeel userLookAndFeel)
        {
            return XtraMessageBox.Show(userLookAndFeel, message, @"Предупреждение", MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Warning);
        }

        public static void ShowMessage(string message, UserLookAndFeel userLookAndFeel)
        {
            XtraMessageBox.Show(userLookAndFeel, message, @"Сообщение");
        }

        public delegate void LayoutSaver(MemoryStream memoryStream);

        public static string LayoutToXmlString(LayoutSaver layoutSaver)
        {
            var stream = new MemoryStream();
            layoutSaver(stream);
            stream.Position = 0;
            return new StreamReader(stream).ReadToEnd();
        }

        public static void SetToolTipForUpdateButton(SimpleButton simpleButton)
        {
            simpleButton.ToolTip = @"Время последнего обновления: " + DateTime.Now.ToString("T");
        }

        public static void GridViewCustomDrawGroupPanel(object sender, CustomDrawEventArgs e)
        {
            var message = String.Format("Выбрано: {0} из {1}", ((GridView)sender).DataController.VisibleCount, ((GridView)sender).DataController.ListSourceRowCount);
            var messageSize = e.Graphics.MeasureString(message, e.Appearance.Font);
            e.Graphics.DrawString(message, e.Appearance.Font, Brushes.DarkGray,
                                  e.Bounds.X + e.Bounds.Width - messageSize.Width,
                                  (int)((e.Bounds.Y + e.Bounds.Height) / 2f - messageSize.Height / 2));
            e.Handled = true;
        }

        public static void ColButtonUpDownClick(object sender, ButtonPressedEventArgs e)
        {
            const int relationIndex = 0;
            var gridView = (GridView)((GridControl)((LookUpEdit)sender).Parent).MainView;
            if (gridView.GetMasterRowExpandedEx(gridView.FocusedRowHandle, relationIndex))
            {
                var detailView = (ColumnView)gridView.GetDetailView(gridView.FocusedRowHandle, relationIndex);
                switch (e.Button.Kind)
                {
                    case ButtonPredefines.Up:
                        if (detailView.FocusedRowHandle > 0)
                        {
                            var curRow = detailView.GetFocusedDataRow();
                            detailView.MovePrev();
                            var prevRow = detailView.GetFocusedDataRow();
                            var curOrder = (int)curRow["OrderId"];
                            var prevOrder = (int)prevRow["OrderId"];
                            curRow["OrderId"] = prevOrder;
                            prevRow["OrderId"] = curOrder;
                            detailView.MovePrev();
                        }
                        break;
                    case ButtonPredefines.Down:
                        if (detailView.FocusedRowHandle < detailView.RowCount - 1)
                        {
                            var curRow = detailView.GetFocusedDataRow();
                            detailView.MoveNext();
                            var nextRow = detailView.GetFocusedDataRow();
                            var curOrder = (int)curRow["OrderId"];
                            var nextOrder = (int)nextRow["OrderId"];
                            curRow["OrderId"] = nextOrder;
                            nextRow["OrderId"] = curOrder;
                            detailView.MoveNext();
                        }
                        break;
                }
            }
        }

        public static bool ValidateValue(GridColumn column, ColumnView view, int rowHandle, out string errorText)
        {
            if (view.GetRowCellValue(rowHandle, column) is DBNull)
            {
                errorText = String.Format("Значение поля \"{0}\" должно быть заполнено.", column.Caption);
                view.SetColumnError(column, errorText);
                return false;
            }
            errorText = string.Empty;
            return true;
        }

        public static void GridViewKeyPress(object sender, KeyEventArgs e)
        {
            var gc = (GridControl)sender;
            if (gc.FocusedView.GetType().ToString().Contains("Grid.GridView"))
            {
                var gw = (GridView)((GridControl)sender).FocusedView;
                if (gw.Editable)
                {
                    if (e.KeyData == Keys.F3)
                        GridViewPressF3(sender, e);
                    else if (e.KeyData == Keys.F4)
                        GridViewPressF4(sender, e);
                }
            }

        }

        private static void GridViewPressF3(object sender, KeyEventArgs e)
        {
            var gc = (GridControl)sender;
            var gw = (GridView)(gc).FocusedView;
            if (gw.RowCount > 1)
            {
                gw.SetFocusedRowCellValue(gw.FocusedColumn, gw.GetRowCellValue(gw.RowCount - 2, gw.FocusedColumn)); 
            }

        }

        private static void GridViewPressF4(object sender, KeyEventArgs e)
        {
            var gc = (GridControl)sender;
            var gw = (GridView)(gc).FocusedView;
            if (gw.RowCount > 1)
            {
                //object[] itemArr = new object[] { gw.GetDataRow(gw.RowCount - 2).ItemArray };
                //((object[])itemArr[0])[0] = 0;
                //gw.GetFocusedDataRow().ItemArray = (object[])itemArr[0];
                foreach (GridColumn clm in gw.Columns)
                {
                    gw.SetFocusedRowCellValue(clm, gw.GetRowCellValue(gw.RowCount - 2, clm));
                }
            }
        }
    }
}
