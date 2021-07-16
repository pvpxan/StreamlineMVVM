using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    public class WindowMessageColorSet
    {
        public Brush Background { get; set; } = Brushes.White;

        public Brush ContentHeaderColor { get; set; } = Brushes.DarkBlue;
        public Brush ContentBodyColor { get; set; } = Brushes.Black;

        public Brush HyperLinkColor { get; set; } = Brushes.Blue;
        public Brush HyperLinkMouseOverColor { get; set; } = Brushes.Red;
        public Brush HyperLinkMouseDisabledColor { get; set; } = Brushes.Gray;
    }

    public static partial class MessageBoxEnhanced
    {
        public static WindowMessageColorSet ColorSet { get; set; } = new WindowMessageColorSet(); // Plans for the future.

        // Takes the arguments from the overload methods to create a DialogData object. This is what is passed to the DialogService and used to open the MessageBox Enhanced window.
        private static DialogData dialogDataBuilder(
            string windowTitle = "",
            string contentHeader = "",
            string contentBody = "",
            string hyperLinkText = "",
            string hyperLinkUri = "",
            WindowMessageButtons windowMessageButtons = WindowMessageButtons.Ok,
            WindowMessageIcon windowMessageIcon = WindowMessageIcon.Information,
            WindowMessageColorSet windowMessageColorSet = null)
        {
            DialogData data = new DialogData()
            {
                WindowTitle = windowTitle,
                ContentBody = contentBody,
                ContentHeader = contentHeader,
                HyperLinkText = hyperLinkText,
                HyperLinkUri = hyperLinkUri,
                MessageButtons = windowMessageButtons,
                MessageIcon = windowMessageIcon,
            };

            if (windowMessageColorSet == null)
            {
                data.Background = ColorSets.Background;
                data.ContentHeaderColor = ColorSets.ContentHeaderColor;
                data.ContentBodyColor = ColorSets.ContentBodyColor;
                data.HyperLinkColor = ColorSets.HyperLinkColor;
                data.HyperLinkMouseOverColor = ColorSets.HyperLinkMouseOverColor;
                data.HyperLinkMouseDisabledColor = ColorSets.HyperLinkMouseDisabledColor;
            }
            else
            {
                if (windowMessageColorSet.Background == null)
                {
                    data.Background = ColorSets.Background;
                }

                if (windowMessageColorSet.ContentHeaderColor == null)
                {
                    data.ContentHeaderColor = ColorSets.ContentHeaderColor;
                }

                if (windowMessageColorSet.ContentBodyColor == null)
                {
                    data.ContentBodyColor = ColorSets.ContentBodyColor;
                }

                if (windowMessageColorSet.HyperLinkColor == null)
                {
                    data.HyperLinkColor = ColorSets.HyperLinkColor;
                }

                if (windowMessageColorSet.HyperLinkMouseOverColor == null)
                {
                    data.HyperLinkMouseOverColor = ColorSets.HyperLinkMouseOverColor;
                }

                if (windowMessageColorSet.HyperLinkMouseDisabledColor == null)
                {
                    data.HyperLinkMouseDisabledColor = ColorSets.HyperLinkMouseDisabledColor;
                }
            }

            return data;
        }

        private static DialogUserControlView generateDialogViews(DialogData dialogData)
        {
            dialogData.RenderWait = true; // For these to work, we always to want to set this.
            WindowsMessageViewModel windowsMessageViewModel = new WindowsMessageViewModel(dialogData);
            return new DialogUserControlView(windowsMessageViewModel, new WindowsMessage(windowsMessageViewModel));
        }

        // Opens Window Message based on DialogData and sets owner of that window to the passed in paramater and sets shutdownmode.
        public static WindowMessageResult OpenWindowMessage(DialogData data, Window parentWindow, ShutdownMode shutdownMode)
        {
            return DialogService.OpenDialog(generateDialogViews(data), parentWindow, shutdownMode);
        }

        // Opens Window Message based on DialogData and sets the owner of that window to the passed in paramater.
        public static WindowMessageResult OpenWindowMessage(DialogData data, Window parentWindow)
        {
            return DialogService.OpenDialog(generateDialogViews(data), parentWindow);
        }

        // Opens Window Message based on DialogData and sets the shutdownmode.
        public static WindowMessageResult OpenWindowMessage(DialogData data, ShutdownMode shutdownMode)
        {
            return DialogService.OpenDialog(generateDialogViews(data), shutdownMode);
        }

        // Opens Window Message based on DialogData and no other parameters.
        public static WindowMessageResult OpenWindowMessage(DialogData data)
        {
            return DialogService.OpenDialog(generateDialogViews(data));
        }

        // Opens Window Message based on DialogData and sets the owner of that window to the passed in paramater. Application Objects ShutdownMode set to the current ShutdownMode.
        public static WindowMessageResult OpenWindowMessage(DialogData data, ViewModelBase viewModelBase)
        {
            return DialogService.OpenDialog(generateDialogViews(data), FactoryService.GetWindowReference(viewModelBase));
        }

        // Opens Window Message based on DialogData and sets the owner of that window to the passed in paramater.
        public static WindowMessageResult OpenWindowMessage(DialogData data, ViewModelBase viewModelBase, ShutdownMode shutdownMode)
        {
            return DialogService.OpenDialog(generateDialogViews(data), FactoryService.GetWindowReference(viewModelBase), shutdownMode);
        }
        // ---------------------------------------------------------
    }
}
