using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace StreamlineMVVM
{
    public static partial class DialogService
    {
        public static void WindowClosingCleanUp()
        {
            // HACK: Cleans up bitmapimage of icon.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // Ugly, but necessary.
        public static void CloseDialog(Window window)
        {
            if (window == null)
            {
                return;
            }
            
            try
            {
                window.Dispatcher.Invoke((Action)delegate
                {
                    // HACK: There might be occations and I do not know how, but the window no longer is considered modal or what not.
                    bool isThisModal = false;
                    try
                    {
                        isThisModal = (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window);
                    }
                    catch
                    {
                        // TODO (DB): Think about logging this in the future.
                    }

                    if (isThisModal)
                    {
                        try
                        {
                            window.DialogResult = true;
                        }
                        catch
                        {
                            // TODO (DB): Think about logging this in the future.
                            window.Close();
                        }
                    }
                    else
                    {
                        try
                        {
                            window.DialogResult = (window.DialogResult == true) || (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window);
                            return;
                        }
                        catch
                        {
                            // TODO (DB): Think about logging this in the future.
                            window.Close();
                        }
                    }
                });
            }
            catch
            {
                // TODO (DB): Think about logging this in the future.
                window.Close();
            }
        }
    }
}
