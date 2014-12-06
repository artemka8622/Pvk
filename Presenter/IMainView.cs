using System;


namespace PVK.Control
{
    public interface IMainView
    {
        void Show();

        string MainText { get; set; }
        event EventHandler<EventArgs> Closed;
        void ShowView(object view);
    }
}