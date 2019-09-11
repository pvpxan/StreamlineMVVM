using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace StreamlineMVVM
{
    public enum WindowMessageResult
    {
        Undefined,
        Yes,
        No,
        Ok,
        Continue,
        Cancel,
        Exit,
        Accept,
        Decline,
        Error,
        Misc1,
        Misc2,
        Misc3,
    }

    public enum WindowMessageButtons
    {
        Default,
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel,
        Exit,
        ContinueCancel,
        AcceptDecline,
        Misc,
    }

    public enum WindowMessageIcon
    {
        Application,
        Asterisk,
        Error,
        Exclamation,
        Hand,
        Information,
        Question,
        Shield,
        Warning,
        WinLogo
    }

    // Makes it easier to change this for later.
    public static class ColorSets
    {
        public static Brush Background { get; } = Brushes.White;

        public static Brush ContentHeaderColor { get; } = Brushes.DarkBlue;
        public static Brush ContentBodyColor { get; } = Brushes.Black;

        public static Brush HyperLinkColor { get; } = Brushes.Blue;
        public static Brush HyperLinkMouseOverColor { get; } = Brushes.Red;
        public static Brush HyperLinkMouseDisabledColor { get; } = Brushes.Gray;

        // If your Hex string is not well formed, this will return black.
        public static Brush HexConverter(string hexCode)
        {
            Brush brush = Brushes.Black;

            if (string.IsNullOrEmpty(hexCode) || ValidateHexCode(hexCode) == false)
            {
                return brush;
            }

            var converter = new BrushConverter();
            try
            {
                brush = (Brush)converter.ConvertFromString(hexCode);
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error getting color from hex code converter.", Ex);
                brush = Brushes.Black;
            }

            return brush;
        }

        public static bool ValidateHexCode(string hexCode)
        {
            if (string.IsNullOrEmpty(hexCode) || hexCode[0] != '#')
            {
                return false;
            }

            return int.TryParse(hexCode.Trim('#'), System.Globalization.NumberStyles.HexNumber, null, out int hexValue);
        }
    }

    public class MiscWindowsMessageButtons
    {
        public string Misc1 { get; set; } = "";
        public string Misc2 { get; set; } = "";
        public string Misc3 { get; set; } = "";
    }

    public class ControlContentRendered
    {
        public void ContentRendered()
        {
            if (OnContentRendered != null)
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        OnContentRendered();
                    });
                }
                else
                {
                    OnContentRendered();
                }
            }
        }

        public Action OnContentRendered { get; set; }
    }

    public class DialogData
    {
        // These if present are used to determine what window this dialog should open under if model.
        public Window ParentWindow { get; set; } 
        public ViewModelBase ParentViewModelBase { get; set; }
        // ------------------------

        public string WindowTitle { get; set; } = "";
        public Brush Background { get; set; } = ColorSets.Background;
        public bool Topmost { get; set; } = true;
        public WindowStyle DialogWindowStyle { get; set; } = WindowStyle.ToolWindow;
        public WindowStartupLocation DialogStartupLocation { get; set; } = WindowStartupLocation.CenterOwner;
        public string WindowIconURI { get; set; } = "";

        public bool RequireResult { get; set; } = false;
        public bool CancelAsync { get; set; } = false;

        public Brush ContentHeaderColor { get; set; } = ColorSets.ContentHeaderColor;
        public string ContentHeader { get; set; } = "";

        public Brush ContentBodyColor { get; set; } = ColorSets.ContentBodyColor;
        public string ContentBody { get; set; } = "";

        public Brush HyperLinkColor { get; set; } = ColorSets.HyperLinkColor;
        public Brush HyperLinkMouseOverColor { get; set; } = ColorSets.HyperLinkMouseOverColor;
        public Brush HyperLinkMouseDisabledColor { get; set; } = ColorSets.HyperLinkMouseDisabledColor;
        public string HyperLinkText { get; set; } = "";
        public string HyperLinkUri { get; set; } = "";

        public WindowMessageIcon MessageIcon { get; set; } = WindowMessageIcon.Information;
        public WindowMessageButtons MessageButtons { get; set; } = WindowMessageButtons.Ok;
        public MiscWindowsMessageButtons MiscButtoms { get; set; } = new MiscWindowsMessageButtons();

        // Pass Args to Dialog
        public object Parameter1 { get; set; }
        public object Parameter2 { get; set; }
    }

    public static class DialogService
    {
        // Takes a ViewModel based user control and opens a window with that control as content. Provides the ability to set Application object ShutdownMode.
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewmodel, Window parentWindow, ShutdownMode shutdownMode)
        {
            WindowMessageResult result = WindowMessageResult.Undefined;

            // If this returns in this manner, you did something wrong and used this libary incorrectly.
            if (Application.Current == null)
            {
                return result;
            }

            try
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    result = getDialogResult(viewmodel, parentWindow, shutdownMode);
                });
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error getting dialog result.", Ex);
            }

            return result;
        }

        private static WindowMessageResult getDialogResult(DialogBaseWindowViewModel viewmodel, Window parentWindow, ShutdownMode shutdownMode)
        {
            DialogBaseWindow dialogBaseWindow = new DialogBaseWindow(viewmodel.dialogData);
            dialogBaseWindow.DataContext = viewmodel;

            // Param shutdownMode can used to prevent the Application.Current from going null. It can be turned off by setting ApplicationExplicitShutdown.
            Application.Current.ShutdownMode = shutdownMode;
            if (parentWindow == null)
            {
                try
                {
                    dialogBaseWindow.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
                }
                catch (Exception Ex)
                {
                    LogWriter.PostException("Error getting current window reference from application.", Ex);

                    if (Application.Current.Windows.Count > 0)
                    {
                        dialogBaseWindow.Owner = Application.Current.Windows[0];
                    }
                }
            }
            else
            {
                dialogBaseWindow.Owner = parentWindow;
            }

            try
            {
                dialogBaseWindow.ShowDialog();
            }
            catch (Exception Ex)
            {
                LogWriter.PostException("Error opening dialog window.", Ex);
            }

            return (dialogBaseWindow.DataContext as DialogBaseWindowViewModel).UserDialogResult;
        }

        // Same as above but also provides but with a default Application object ShutdownMode set to ShutdownMode.OnLastWindowClose.
        public static WindowMessageResult OpenDialog(DialogBaseWindowViewModel viewmodel, Window parentWindow)
        {
            return OpenDialog(viewmodel, parentWindow, ShutdownMode.OnLastWindowClose);
        }

        // Opens Window Message based on DialogData and sets the owner of that window to the passed in paramater.
        public static WindowMessageResult OpenWindowMessage(DialogData data, Window parentWindow)
        {
            DialogBaseWindowViewModel viewmodel = new WindowsMessageViewModel(data);
            return OpenDialog(viewmodel, parentWindow, ShutdownMode.OnLastWindowClose);
        }

        // Opens Window Message based on DialogData and sets the owner of that window to the passed in paramater.
        public static WindowMessageResult OpenWindowMessage(DialogData data, Window parentWindow, ShutdownMode shutdownMode)
        {
            DialogBaseWindowViewModel viewmodel = new WindowsMessageViewModel(data);
            return OpenDialog(viewmodel, parentWindow, shutdownMode);
        }

        // Opens Window Message based on DialogData and sets the owner of that window to the passed in paramater.
        public static WindowMessageResult OpenWindowMessage(DialogData data, ViewModelBase viewModelBase)
        {
            DialogBaseWindowViewModel viewmodel = new WindowsMessageViewModel(data);
            return OpenDialog(viewmodel, FactoryService.GetWindowReference(viewModelBase), ShutdownMode.OnLastWindowClose);
        }

        // Opens Window Message based on DialogData and sets the owner of that window to the passed in paramater.
        public static WindowMessageResult OpenWindowMessage(DialogData data, ViewModelBase viewModelBase, ShutdownMode shutdownMode)
        {
            DialogBaseWindowViewModel viewmodel = new WindowsMessageViewModel(data);
            return OpenDialog(viewmodel, FactoryService.GetWindowReference(viewModelBase), shutdownMode);
        }
    }
}
