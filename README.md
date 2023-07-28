# KekUploadServerGUI - A plugin bringing a GUI to [KekUploadServer](https://github.com/GamePowerX/KekUploadServer)
## How to use
### 1. Build or (in the future) download the plugin
Clone the repository and build the plugin with Visual Studio or the dotnet CLI.
### 2. Add the plugin to your KekUploadServer
Add the plugin to your KekUploadServer by putting the plugin dll into the `plugins` folder.
You also need to add all the assemblies that the plugin depends on to `plugins` folder, they can all be found in the build output of the plugin. (/bin/Debug/net7.0/ or /bin/Release/net7.0/) <br>
For KekUploadServerGUI, you need to add the following assemblies:
- `KekUploadServerGUI.dll`
- `Avalonia.Base.dll`
- `Avalonia.Controls.dll`
- `Avalonia.DesignerSupport.dll`
- `Avalonia.Desktop.dll`
- `Avalonia.Dialogs.dll`
- `Avalonia.dll`
- `Avalonia.Fonts.Inter.dll`
- `Avalonia.FreeDesktop.dll`
- `Avalonia.Markup.dll`
- `Avalonia.Markup.Xaml.dll`
- `Avalonia.Metal.dll`
- `Avalonia.MicroCom.dll`
- `Avalonia.Native.dll`
- `Avalonia.OpenGL.dll`
- `Avalonia.Remote.Protocol.dll`
- `Avalonia.Skia.dll`
- `Avalonia.Themes.Fluent.dll`
- `Avalonia.Win32.dll`
- `Avalonia.X11.dll`
- `HarfBuzzSharp.dll`
- `MicroCom.Runtime.dll`
- `SkiaSharp.dll`
- `System.Drawing.Common.dll`
- `System.IO.Pipelines.dll`
- `Tmds.DBus.Protocol.dll`
And also get the native libraries for your platform from the runtimes folder in the plugin's build output.
### 3. Start the server
Start the server and you should see the GUI window pop up.

## Contributing
You can contribute to this project by creating a pull request or by creating an issue.

## License
This project is licensed under the MIT license. See the [LICENSE](https://github.com/GamePowerX/KekUploadServerGUI/blob/master/LICENSE) file for more information.