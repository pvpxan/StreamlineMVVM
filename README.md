# StreamlineMVVM
* MVVM Framework comes with:
  * Enhanced Windows MessageBox
  * Logging Utility
  * Config File Handling
  * INI File Handling
  * Minor System.IO wrapping
  * Specialized Regex Match/Replace
* Also can add custom dialogs by creating a User Control and using that as the DataTemplate for window content.

# Injection
* Add Reference: StreamlineMVVM.dll
* Add Application Resource:
  * `StreamlineMVVM.dll` is the output file.
  * `<ResourceDictionary Source="pack://application:,,,/StreamlineMVVM;component/Templates/MergedResources.xaml"/>`
* Supports Embedding with this code: https://github.com/pvpxan/DLLEmbedding

# Framework
See https://github.com/pvpxan/MVVMTemplate for example code of how to use this framework.
* Creation of a ViewModel should be done by extending your class with `ViewModelBase`.
* The `RelayCommand` Class is used for tieing your business logic to a bound `ICommand`.

# MessageBoxEnhanced
Customizable replacement for the standard C# MessageBox with more options for buttons and dialog results.
NOTE: 
* Using this on application launch BEFORE you open another window will result in the Application class shutting down.
* Set Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown; to get around this.
* Downside is you have to call an explicit shutdown or change the setting back.

# Classes
The Logging classes are simple thread safe log writers with multiple options.
* `LogWriter`
  * `bool SetPath(string path, string user, string application)` (Needed to assign where you want the log files to go.)
  * `Exception(string log, Exception ex)`
  * `LogEntry(string log)`
* `LogWriterWPF`
  * `LogDisplay(string log, MessageBoxImage messageType)`
  * `LogDisplay(string log, MessageBoxImage messageBoxImage, Window window)`
  * `ExceptionDisplay(string log, Exception ex, bool showFull)`
  * `ExceptionDisplay(string log, Exception ex, bool showFull, Window window)`

The following classes to interface with INI files and `app.config` files in a thread safe manner.
* `Config`
  * `string Read(string key)` Used for reading `app.config` file.
  * `bool Update(string key, string value)` Writes to `app.config` file.
* `INI`
  * `bool? ReadBool(string file, string key)`
  * `int? ReadInt(string file, string key)`
  * `string Read(string file, string key)`
  * `bool Write(string file, string key, string value, bool create, bool backup)`

Below are some utility classes used to wrap various methods capaable of throwing exceptions.
* `SystemIO`
  * `PathType GetPathType(string path)`
  * `bool Delete(string file)`
  * `bool Copy(string fileSource, string fileTarget, bool overwrite)`
  * `bool CreateDirectory(string directory)`
  * `OutputResult[] CopyDirectory(string sourceDirectory, string targetDirectory)`
* `RegexFunctions`
  * Lots of matching and replacing based on number, special characters, and spacing.
  
  