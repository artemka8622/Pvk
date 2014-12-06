using System;
using TRLibrary.Commons;
using TRLibrary2;
using System.Data;
using PVK.Data;
using System.Collections.Generic;

namespace PVK.Control
{
    public class MainPresenter
    {
        private IMainView _view;
        public readonly bool IsAuthorization;
        public event EventHandler<EventArgs> Close;

        private void OnClose(EventArgs e)
        {
            var handler = Close;
            if (handler != null) handler(this, e);
        }

        public MainPresenter(IViewFactory viewFactory)
         {

		}

        public int MainPresenterInit(IViewFactory viewFactory)
        {
            _view = viewFactory.GetMainView();
            _view.MainText = string.Format("АРМ TraceReports {0} {1}", CentralModule.ProductVersionGet(), StringsConverter.MySqlDateString(CentralModule.TrReleaseDate));
            _view.Closed += ViewClosed;
 
            return 0;
        }
			
        private void ViewClosed(object sender, EventArgs eventArgs)
        {
            OnClose(EventArgs.Empty);
        }

        public void ShowView()
        {
            _view.Show();
        }	   

    }
}
