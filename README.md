# StreamlineMVVM
* MVVM Framework comes with:
  * Enhanced Windows MessageBox
  * Logging Utility
  * Config File Handling
  * INI File Handling
  * Minor System.IO wrapping
  * Specialized Regex Match/Replace
  * Tools to help create robust Window Factory classes.
* Also can add custom dialogs by creating a User Control and using that as the DataTemplate for window content.
* Two versions available for compatibility with 2 different .net Framework distributions.
  * .net Framework 4.0
  * .net 5.0

# Injection
* Add Reference: StreamlineMVVM.dll
* Add Application Resource:
  * `<ResourceDictionary Source="pack://application:,,,/StreamlineMVVM;component/Templates/MergedResources.xaml"/>`
  * Add to XAML where resources are used:`xmlns:ext="clr-namespace:MVVMFramework;assembly=MVVMFramework"`
* Supports Embedding with this code: https://github.com/pvpxan/DLLEmbedding

# Framework
* See https://github.com/pvpxan/MVVMTemplate for example code of how to use this framework.
* Creation of a ViewModel should be done by extending your class with `ViewModelBase`.
* The `RelayCommand` Class is used for tieing your business logic to a bound `ICommand`.

# MessageBoxEnhanced
* Customizable replacement for the standard C# MessageBox with more options for buttons and dialog results.
* __NOTE:__ 
  * Using this on application launch BEFORE you open another window will result in the Application class shutting down.
  * Set `Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;` to get around this.
  * Downside is you have to call an explicit shutdown or change the setting back.

# Classes
* `DialogService` - Methods ingests User Control and ViewModel based off `DialogViewModel` to create dialog windows.
  * `WindowMessageResult OpenDialog(DialogUserControlView dialogUserControlView, Window parentWindow, ShutdownMode shutdownMode)`
  * `WindowMessageResult OpenDialog(DialogWindowView dialogWindowView, Window parentWindow, ShutdownMode shutdownMode)`
  * The above methods have multiple overloads.
* `DialogData`
  * Pretty new addition with tools to help create robust Window Factory classes.
* `DialogViewModel`
  * ViewModel base class focused around dialogs. Requires `DialogData` in the base parameter.
* `DialogUserControlView` and `DialogWindowView`
  * Used as part of dialog creation. Makes sure some necessary parameters are valid.
  
# Extention Methods
`ComboBoxHighlight`\
`CornerRadius`\
`FocusThickness`\
`MouseOverBackground`\
`MouseOverBorder`\
`MouseDownBackground`\
`MouseDownBorder`\
`SelectionActiveBackground`\
`SelectionActiveBorder`\
`SelectionInactiveBackground`\
`SelectionInactiveBorder`\
`CommandParameter`\
`Command`

# Styles/Templates
* Some of these are a semi work in progress, but all work well when you know how to use them.
`pack://application:,,,/StreamlineMVVM;component/Templates/ButtonExtended.xaml`\
`pack://application:,,,/StreamlineMVVM;component/Templates/CheckBoxExtended.xaml`\
`pack://application:,,,/StreamlineMVVM;component/Templates/FlatComboBox.xaml`\
`pack://application:,,,/StreamlineMVVM;component/Templates/GroupBoxImproved.xaml`\
`pack://application:,,,/StreamlineMVVM;component/Templates/ListBoxExtended.xaml`\
`pack://application:,,,/StreamlineMVVM;component/Templates/ListViewExtended.xaml`\
`pack://application:,,,/StreamlineMVVM;component/Templates/ScrollBarExtended.xaml`
`pack://application:,,,/StreamlineMVVM;component/Templates/TextBoxExtended.xaml`\
`pack://application:,,,/StreamlineMVVM;component/Templates/TreeViewExtended.xaml`\

# DialogBaseWindow
* Generic Window with a bound content presenter.
* Creating custom dialogs requires use of `DialogViewModel` to extend a datacontext class of your own making.
