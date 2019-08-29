using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace StreamlineMVVM
{
    public static partial class MessageBoxEnhanced
    {
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
                data.Background = Brushes.White;
                data.ContentHeaderColor = Brushes.DarkBlue;
                data.ContentBodyColor = Brushes.Black;
                data.HyperLinkColor = Brushes.Blue;
                data.HyperLinkMouseOverColor = Brushes.Red;
                data.HyperLinkMouseDisabledColor = Brushes.Gray;
            }
            else
            {
                if (windowMessageColorSet.Background == null)
                {
                    data.Background = Brushes.White;
                }

                if (windowMessageColorSet.ContentHeaderColor == null)
                {
                    data.ContentHeaderColor = Brushes.DarkBlue;
                }

                if (windowMessageColorSet.ContentBodyColor == null)
                {
                    data.ContentBodyColor = Brushes.Black;
                }

                if (windowMessageColorSet.HyperLinkColor == null)
                {
                    data.HyperLinkColor = Brushes.Blue;
                }

                if (windowMessageColorSet.HyperLinkMouseOverColor == null)
                {
                    data.HyperLinkMouseOverColor = Brushes.Red;
                }

                if (windowMessageColorSet.HyperLinkMouseDisabledColor == null)
                {
                    data.HyperLinkMouseDisabledColor = Brushes.Gray;
                }
            }

            return data;
        }

        // Base set of overload methdods.
        // Sets Window Title and Message Content.
        public static WindowMessageResult Show(string windowTitle, string contentHeader)
        {
            return DialogService.OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader), null as Window);
        }

        // Sets Window Title, Message Content, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return DialogService.OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, "", "", "", windowMessageButtons, windowMessageIcon), null as Window);
        }

        // Sets Window Title, Message Header, Message Body, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return DialogService.OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, "", "", windowMessageButtons, windowMessageIcon), null as Window);
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, string hyperLink, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return DialogService.OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, "", hyperLink, windowMessageButtons, windowMessageIcon), null as Window);
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, and Message Icon.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon)
        {
            return DialogService.OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon), null as Window);
        }

        // Sets Window Title, Message Header, Message Body, Message HyperLink Text, Message HyperLink Uri, Message Buttons, Message Icon, and Color Set.
        public static WindowMessageResult Show(string windowTitle, string contentHeader, string contentBody, string hyperLinkText, string hyperLinkUri, WindowMessageButtons windowMessageButtons, WindowMessageIcon windowMessageIcon, WindowMessageColorSet windowMessageColorSet)
        {
            return DialogService.OpenWindowMessage(dialogDataBuilder(windowTitle, contentHeader, contentBody, hyperLinkText, hyperLinkUri, windowMessageButtons, windowMessageIcon), null as Window);
        }
    }
}
