using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

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

    public enum WindowButtonFocus
    {
        Left,
        Center,
        Right,
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
                LogMVVM.Exception("MVVM Exception: HexConverter failed toparse color from hex code: " +  hexCode + ".", Ex);
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

    public class DialogData
    {
        // These if present are used to determine what window this dialog should open under if model.
        public Window ParentWindow { get; set; } = null;
        public ViewModelBase ParentViewModelBase { get; set; } = null;
        // ------------------------

        public string WindowTitle { get; set; } = "";
        public Brush Background { get; set; } = ColorSets.Background;
        public bool Topmost { get; set; } = true;
        public WindowStyle DialogWindowStyle { get; set; } = WindowStyle.ToolWindow;
        public WindowStartupLocation DialogStartupLocation { get; set; } = WindowStartupLocation.CenterOwner;
        public string WindowIconURI { get; set; } = "";

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
        public WindowButtonFocus MessageButtonFocus { get; set; } = WindowButtonFocus.Center;
        public MiscWindowsMessageButtons MiscButtoms { get; set; } = new MiscWindowsMessageButtons();

        // Pass Args to Dialog
        public object Parameter1 { get; set; } = null;
        public object Parameter2 { get; set; } = null;

        // Control to Focus on launch
        public UIElement FocusUIElement { get; set; } = null;

        // Controls if the DialogBaseWindow will wait for the WPF render thread to "catch up"
        public bool RenderWait { get; set; } = false;
    }

    public class DialogEvents
    {
        public event EventHandler CloseDialogHandler;
        public void CloseDialog()
        {
            OnCloseDialog(EventArgs.Empty);
        }
        protected virtual void OnCloseDialog(EventArgs e)
        {
            if (CloseDialogHandler != null)
            {
                CloseDialogHandler(this, e);
            }
        }

        public event EventHandler<MessageEventArgs> SendMessageHandler;
        public WindowMessageResult SendMessage(DialogData dialogData)
        {
            if (dialogData != null)
            {
                return OnSendMessage(new MessageEventArgs() { MessageData = dialogData, });
            }

            return WindowMessageResult.Undefined;
        }
        protected virtual WindowMessageResult OnSendMessage(MessageEventArgs e)
        {
            if (SendMessageHandler != null)
            {
                SendMessageHandler(this, e);
            }

            return e.MessageResult;
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public DialogData MessageData { get; set; } = null;
        public WindowMessageResult MessageResult { get; set; } = WindowMessageResult.Undefined;
    }

    public class DialogUserControlView
    {
        public DialogBaseWindow BaseDialogWindowView { get; private set; } = null;
        public UserControl UserControlView { get; private set; } = null;
        public DialogViewModel BaseDialogWindowViewModel { get; private set; } = null;

        public bool IsValid { get; private set; } = false;

        public DialogUserControlView(DialogViewModel dialogViewModel, UserControl userControl)
        {
            if (userControl == null || dialogViewModel == null)
            {
                return;
            }

            if (userControl.DataContext != dialogViewModel)
            {
                userControl.DataContext = dialogViewModel;
            }

            BaseDialogWindowView = new DialogBaseWindow(userControl, dialogViewModel);
            UserControlView = userControl;
            BaseDialogWindowViewModel = dialogViewModel;

            IsValid = true;
        }
    }

    public class DialogWindowView
    {
        public Window DialogWindow { get; private set; } = null;
        public DialogViewModel WindowViewModel { get; private set; } = null;

        public bool IsValid { get; private set; } = false;

        public DialogWindowView(DialogViewModel dialogViewModel, Window window)
        {
            if (window == null || window.DataContext != dialogViewModel)
            {
                return;
            }

            DialogWindow = window;
            WindowViewModel = dialogViewModel;

            IsValid = true;
        }
    }
}
