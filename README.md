# StreamlineMVVM
* MVVM Framework with enhanced windows messagebox, logging utility, config file handling, and INI file handling.
* Also can add custom dialogs by creating a User Control and using that as the DataTemplate for window content.

# Injection
* Add Reference: StreamlineMVVM.dll
* Add Application Resource:
  * `<ResourceDictionary Source="pack://application:,,,/StreamlineMVVM;component/Templates/DataTemplates.xaml"/>`
* Supports Embedding with this code: https://github.com/pvpxan/DLLEmbedding

# Framework
See https://github.com/pvpxan/MVVMTemplate for example code of how to use this framework.
* Creation of a ViewModel should be done by extending your class with `ViewModelBase`.
* The `RelayCommand` Class is used for tieing your business logic to a bound `ICommand`.

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
