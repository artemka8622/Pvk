namespace PVK.Control.View
{
	partial class ScalesSettingsForm
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
            this.pVKSCALESBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pVKds = new PVK.Control.PVKds();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colNomer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDateChecking = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pVKSCALESBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pVKds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.pVKSCALESBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridControl1.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gridControl1_EmbeddedNavigator_ButtonClick_1);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(764, 406);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.UseEmbeddedNavigator = true;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // pVKSCALESBindingSource
            // 
            this.pVKSCALESBindingSource.DataMember = "PVK_SCALES";
            this.pVKSCALESBindingSource.DataSource = this.pVKds;
            // 
            // pVKds
            // 
            this.pVKds.DataSetName = "PVKds";
            this.pVKds.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colNomer,
            this.colName,
            this.colDateChecking});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colNomer
            // 
            this.colNomer.Caption = "Номер";
            this.colNomer.FieldName = "Nomer";
            this.colNomer.Name = "colNomer";
            this.colNomer.Visible = true;
            this.colNomer.VisibleIndex = 0;
            // 
            // colName
            // 
            this.colName.Caption = "Имя";
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 1;
            // 
            // colDateChecking
            // 
            this.colDateChecking.Caption = "Дата проверки";
            this.colDateChecking.FieldName = "DateChecking";
            this.colDateChecking.Name = "colDateChecking";
            this.colDateChecking.Visible = true;
            this.colDateChecking.VisibleIndex = 2;
            // 
            // ScalesSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 406);
            this.Controls.Add(this.gridControl1);
            this.Name = "ScalesSettingsForm";
            this.Text = "Настройка весов";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pVKSCALESBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pVKds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraGrid.GridControl gridControl1;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
		private System.Windows.Forms.BindingSource pVKSCALESBindingSource;
		private PVKds pVKds;
		private DevExpress.XtraGrid.Columns.GridColumn colNomer;
		private DevExpress.XtraGrid.Columns.GridColumn colName;
		private DevExpress.XtraGrid.Columns.GridColumn colDateChecking;
	}
}