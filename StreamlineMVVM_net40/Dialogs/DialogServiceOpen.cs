using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    public static partial class DialogService
    {
        // When this class is first accessed, we need to know what access level it has.
        public static bool CheckApplicationUIThreadAccess()
        {
            try
            {
                return Application.Current.Dispatcher.CheckAccess();
            }
            catch
            {
                return false;
            }
        }

        public static ShutdownMode CurrentApplicationShutdownMode()
        {
            try
            {
                if (CheckApplicationUIThreadAccess())
                {
                    return Application.Current.ShutdownMode;
                }

                return ShutdownMode.OnLastWindowClose;
            }
            catch
            {
                return ShutdownMode.OnLastWindowClose;
            }
        }

        // Primary method for opening a dialog.
        private static WindowMessageResult openDialogWork(Window dialogWindow, DialogViewModel dialogWindowViewModel, Window parentWindow, ShutdownMode shutdownMode)
        {
            bool applicationUIThreadAccess = CheckApplicationUIThreadAccess();
            if (applicationUIThreadAccess && shutdownMode != CurrentApplicationShutdownMode())
            {
                // Param shutdownMode can used to prevent the Application.Current from going null. It can be turned off by setting ApplicationExplicitShutdown.
                // This should not throw. Possible investigation should be made to check on this.
                Application.Current.ShutdownMode = shutdownMode;
            }

            // We need to find a dispatcher to display this message.
            Dispatcher dispatcher = null;
            if (parentWindow == null && applicationUIThreadAccess == false)
            {
                return WindowMessageResult.Undefined;
            }
            else if (parentWindow == null && applicationUIThreadAccess)
            {
                try
                {
                    dialogWindow.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
                }
                catch
                {
                    // This should not throw. Possible investigation should be made to check on this.
                    if (Application.Current.Windows.Count > 0)
                    {
                        dialogWindow.Owner = Application.Current.Windows[0];
                    }
                    else
                    {
                        dialogWindow.Owner = null;
                    }
                }

                if (dialogWindow.Owner == null)
                {
                    dispatcher = Application.Current.Dispatcher;
                }
                else
                {
                    dispatcher = dialogWindow.Owner.Dispatcher;
                }
            }
            else // This should ONLY happen if parentWindow is NOT null.
            {
                dialogWindow.Owner = parentWindow;
                dispatcher = parentWindow.Dispatcher;
            }

            try
            {
                if (dispatcher.CheckAccess() == false)
                {
                    return WindowMessageResult.Undefined;
                }

                WindowMessageResult result = WindowMessageResult.Undefined;
                dispatcher.Invoke((Action)delegate
                {
                    dialogWindow.ShowDialog();

                    result = dialogWindowViewModel.Result;
                });

                return result;
            }
            catch
            {
                return WindowMessageResult.Undefined;
            }
        }

        // -------------------------------------------------------------------------------------------------------
        // OpenDialog base function with a provided window.
        public static WindowMessageResult OpenDialog(DialogWindowView dialogWindowView, Window parentWindow, ShutdownMode shutdownMode)
        {
            if (dialogWindowView.IsValid == false)
            {
                return WindowMessageResult.Undefined;
            }

            return openDialogWork(dialogWindowView.DialogWindow, dialogWindowView.WindowViewModel, parentWindow, shutdownMode);
        }

        // OpenDialog with defined parent window and using current Application Object ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogWindowView dialogWindowView, Window parentWindow)
        {
            return OpenDialog(dialogWindowView, parentWindow, CurrentApplicationShutdownMode());
        }

        // OpenDialog with no parent window object and passed in ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogWindowView dialogWindowView, ShutdownMode shutdownMode)
        {
            return OpenDialog(dialogWindowView, null, shutdownMode);
        }

        // OpenDialog with no parent window and using current Application Object ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogWindowView dialogWindowView)
        {
            return OpenDialog(dialogWindowView, null, CurrentApplicationShutdownMode());
        }

        // -------------------------------------------------------------------------------------------------------
        // OpenDialog base function with a provided user control.
        public static WindowMessageResult OpenDialog(DialogUserControlView dialogUserControlView, Window parentWindow, ShutdownMode shutdownMode)
        {
            if (dialogUserControlView.IsValid == false)
            {
                return WindowMessageResult.Undefined;
            }

            return openDialogWork(dialogUserControlView.BaseDialogWindowView, dialogUserControlView.BaseDialogWindowViewModel, parentWindow, shutdownMode);
        }

        // OpenDialog with defined parent window and using current Application Object ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogUserControlView dialogViews, Window parentWindow)
        {
            return OpenDialog(dialogViews, parentWindow, CurrentApplicationShutdownMode());
        }

        // OpenDialog with no parent window object and passed in ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogUserControlView dialogViews, ShutdownMode shutdownMode)
        {
            return OpenDialog(dialogViews, null, shutdownMode);
        }

        // OpenDialog with no parent window and using current Application Object ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogUserControlView dialogViews)
        {
            return OpenDialog(dialogViews, null, CurrentApplicationShutdownMode());
        }
    }
}
