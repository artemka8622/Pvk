using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace PVK.Control.View
{
	public partial class PhotoView : DevExpress.XtraEditors.XtraForm
	{
		public Image Photo { get
		{
			return pictureEdit1.Image; 	
		}
		set
		{
			pictureEdit1.Image = value;	
		}
		}
		public PhotoView()
		{
			InitializeComponent();
		}
	}
}