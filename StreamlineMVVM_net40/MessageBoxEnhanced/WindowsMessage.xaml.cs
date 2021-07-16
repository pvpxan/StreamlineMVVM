using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    /// <summary>
    /// Interaction logic for WindowsMessage.xaml
    /// </summary>
    public partial class WindowsMessage : UserControl
    {
        public WindowsMessage(WindowsMessageViewModel windowsMessageViewModel)
        {
            try
            {
                this.DataContext = windowsMessageViewModel;
                InitializeComponent();

                switch (windowsMessageViewModel.Data.MessageButtons)
                {
                    case WindowMessageButtons.AcceptDecline:
                    case WindowMessageButtons.ContinueCancel:
                    case WindowMessageButtons.OkCancel:
                    case WindowMessageButtons.YesNo:
                        if (windowsMessageViewModel.Data.MessageButtonFocus == WindowButtonFocus.Center)
                        {
                            windowsMessageViewModel.Data.MessageButtonFocus = WindowButtonFocus.Left;
                        }
                        break;

                    case WindowMessageButtons.Default:
                    case WindowMessageButtons.Exit:
                    case WindowMessageButtons.Ok:
                        if (windowsMessageViewModel.Data.MessageButtonFocus == WindowButtonFocus.Left ||
                            windowsMessageViewModel.Data.MessageButtonFocus == WindowButtonFocus.Right)
                        {
                            windowsMessageViewModel.Data.MessageButtonFocus = WindowButtonFocus.Center;
                        }
                        break;

                    case WindowMessageButtons.YesNoCancel:
                        // TODO(DB): Determine if there needs to be logic here.
                        break;

                    case WindowMessageButtons.Misc:
                        // TODO(DB): Determine if there needs to be logic here.
                        break;

                    default:
                        // TODO(DB): Determine if there needs to be logic here.
                        break;
                }

                switch (windowsMessageViewModel.Data.MessageButtonFocus)
                {
                    case WindowButtonFocus.Left:
                        windowsMessageViewModel.Data.FocusUIElement = leftButton;
                        break;

                    case WindowButtonFocus.Center:
                        windowsMessageViewModel.Data.FocusUIElement = centerButton;
                        break;

                    case WindowButtonFocus.Right:
                        windowsMessageViewModel.Data.FocusUIElement = rightButton;
                        break;
                }
            }
            catch
            {
                // TODO (DB): Create some sort of error log in a default windows directory.
            }
        }

        private void hyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            using (Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)))
            {
                e.Handled = true;
            }
        }
    }
}
