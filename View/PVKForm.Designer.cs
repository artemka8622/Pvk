namespace PVK.Control.View
{
	partial class PVKForm
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
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.pvKds1 = new PVK.Control.PVKds();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colNOMER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gv1Name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colORGANIZATIONS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSPECIALIST_PVK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colINSPECTOR_PVK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colINSPECTOR_GIBDD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSPRING_LIMIT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colNUMBER_COM_PORT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCAMERA_URL = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCAMERA_PASS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCAMERA_LOGIN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pvKds1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.DataSource = this.bindingSource1;
            this.gridControl1.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gridControl1_EmbeddedNavigator_ButtonClick);
            this.gridControl1.Location = new System.Drawing.Point(0, 3);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemTextEdit1,
            this.repositoryItemCheckEdit1});
            this.gridControl1.Size = new System.Drawing.Size(681, 278);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.UseEmbeddedNavigator = true;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "PVK";
            this.bindingSource1.DataSource = this.pvKds1;
            // 
            // pvKds1
            // 
            this.pvKds1.DataSetName = "PVKds";
            this.pvKds1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colNOMER,
            this.gv1Name,
            this.colORGANIZATIONS,
            this.colSPECIALIST_PVK,
            this.colINSPECTOR_PVK,
            this.colINSPECTOR_GIBDD,
            this.colSPRING_LIMIT,
            this.colNUMBER_COM_PORT,
            this.colCAMERA_URL,
            this.colCAMERA_PASS,
            this.colCAMERA_LOGIN});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colNOMER
            // 
            this.colNOMER.Caption = "Номер ПВК";
            this.colNOMER.FieldName = "NOMER";
            this.colNOMER.Name = "colNOMER";
            this.colNOMER.Visible = true;
            this.colNOMER.VisibleIndex = 0;
            // 
            // gv1Name
            // 
            this.gv1Name.Caption = "Место расположения ПВК";
            this.gv1Name.FieldName = "ADRESS";
            this.gv1Name.Name = "gv1Name";
            this.gv1Name.Visible = true;
            this.gv1Name.VisibleIndex = 1;
            // 
            // colORGANIZATIONS
            // 
            this.colORGANIZATIONS.Caption = "Организация обслуживающая ПВК";
            this.colORGANIZATIONS.FieldName = "ORGANIZATIONS";
            this.colORGANIZATIONS.Name = "colORGANIZATIONS";
            this.colORGANIZATIONS.Visible = true;
            this.colORGANIZATIONS.VisibleIndex = 2;
            // 
            // colSPECIALIST_PVK
            // 
            this.colSPECIALIST_PVK.Caption = "ФИО Специалиста ПВК";
            this.colSPECIALIST_PVK.FieldName = "SPECIALIST_PVK";
            this.colSPECIALIST_PVK.Name = "colSPECIALIST_PVK";
            this.colSPECIALIST_PVK.Visible = true;
            this.colSPECIALIST_PVK.VisibleIndex = 3;
            // 
            // colINSPECTOR_PVK
            // 
            this.colINSPECTOR_PVK.Caption = "ФИО Инспектора ПВК";
            this.colINSPECTOR_PVK.FieldName = "INSPECTOR_PVK";
            this.colINSPECTOR_PVK.Name = "colINSPECTOR_PVK";
            this.colINSPECTOR_PVK.Visible = true;
            this.colINSPECTOR_PVK.VisibleIndex = 4;
            // 
            // colINSPECTOR_GIBDD
            // 
            this.colINSPECTOR_GIBDD.Caption = "ФИО Инспектора ГИБДД";
            this.colINSPECTOR_GIBDD.FieldName = "INSPECTOR_GIBDD";
            this.colINSPECTOR_GIBDD.Name = "colINSPECTOR_GIBDD";
            this.colINSPECTOR_GIBDD.Visible = true;
            this.colINSPECTOR_GIBDD.VisibleIndex = 5;
            // 
            // colSPRING_LIMIT
            // 
            this.colSPRING_LIMIT.Caption = "Наличие весеннего ограничения на перевозку грузов";
            this.colSPRING_LIMIT.ColumnEdit = this.repositoryItemCheckEdit1;
            this.colSPRING_LIMIT.FieldName = "SPRING_LIMIT";
            this.colSPRING_LIMIT.Name = "colSPRING_LIMIT";
            this.colSPRING_LIMIT.Visible = true;
            this.colSPRING_LIMIT.VisibleIndex = 6;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // colNUMBER_COM_PORT
            // 
            this.colNUMBER_COM_PORT.Caption = "Номер порта весов";
            this.colNUMBER_COM_PORT.FieldName = "NUMBER_COM_PORT";
            this.colNUMBER_COM_PORT.Name = "colNUMBER_COM_PORT";
            // 
            // colCAMERA_URL
            // 
            this.colCAMERA_URL.Caption = "Веб адрес камеры";
            this.colCAMERA_URL.FieldName = "CAMERA_URL";
            this.colCAMERA_URL.Name = "colCAMERA_URL";
            // 
            // colCAMERA_PASS
            // 
            this.colCAMERA_PASS.Caption = "Пароль(камеры)";
            this.colCAMERA_PASS.FieldName = "CAMERA_PASS";
            this.colCAMERA_PASS.Name = "colCAMERA_PASS";
            // 
            // colCAMERA_LOGIN
            // 
            this.colCAMERA_LOGIN.Caption = "Имя пользователя(камеры)";
            this.colCAMERA_LOGIN.FieldName = "CAMERA_LOGIN";
            this.colCAMERA_LOGIN.Name = "colCAMERA_LOGIN";
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name")});
            this.repositoryItemLookUpEdit1.DisplayMember = "Name";
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.ShowFooter = false;
            this.repositoryItemLookUpEdit1.ShowHeader = false;
            this.repositoryItemLookUpEdit1.ValueMember = "Id";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.DisplayFormat.FormatString = "f6";
            this.repositoryItemTextEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemTextEdit1.Mask.EditMask = "f6";
            this.repositoryItemTextEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(598, 287);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Выбрать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PVKForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 313);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.button1);
            this.Name = "PVKForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Пункты весового контроля";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PVKForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pvKds1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraGrid.GridControl gridControl1;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
		private DevExpress.XtraGrid.Columns.GridColumn gv1Name;
		private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
		private System.Windows.Forms.Button button1;
		private PVKds pvKds1;
		private DevExpress.XtraGrid.Columns.GridColumn colNOMER;
		private DevExpress.XtraGrid.Columns.GridColumn colORGANIZATIONS;
		private DevExpress.XtraGrid.Columns.GridColumn colSPECIALIST_PVK;
		private DevExpress.XtraGrid.Columns.GridColumn colINSPECTOR_PVK;
		private DevExpress.XtraGrid.Columns.GridColumn colSPRING_LIMIT;
		private DevExpress.XtraGrid.Columns.GridColumn colINSPECTOR_GIBDD;
		private System.Windows.Forms.BindingSource bindingSource1;
		private DevExpress.XtraGrid.Columns.GridColumn colNUMBER_COM_PORT;
		private DevExpress.XtraGrid.Columns.GridColumn colCAMERA_URL;
		private DevExpress.XtraGrid.Columns.GridColumn colCAMERA_PASS;
		private DevExpress.XtraGrid.Columns.GridColumn colCAMERA_LOGIN;
	}
}