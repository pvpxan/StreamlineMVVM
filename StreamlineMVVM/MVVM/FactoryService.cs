using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace StreamlineMVVM
{
    public static class FactoryService
    {
        private class FactoryReference
        {
            public ViewModelBase ViewModelBaseReference { get; set; } = null;
            public Window WindowReference { get; set; } = null;
        }

        private static List<FactoryReference> factoryList = new List<FactoryReference>();

        public static void Register(ViewModelBase viewModelBase, Window window)
        {
            factoryList.Add(new FactoryReference { ViewModelBaseReference = viewModelBase, WindowReference = window });
        }

        public static void DeRegister(ViewModelBase viewModelBase)
        {
            try
            {
                int index = factoryList.FindIndex(f => f.ViewModelBaseReference == viewModelBase);
                factoryList.RemoveAt(index);
            }
            catch
            {
                // Nothing for now.
            }
        }

        public static Window GetWindowReference(ViewModelBase viewModelBase)
        {
            if (viewModelBase == null)
            {
                return null;
            }

            try
            {
                Window window = factoryList.FirstOrDefault(w => w.ViewModelBaseReference == viewModelBase).WindowReference;
                return window;
            }
            catch
            {
                return null;
            }
        }
    }
}
