using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace StreamlineMVVM
{
    public class DataMessaging
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(DialogData data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher == null)
                {
                    OnDataTransmittedEvent(data);
                    return;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            finally
            {

            }
        }

        public Action<DialogData> OnDataTransmittedEvent { get; set; }
    }

    public class TextAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(string data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher == null)
                {
                    OnDataTransmittedEvent(data);
                    return;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            finally
            {

            }
        }

        public Action<string> OnDataTransmittedEvent { get; set; }
    }

    public class IntAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(int data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher == null)
                {
                    OnDataTransmittedEvent(data);
                    return;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            finally
            {

            }
        }

        public Action<int> OnDataTransmittedEvent { get; set; }
    }

    public class BoolAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(bool data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher == null)
                {
                    OnDataTransmittedEvent(data);
                    return;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            finally
            {

            }
        }

        public Action<bool> OnDataTransmittedEvent { get; set; }
    }

    public class IInputElementAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(IInputElement data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher == null)
                {
                    OnDataTransmittedEvent(data);
                    return;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            finally
            {

            }
        }

        public Action<IInputElement> OnDataTransmittedEvent { get; set; }
    }

    public class UIElementAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(UIElement data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher == null)
                {
                    OnDataTransmittedEvent(data);
                    return;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            finally
            {

            }
        }

        public Action<UIElement> OnDataTransmittedEvent { get; set; }
    }

    public class ObjectAggregator
    {
        public Window DispatcherWindow { get; set; } = null;

        public void Transmit(object data)
        {
            if (OnDataTransmittedEvent == null)
            {
                return;
            }

            Dispatcher dispatcher = null;
            try
            {
                if (DispatcherWindow != null)
                {
                    dispatcher = DispatcherWindow.Dispatcher;
                }
                else
                {
                    dispatcher = Application.Current.Dispatcher;
                }

                if (dispatcher == null)
                {
                    OnDataTransmittedEvent(data);
                    return;
                }

                if (dispatcher.CheckAccess())
                {
                    DispatcherWindow.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
            finally
            {

            }
        }

        public Action<object> OnDataTransmittedEvent { get; set; }
    }
}
