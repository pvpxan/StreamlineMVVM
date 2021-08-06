using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace StreamlineMVVM
{
    // Needed to extend a dialog control viewmodel.
    public abstract class DialogViewModel : ViewModelBase
    {
        public WindowMessageResult Result { get; set; } = WindowMessageResult.Undefined;
        public DialogData Data { get; private set; } = null;
        public readonly DialogEvents Events = new DialogEvents();

        public DialogViewModel(DialogData dialogData) : base()
        {
            Data = dialogData;
        }

        // Window loaded event.
        public Action<object, RoutedEventArgs> OnContentLoaded { get; set; } = null;
        public void ContentLoaded(object sender, RoutedEventArgs e)
        {
            if (OnContentLoaded != null)
            {
                OnContentLoaded(sender, e);
            }
        }

        // Window rendered event.
        public Action<object, EventArgs> OnContentRendered { get; set; } = null;
        public void ContentRendered(object sender, EventArgs e)
        {
            if (OnContentRendered != null)
            {
                OnContentRendered(sender, e);
            }
        }

        // Window closing event.
        public Action<object, CancelEventArgs> OnWindowClosing { get; set; } = null;
        public void WindowClosing(object sender, CancelEventArgs e)
        {
            if (OnWindowClosing != null)
            {
                OnWindowClosing(sender, e);
            }
        }

        public void CloseDialogWithResult(WindowMessageResult result)
        {
            Result = result;
            Events.CloseDialog();
        }
    }
}
