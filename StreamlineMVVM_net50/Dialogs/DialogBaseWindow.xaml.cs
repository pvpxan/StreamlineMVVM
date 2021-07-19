using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    /// <summary>
    /// Interaction logic for DialogBaseWindow.xaml
    /// </summary>
    public partial class DialogBaseWindow : Window
    {
        private DialogViewModel pDialogViewModel = null;

        public DialogBaseWindow(UserControl userControl, DialogViewModel dialogViewModel)
        {
            try
            {
                pDialogViewModel = dialogViewModel;
                this.DataContext = dialogViewModel;

                InitializeComponent();

                try
                {
                    // These are some conditions that are needed for everything to work correctly.
                    if (userControl == null || userControl.DataContext != dialogViewModel)
                    {
                        // NOTE: This will cause the following suppressed error that only shows in debugger output:
                        // 'System.InvalidOperationException' in PresentationFramework.dll
                        // This happens since the window is already closed before a Show() or ShowDialog() method can actually do anything.
                        this.Close();
                        return;
                    }
                }
                catch (Exception Ex)
                {
                    LogMVVM.Exception("MVVM Exception: Dialog Window user control and data context check error.", Ex);
                }

                try
                {
                    if (dialogViewModel.Data.WindowIconURI.Length > 0)
                    {
                        Icon = BitmapFrame.Create(new Uri(dialogViewModel.Data.WindowIconURI, UriKind.RelativeOrAbsolute));
                    }
                }
                catch (Exception Ex)
                {
                    LogMVVM.Exception("MVVM Exception: Dialog Window title bar icon creation error.", Ex);
                }

                // General Window properties. Not bound since some are not technically content and one is not able to be bound.
                WindowStartupLocation = dialogViewModel.Data.DialogStartupLocation; // Cannot be bound since it is a DependencyProperty
                WindowStyle = dialogViewModel.Data.DialogWindowStyle;
                Topmost = dialogViewModel.Data.Topmost;
                Title = dialogViewModel.Data.WindowTitle;
                Background = dialogViewModel.Data.Background;

                // Adds the user control to the window.
                this.dialogBaseWindowGrid.Children.Add(userControl);
                
                // If there is a specified control to focus, this will process that.
                FocusElementControl(dialogViewModel.Data.FocusUIElement);

                Loaded += contentLoaded;
                ContentRendered += contentRendered;
                Closing += windowClosing;
                dialogViewModel.Events.CloseDialogHandler += closeDialog;
                dialogViewModel.Events.SendMessageHandler += openWindowMessage;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, "Window load error: " + Environment.NewLine + Convert.ToString(Ex), "Error...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void contentLoaded(object sender, RoutedEventArgs e)
        {
            pDialogViewModel.ContentLoaded(sender, e);
        }

        private void contentRendered(object sender, EventArgs e)
        {
            pDialogViewModel.ContentRendered(sender, e);

            if (pDialogViewModel.Data.RenderWait)
            {
                try
                {
                    Dispatcher dispatcher = this.Dispatcher;
                    if (dispatcher.CheckAccess())
                    {
                        dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
                    }
                }
                catch (Exception Ex)
                {
                    LogMVVM.Exception("MVVM Exception: Dialog Window cotent render error. Possible dispatcher error.", Ex);
                }
            }
        }

        private void windowClosing(object sender, CancelEventArgs e)
        {
            pDialogViewModel.WindowClosing(sender, e);
            DialogService.WindowClosingCleanUp();
        }

        public void FocusElementControl(UIElement uiElement)
        {
            if (uiElement == null)
            {
                return;
            }

            KeyboardHelper.Focus(uiElement, this.Dispatcher, DispatcherPriority.Render);
        }

        private void openWindowMessage(object sender, MessageEventArgs e)
        {
            e.MessageResult = MessageBoxEnhanced.OpenWindowMessage(e.MessageData, this);
        }

        private void closeDialog(object sender, EventArgs e)
        {
            DialogService.CloseDialog(this);
        }
    }
}
