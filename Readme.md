# IEShellEx

A series of plugin shell extensions for the Windows shell to expose the content of file-types used in the Infinity Engine. For more details view the application (readme)[https://github.com/btigi/ieshellex/src/documentation/Readme.html]

## Usage

__Prerequisites__

- .NET Framework 4.7+

- Powershell

Shell extensions must be registered with the Windows shell. IEShellEx this can be done via the use of ServerRegistrationManager.exe, e.g. 
```
ServerRegistrationManager.exe install IE2DAInfotip.dll -codebase
```

Note: Due to interactions with the Windows shell, installation must be run with administrator privileges.

Note: The Windows Shell may need restarting to pick up changes.



## Download

You can [download](https://github.com/btigi/ieshellex/releases/) the latest version of ieshell.


## Technologies

IEShellEx is written in C# .NET Framework 4.7.2, and uses [Sharpshell](https://github.com/dwmkerr/sharpshell) to facilitate communication with the Windows shell.


## Compiling

To clone and run this application, you'll need [Git](https://git-scm.com) and [.NET](https://dotnet.microsoft.com/) installed on your computer.

To build the installer you will need NSIS(https://nsis.sourceforge.io/Main_Page) installed on your computer and in the PATH - you can then run src\installer\go.bat to generate the installer executable.


## Notes

The guidance from Microsoft on using managed code (e.g. .net) to shell extensions is mixed - you can view a summary of statements in the [Sharpshell](https://github.com/dwmkerr/sharpshell/blob/master/docs/managed-shell-extensions.md) documentation.


## License

IEShellEx is licensed under [CC BY-NC 4.0](https://creativecommons.org/licenses/by-nc/4.0/)

The ZLIB code contained in IEMOSPreview is licenced under The Code Project Open Licence (CPOL) 1.02 (https://www.codeproject.com/info/cpol10.aspx)

The application icon created by [Exhumed](https://iconarchive.com/show/mega-games-pack-25-icons-by-3xhumed/Baldur-s-Gate-1-icon.html) is licenced under [CC BY-NC-ND 4.0](https://creativecommons.org/licenses/by-nc-nd/4.0/)