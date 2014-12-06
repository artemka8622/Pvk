
namespace PVK.Control
{
    sealed partial class MainForm
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
            this.pvKds1 = new PVK.Control.PVKds();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDATE_WEIGHING = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPVK_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOWNER_VEHICLE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMODEL_VEHICLE_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVEHICLE_REG_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMODEL_TRAILER_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTRAILER_REG_NUMBER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colROUTE_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colACT_NOMER = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraUserControl1 = new DevExpress.XtraEditors.XtraUserControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lable1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem12 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem5 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem13 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem14 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem7 = new DevExpress.XtraBars.BarSubItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::PVK.Control.WaitForm1), true, true);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pvKds1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.DataMember = "PVK_WEIGHING";
            this.gridControl1.DataSource = this.pvKds1;
            this.gridControl1.Location = new System.Drawing.Point(4, 56);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(861, 234);
            this.gridControl1.TabIndex = 21;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // pvKds1
            // 
            this.pvKds1.DataSetName = "PVKds";
            this.pvKds1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDATE_WEIGHING,
            this.colPVK_ID,
            this.colOWNER_VEHICLE,
            this.colMODEL_VEHICLE_ID,
            this.colVEHICLE_REG_NUMBER,
            this.colMODEL_TRAILER_ID,
            this.colTRAILER_REG_NUMBER,
            this.colROUTE_NAME,
            this.colACT_NOMER});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
            this.gridView1.OptionsDetail.AllowExpandEmptyDetails = true;
            this.gridView1.OptionsDetail.AllowOnlyOneMasterRowExpanded = true;
            this.gridView1.OptionsDetail.AllowZoomDetail = false;
            this.gridView1.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // colDATE_WEIGHING
            // 
            this.colDATE_WEIGHING.Caption = "Дата, время взвешивания";
            this.colDATE_WEIGHING.FieldName = "DATE_WEIGHING";
            this.colDATE_WEIGHING.Name = "colDATE_WEIGHING";
            this.colDATE_WEIGHING.Visible = true;
            this.colDATE_WEIGHING.VisibleIndex = 0;
            // 
            // colPVK_ID
            // 
            this.colPVK_ID.Caption = "Место расположения ПВК";
            this.colPVK_ID.FieldName = "PVK_ID";
            this.colPVK_ID.Name = "colPVK_ID";
            this.colPVK_ID.Visible = true;
            this.colPVK_ID.VisibleIndex = 1;
            // 
            // colOWNER_VEHICLE
            // 
            this.colOWNER_VEHICLE.Caption = "Наименование владельца ТС";
            this.colOWNER_VEHICLE.FieldName = "OWNER_VEHICLE";
            this.colOWNER_VEHICLE.Name = "colOWNER_VEHICLE";
            this.colOWNER_VEHICLE.Visible = true;
            this.colOWNER_VEHICLE.VisibleIndex = 2;
            // 
            // colMODEL_VEHICLE_ID
            // 
            this.colMODEL_VEHICLE_ID.Caption = "Марка, модель ТС ";
            this.colMODEL_VEHICLE_ID.FieldName = "MODEL_VEHICLE_ID";
            this.colMODEL_VEHICLE_ID.Name = "colMODEL_VEHICLE_ID";
            this.colMODEL_VEHICLE_ID.Visible = true;
            this.colMODEL_VEHICLE_ID.VisibleIndex = 3;
            // 
            // colVEHICLE_REG_NUMBER
            // 
            this.colVEHICLE_REG_NUMBER.Caption = "Гос.номер ТС";
            this.colVEHICLE_REG_NUMBER.FieldName = "VEHICLE_REG_NUMBER";
            this.colVEHICLE_REG_NUMBER.Name = "colVEHICLE_REG_NUMBER";
            this.colVEHICLE_REG_NUMBER.Visible = true;
            this.colVEHICLE_REG_NUMBER.VisibleIndex = 4;
            // 
            // colMODEL_TRAILER_ID
            // 
            this.colMODEL_TRAILER_ID.Caption = "Марка, модель прицепа";
            this.colMODEL_TRAILER_ID.FieldName = "MODEL_TRAILER_ID";
            this.colMODEL_TRAILER_ID.Name = "colMODEL_TRAILER_ID";
            this.colMODEL_TRAILER_ID.Visible = true;
            this.colMODEL_TRAILER_ID.VisibleIndex = 5;
            // 
            // colTRAILER_REG_NUMBER
            // 
            this.colTRAILER_REG_NUMBER.Caption = "Гос. Номер прицепа";
            this.colTRAILER_REG_NUMBER.FieldName = "TRAILER_REG_NUMBER";
            this.colTRAILER_REG_NUMBER.Name = "colTRAILER_REG_NUMBER";
            this.colTRAILER_REG_NUMBER.Visible = true;
            this.colTRAILER_REG_NUMBER.VisibleIndex = 6;
            // 
            // colROUTE_NAME
            // 
            this.colROUTE_NAME.Caption = "Маршрут движения ТС ";
            this.colROUTE_NAME.FieldName = "ROUTE_NAME";
            this.colROUTE_NAME.Name = "colROUTE_NAME";
            this.colROUTE_NAME.Visible = true;
            this.colROUTE_NAME.VisibleIndex = 7;
            // 
            // colACT_NOMER
            // 
            this.colACT_NOMER.Caption = "Номер акта";
            this.colACT_NOMER.FieldName = "ACT_NOMER";
            this.colACT_NOMER.Name = "colACT_NOMER";
            this.colACT_NOMER.Visible = true;
            this.colACT_NOMER.VisibleIndex = 8;
            // 
            // xtraUserControl1
            // 
            this.xtraUserControl1.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xtraUserControl1.Appearance.Options.UseBackColor = true;
            this.xtraUserControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.xtraUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraUserControl1.Location = new System.Drawing.Point(0, 22);
            this.xtraUserControl1.Name = "xtraUserControl1";
            this.xtraUserControl1.Size = new System.Drawing.Size(871, 315);
            this.xtraUserControl1.TabIndex = 6;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(320, 29);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 20);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "Обновить";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Location = new System.Drawing.Point(790, 29);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 20);
            this.simpleButton2.TabIndex = 13;
            this.simpleButton2.Text = "Добавить";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(0, 13);
            this.labelControl1.TabIndex = 0;
            // 
            // lable1
            // 
            this.lable1.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lable1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.lable1.Location = new System.Drawing.Point(4, 33);
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size(7, 13);
            this.lable1.TabIndex = 19;
            this.lable1.Text = "C";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.labelControl2.Location = new System.Drawing.Point(153, 33);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 13);
            this.labelControl2.TabIndex = 20;
            this.labelControl2.Text = "по";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd:MM:yyyy HH:mm";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(17, 28);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dateTimePicker1.Size = new System.Drawing.Size(124, 21);
            this.dateTimePicker1.TabIndex = 27;
            this.dateTimePicker1.Value = new System.DateTime(2013, 9, 6, 0, 0, 0, 0);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "dd:MM:yyyy HH:mm";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(182, 28);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dateTimePicker2.Size = new System.Drawing.Size(124, 21);
            this.dateTimePicker2.TabIndex = 28;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton3.Location = new System.Drawing.Point(709, 29);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(75, 20);
            this.simpleButton3.TabIndex = 34;
            this.simpleButton3.Text = "Печать";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Файл";
            this.barSubItem1.Id = 0;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem12),
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.Caption | DevExpress.XtraBars.BarLinkUserDefines.PaintStyle))), this.barButtonItem11, "Настройка весов", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem8),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem10)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "Пункты весового контроля";
            this.barButtonItem7.Id = 41;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // barButtonItem12
            // 
            this.barButtonItem12.Caption = "Настройка оборудования";
            this.barButtonItem12.Id = 48;
            this.barButtonItem12.Name = "barButtonItem12";
            this.barButtonItem12.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem12_ItemClick_1);
            // 
            // barButtonItem11
            // 
            this.barButtonItem11.Caption = "Настройка весов";
            this.barButtonItem11.Id = 45;
            this.barButtonItem11.Name = "barButtonItem11";
            this.barButtonItem11.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem11_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "Марки автомобилей";
            this.barButtonItem5.Id = 39;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "Марки прицепов";
            this.barButtonItem6.Id = 40;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "Характер грузов";
            this.barButtonItem8.Id = 42;
            this.barButtonItem8.Name = "barButtonItem8";
            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem8_ItemClick);
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "Характер нарушений";
            this.barButtonItem9.Id = 43;
            this.barButtonItem9.Name = "barButtonItem9";
            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
            // 
            // barButtonItem10
            // 
            this.barButtonItem10.Caption = "Реквизиты и константы";
            this.barButtonItem10.Id = 44;
            this.barButtonItem10.Name = "barButtonItem10";
            this.barButtonItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem10_ItemClick);
            // 
            // barSubItem5
            // 
            this.barSubItem5.Caption = "Сервис";
            this.barSubItem5.Id = 11;
            this.barSubItem5.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem13),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem14)});
            this.barSubItem5.Name = "barSubItem5";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Синхронизация с сервером";
            this.barButtonItem1.Id = 13;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarButtonItem1ItemClick);
            // 
            // barButtonItem13
            // 
            this.barButtonItem13.Caption = "Установить номер ПВК";
            this.barButtonItem13.Id = 47;
            this.barButtonItem13.Name = "barButtonItem13";
            this.barButtonItem13.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem13_ItemClick);
            // 
            // barButtonItem14
            // 
            this.barButtonItem14.Caption = "Параметры фильтрации";
            this.barButtonItem14.Id = 49;
            this.barButtonItem14.Name = "barButtonItem14";
            this.barButtonItem14.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem14_ItemClick);
            // 
            // barSubItem7
            // 
            this.barSubItem7.Caption = "Помощь";
            this.barSubItem7.Id = 22;
            this.barSubItem7.Name = "barSubItem7";
            // 
            // bar2
            // 
            this.bar2.BarAppearance.Normal.BorderColor = System.Drawing.Color.Silver;
            this.bar2.BarAppearance.Normal.Options.UseBorderColor = true;
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.FloatLocation = new System.Drawing.Point(80, 156);
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem7)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DisableClose = true;
            this.bar2.OptionsBar.DisableCustomization = true;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(871, 22);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 337);
            this.barDockControlBottom.Size = new System.Drawing.Size(871, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 22);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 315);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(871, 22);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 315);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barSubItem5,
            this.barButtonItem1,
            this.barSubItem7,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barButtonItem8,
            this.barButtonItem9,
            this.barButtonItem10,
            this.barButtonItem11,
            this.barButtonItem13,
            this.barButtonItem12,
            this.barButtonItem14});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 50;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 337);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lable1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.xtraUserControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IsMdiContainer = true;
            this.LookAndFeel.SkinName = "Office 2010 Silver";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Весовой контроль";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pvKds1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit2;
        //private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        ////private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit3;
        ////private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit7;
        ////private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit8;
        ////private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit9;
        ////private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit10;
        //private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit4;
        //private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit11;
        //private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit13;
        //private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        //private VehiclesView vehiclesView1;
		//private OrganizationView organisationView1;
		//private DriversView driversView1;
		//private GeozonesGroupView geozonesGroupView1;
        //private ResolutionView resolutionView1;
		//private TripView tripView1;
		//private PlaningView2 planingView1;
		//private ReasonsForFailureView reasonsForFailureView1;
		//private RoutesPassportView routesPassportView1;
		//private TerminalHistoryView terminalHistoryView1;
		//private TripsTemplatesView tripsTemplatesView1;
		//private ScheduleView scheduleView1;
        //private AreasView areasView1;
        //private RoadPartsView roadPartsView1;
        //private RegularityView regularityView1;
        //private MediaDatasView mediaDatasView1;
        //private CamerasView camerasView1;
        //private WeatherEventsView weatherEventsView1;
        //private DirectionsView directionsView1;
        //private RouteNetworksView routeNetworksView1;
        //private AreaExtendedInfosView areaExtendedInfosView1;
		//private WorkGroupParametersView workGroupParametersView1;
        //private RecommendationsView recommendationsView1;
        //private WeatherRemoveTimesView weatherRemoveTimesView1;
		private DevExpress.XtraEditors.XtraUserControl xtraUserControl1;
		private DevExpress.XtraEditors.SimpleButton simpleButton1;
		private DevExpress.XtraEditors.SimpleButton simpleButton2;
		private DevExpress.XtraEditors.LabelControl labelControl1;
		private DevExpress.XtraEditors.LabelControl labelControl2;
		private DevExpress.XtraEditors.LabelControl lable1;
        private DevExpress.XtraGrid.GridControl gridControl1;
		private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
		private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.DateTimePicker dateTimePicker2;
		private PVKds pvKds1;
		private DevExpress.XtraGrid.Columns.GridColumn colDATE_WEIGHING;
		private DevExpress.XtraGrid.Columns.GridColumn colPVK_ID;
		private DevExpress.XtraGrid.Columns.GridColumn colOWNER_VEHICLE;
		private DevExpress.XtraGrid.Columns.GridColumn colMODEL_VEHICLE_ID;
		private DevExpress.XtraGrid.Columns.GridColumn colVEHICLE_REG_NUMBER;
		private DevExpress.XtraGrid.Columns.GridColumn colMODEL_TRAILER_ID;
		private DevExpress.XtraGrid.Columns.GridColumn colTRAILER_REG_NUMBER;
        private DevExpress.XtraGrid.Columns.GridColumn colROUTE_NAME;
        private DevExpress.XtraGrid.Columns.GridColumn colACT_NOMER;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarButtonItem barButtonItem12;
        private DevExpress.XtraBars.BarButtonItem barButtonItem11;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
        private DevExpress.XtraBars.BarSubItem barSubItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem13;
        private DevExpress.XtraBars.BarButtonItem barButtonItem14;
        private DevExpress.XtraBars.BarSubItem barSubItem7;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
		//private DataDiagnosticsView dataDiagnosticsView1;
    }
}
