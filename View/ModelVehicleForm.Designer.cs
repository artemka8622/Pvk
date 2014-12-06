namespace PVK.Control.View
{
	partial class ModelVehicleForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.pVKMODELVEHICLEBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pVKds = new PVK.Control.PVKds();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colMODEL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCOUNT_AXIS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLINK_LENGTH = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAXIS_LENGTH_12 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pVKMODELVEHICLEBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pVKds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.pVKMODELVEHICLEBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gridControl1_EmbeddedNavigator_ButtonClick);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(856, 323);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.UseEmbeddedNavigator = true;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // pVKMODELVEHICLEBindingSource
            // 
            this.pVKMODELVEHICLEBindingSource.DataMember = "PVK_MODEL_VEHICLE";
            this.pVKMODELVEHICLEBindingSource.DataSource = this.pVKds;
            // 
            // pVKds
            // 
            this.pVKds.DataSetName = "PVKds";
            this.pVKds.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMODEL,
            this.colCOUNT_AXIS,
            this.colLINK_LENGTH,
            this.colAXIS_LENGTH_1,
            this.colAXIS_LENGTH_2,
            this.colAXIS_LENGTH_3,
            this.colAXIS_LENGTH_4,
            this.colAXIS_LENGTH_5,
            this.colAXIS_LENGTH_6,
            this.colAXIS_LENGTH_7,
            this.colAXIS_LENGTH_8,
            this.colAXIS_LENGTH_9,
            this.colAXIS_LENGTH_10,
            this.colAXIS_LENGTH_11,
            this.colAXIS_LENGTH_12});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindDelay = 100;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colMODEL
            // 
            this.colMODEL.Caption = "Марка, модель";
            this.colMODEL.FieldName = "MODEL";
            this.colMODEL.Name = "colMODEL";
            this.colMODEL.Visible = true;
            this.colMODEL.VisibleIndex = 0;
            this.colMODEL.Width = 140;
            // 
            // colCOUNT_AXIS
            // 
            this.colCOUNT_AXIS.Caption = "Количество осей";
            this.colCOUNT_AXIS.FieldName = "COUNT_AXIS";
            this.colCOUNT_AXIS.Name = "colCOUNT_AXIS";
            this.colCOUNT_AXIS.Visible = true;
            this.colCOUNT_AXIS.VisibleIndex = 1;
            this.colCOUNT_AXIS.Width = 48;
            // 
            // colLINK_LENGTH
            // 
            this.colLINK_LENGTH.Caption = "Длина связи";
            this.colLINK_LENGTH.FieldName = "LINK_LENGTH";
            this.colLINK_LENGTH.Name = "colLINK_LENGTH";
            this.colLINK_LENGTH.Visible = true;
            this.colLINK_LENGTH.VisibleIndex = 2;
            this.colLINK_LENGTH.Width = 30;
            // 
            // colAXIS_LENGTH_1
            // 
            this.colAXIS_LENGTH_1.Caption = "1";
            this.colAXIS_LENGTH_1.FieldName = "AXIS_LENGTH_1";
            this.colAXIS_LENGTH_1.Name = "colAXIS_LENGTH_1";
            this.colAXIS_LENGTH_1.Visible = true;
            this.colAXIS_LENGTH_1.VisibleIndex = 3;
            this.colAXIS_LENGTH_1.Width = 30;
            // 
            // colAXIS_LENGTH_2
            // 
            this.colAXIS_LENGTH_2.Caption = "2";
            this.colAXIS_LENGTH_2.FieldName = "AXIS_LENGTH_2";
            this.colAXIS_LENGTH_2.Name = "colAXIS_LENGTH_2";
            this.colAXIS_LENGTH_2.Visible = true;
            this.colAXIS_LENGTH_2.VisibleIndex = 4;
            this.colAXIS_LENGTH_2.Width = 30;
            // 
            // colAXIS_LENGTH_3
            // 
            this.colAXIS_LENGTH_3.Caption = "3";
            this.colAXIS_LENGTH_3.FieldName = "AXIS_LENGTH_3";
            this.colAXIS_LENGTH_3.Name = "colAXIS_LENGTH_3";
            this.colAXIS_LENGTH_3.Visible = true;
            this.colAXIS_LENGTH_3.VisibleIndex = 5;
            this.colAXIS_LENGTH_3.Width = 30;
            // 
            // colAXIS_LENGTH_4
            // 
            this.colAXIS_LENGTH_4.Caption = "4";
            this.colAXIS_LENGTH_4.FieldName = "AXIS_LENGTH_4";
            this.colAXIS_LENGTH_4.Name = "colAXIS_LENGTH_4";
            this.colAXIS_LENGTH_4.Visible = true;
            this.colAXIS_LENGTH_4.VisibleIndex = 6;
            this.colAXIS_LENGTH_4.Width = 30;
            // 
            // colAXIS_LENGTH_5
            // 
            this.colAXIS_LENGTH_5.Caption = "5";
            this.colAXIS_LENGTH_5.FieldName = "AXIS_LENGTH_5";
            this.colAXIS_LENGTH_5.Name = "colAXIS_LENGTH_5";
            this.colAXIS_LENGTH_5.Visible = true;
            this.colAXIS_LENGTH_5.VisibleIndex = 7;
            this.colAXIS_LENGTH_5.Width = 30;
            // 
            // colAXIS_LENGTH_6
            // 
            this.colAXIS_LENGTH_6.Caption = "6";
            this.colAXIS_LENGTH_6.FieldName = "AXIS_LENGTH_6";
            this.colAXIS_LENGTH_6.Name = "colAXIS_LENGTH_6";
            this.colAXIS_LENGTH_6.Visible = true;
            this.colAXIS_LENGTH_6.VisibleIndex = 8;
            this.colAXIS_LENGTH_6.Width = 30;
            // 
            // colAXIS_LENGTH_7
            // 
            this.colAXIS_LENGTH_7.Caption = "7";
            this.colAXIS_LENGTH_7.FieldName = "AXIS_LENGTH_7";
            this.colAXIS_LENGTH_7.Name = "colAXIS_LENGTH_7";
            this.colAXIS_LENGTH_7.Visible = true;
            this.colAXIS_LENGTH_7.VisibleIndex = 9;
            this.colAXIS_LENGTH_7.Width = 30;
            // 
            // colAXIS_LENGTH_8
            // 
            this.colAXIS_LENGTH_8.Caption = "8";
            this.colAXIS_LENGTH_8.FieldName = "AXIS_LENGTH_8";
            this.colAXIS_LENGTH_8.Name = "colAXIS_LENGTH_8";
            this.colAXIS_LENGTH_8.Visible = true;
            this.colAXIS_LENGTH_8.VisibleIndex = 10;
            this.colAXIS_LENGTH_8.Width = 30;
            // 
            // colAXIS_LENGTH_9
            // 
            this.colAXIS_LENGTH_9.Caption = "9";
            this.colAXIS_LENGTH_9.FieldName = "AXIS_LENGTH_9";
            this.colAXIS_LENGTH_9.Name = "colAXIS_LENGTH_9";
            this.colAXIS_LENGTH_9.Visible = true;
            this.colAXIS_LENGTH_9.VisibleIndex = 11;
            this.colAXIS_LENGTH_9.Width = 30;
            // 
            // colAXIS_LENGTH_10
            // 
            this.colAXIS_LENGTH_10.Caption = "10";
            this.colAXIS_LENGTH_10.FieldName = "AXIS_LENGTH_10";
            this.colAXIS_LENGTH_10.Name = "colAXIS_LENGTH_10";
            this.colAXIS_LENGTH_10.Visible = true;
            this.colAXIS_LENGTH_10.VisibleIndex = 12;
            this.colAXIS_LENGTH_10.Width = 30;
            // 
            // colAXIS_LENGTH_11
            // 
            this.colAXIS_LENGTH_11.Caption = "11";
            this.colAXIS_LENGTH_11.FieldName = "AXIS_LENGTH_11";
            this.colAXIS_LENGTH_11.Name = "colAXIS_LENGTH_11";
            this.colAXIS_LENGTH_11.Visible = true;
            this.colAXIS_LENGTH_11.VisibleIndex = 13;
            this.colAXIS_LENGTH_11.Width = 30;
            // 
            // colAXIS_LENGTH_12
            // 
            this.colAXIS_LENGTH_12.Caption = "12";
            this.colAXIS_LENGTH_12.FieldName = "AXIS_LENGTH_12";
            this.colAXIS_LENGTH_12.Name = "colAXIS_LENGTH_12";
            this.colAXIS_LENGTH_12.Visible = true;
            this.colAXIS_LENGTH_12.VisibleIndex = 14;
            this.colAXIS_LENGTH_12.Width = 30;
            // 
            // ModelVehicleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 323);
            this.Controls.Add(this.gridControl1);
            this.Name = "ModelVehicleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Модели";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ModelVehicleForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pVKMODELVEHICLEBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pVKds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraGrid.GridControl gridControl1;
		private System.Windows.Forms.BindingSource pVKMODELVEHICLEBindingSource;
        private PVKds pVKds;
		private DevExpress.XtraGrid.Columns.GridColumn colMODEL;
		private DevExpress.XtraGrid.Columns.GridColumn colCOUNT_AXIS;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_1;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_2;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_3;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_4;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_5;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_6;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_7;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_8;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_9;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_10;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_11;
		private DevExpress.XtraGrid.Columns.GridColumn colAXIS_LENGTH_12;
		private DevExpress.XtraGrid.Columns.GridColumn colLINK_LENGTH;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;

	}
}