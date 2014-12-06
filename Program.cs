using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
//using Vlc.DotNet.Core;

namespace PVK.Control
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            bool result;
            var mutex = new System.Threading.Mutex(true, "UniqueAppId", out result);

            if (!result)
            {
                MessageBox.Show("Другая версия программы уже запущена.");
                return;
            }
//            #if DEBUG
//                VlcContext.LibVlcDllsPath = @"..\..\..\__bin\vlc"; // CommonStrings.LIBVLC_DLLS_PATH_DEFAULT_VALUE_AMD64;
//            #else
//                VlcContext.LibVlcDllsPath = @".\vlc"; // CommonStrings.LIBVLC_DLLS_PATH_DEFAULT_VALUE_AMD64;
//#endif

//                // Set the vlc plugins directory path
//            //VlcContext.LibVlcPluginsPath = @".\vlc\pugins"; //CommonStrings.PLUGINS_PATH_DEFAULT_VALUE_AMD64;

//            // Ignore the VLC configuration file
//            VlcContext.StartupOptions.IgnoreConfig = false;

//            // Enable file based logging
//            VlcContext.StartupOptions.LogOptions.LogInFile = false;

//            // Shows the VLC log console (in addition to the applications window)
//            VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = false;

//            // Set the log level for the VLC instance
//            VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Standard;

//            // Initialize the VlcContext
//            VlcContext.Initialize();
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //GC.KeepAlive(mutex); 
		}
	}
}

