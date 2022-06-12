# Electron.NET and ASP.NET core 6

When I tried to `electronize init` to a new Asp.Net Core 6.0 Razor
webapp, I'm told it does not support dotnet core 6.0

Here is what I got when I tried to 'electronize init'

```sh

It was not possible to find any compatible framework version
The framework 'Microsoft.NETCore.App', version '5.0.0' (x64) was not
found.

  - The following frameworks were found:
        6.0.5 at [/usr/local/share/dotnet/shared/Microsoft.NETCore.App]
	
	You can resolve the problem by installing the specified framework and/or SDK.

The specified framework can be found at:
  - https://aka.ms/dotnet-core-applaunch?framework=Microsoft.NETCore.App&framework_version=5.0.0&arch=x64&rid=osx.12-x64
```

Let's see if we can add dotnet 5, then tweak things to work with dotnet 6.

On my Mac, I only have dotnet 6 installed:

```sh
dotnet --list-sdks
6.0.300 [/usr/local/share/dotnet/sdk]
```

Install dotnet 5.


```
% dotnet --list-sdks 
5.0.408 [/usr/local/share/dotnet/sdk]
6.0.300 [/usr/local/share/dotnet/sdk]
```

Check does electronize work:
```
 % electronize version
 ElectronNET.CLI Version: 13.5.1.0
 
```

Let's try with this asp.net core 6 razor webapp template

```
% mkdir Electron.Net
% cd Electron.Net
% dotnet new webapp
% electronize init
Arguments: 

Adding our config file to your project...
Search your .csproj/fsproj to add the needed electron.manifest.json...
Found your .csproj:
/Users/michealcolhoun/Projects/CT-Studio/Electron.Net/Electron.Net.csproj
- check for existing config or update it.
electron.manifest.json will be added to csproj/fsproj.
electron.manifest.json added in csproj/fsproj!
Search your .launchSettings to add our electron debug profile...
Debug profile added!
Everything done - happy electronizing!


```

What did this add? I did a quick `git diff`:
```

<ItemGroup>
    <Content Update="electron.manifest.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
</ItemGroup>
```

This tool also added the blank template for `electron.manifest.json`


```
% electronize start /watch
Start Electron Desktop Application...

```
I get npm warning that my version 4.1.1 is bad.

- versions greater than 3.2.0 and less than 3.2.7 
- versions greater than 4.0.0 and less than 4.3.1 have issues

Need to check vesion number of node and npm

Also, got a syntax error, which is usually a missing package or out of
date package. 

```
async function downloadArtifact(_artifactDetails) {
      ^^^^^^^^
      
      SyntaxError: Unexpected token function
```      

Further down I get these errors:

```
npm WARN ws@7.4.6 requires a peer of bufferutil@^4.0.1 but none was installed.
npm WARN ws@7.4.6 requires a peer of utf-8-validate@^5.0.2 but none was installed.
npm ERR! Darwin 21.4.0
npm ERR! argv "/usr/local/bin/node" "/usr/local/bin/npm" "install"
npm ERR! node v6.11.4
npm ERR! npm  v3.10.10
npm ERR! code ELIFECYCLE

```


Ok, a little sanity check:

* We are running node v6.11.4
* We are running npm v3.10.10

Let's see what the latest LTS version available are today:

* [Latest LTS Version: 16.15.1 (includes npm 8.11.0)](https://nodejs.org/en/download/)

OK. This might be the problem. Let's upgrade and try again.

This package will install:

* Node.js v16.15.1 to /usr/local/bin/node
* npm v8.11.0 to /usr/local/bin/npm

--

Before I do this, I would really like to remove all other versions of
node and npm


```
brew uninstall node
brew uninstall --force node 
rm -f /usr/local/bin/npm /usr/local/lib/dtrace/node.d;
rm -rf ~/.npm;
sudo rm -rf /usr/local/bin/npm /usr/local/share/man/man1/node* /usr/local/lib/dtrace/node.d ~/.npm ~/.node-gyp 

go to 
/usr/local/lib
and delete any node and node_modules

go to 
/usr/local/include
and delete any node and node_modules directory

check your Home directory for any local or lib or include folders, and
delete any node or node_modules from there

go to 
/usr/local/bin
and delete any node executable

You may also need to do:

sudo rm -rf /usr/local/bin/npm /usr/local/share/man/man1/node.1 /usr/local/lib/dtrace/node.d
sudo rm -rf /opt/local/bin/node /opt/local/include/node /opt/local/lib/node_modules

Additionally, NVM modifies the PATH variable in $HOME/.bashrc, which must be reverted manually.

```

OK. I've instaled latest version of Node

```
This package has installed:

* Node.js v16.15.1 to /usr/local/bin/node
* npm v8.11.0 to /usr/local/bin/npm

Make sure that /usr/local/bin is in your $PATH.
```

Check can it find node:

```
% which node
/usr/local/bin/node
```

All Good. Let's run again:

```
% electronize start 
Start Electron Desktop Application...

... 

bash: ./electron: Permission denied

```

Fix this by removing the obj and bin folders and starting again


so `electronize start` is working. Now let's build


```
% electronize build

  • electron-builder  version=23.0.3 os=21.4.0
    • loaded configuration file=/Users/** path ***/Electron.Net/obj/desktop/osx/bin/electron-builder.json
  • description is missed in the package.json appPackageFile=/Users/** path ***/Electron.Net/obj/desktop/osx/package.json
  • packaging       platform=darwin arch=x64 electron=13.1.5 appOutDir=/Users/** path ***/Electron.Net/bin/Desktop/mac
  • downloading url=https://github.com/electron/electron/releases/download/v13.1.5/electron-v13.1.5-darwin-x64.zip size=79 MB parts=8
  • downloaded url=https://github.com/electron/electron/releases/download/v13.1.5/electron-v13.1.5-darwin-x64.zip duration=39.719s
  • default Electron icon is used  reason=application icon is not set
    • signing file=/Users/** path ***/Electron.Net/bin/Desktop/mac/Electron.Net.app identityName=Apple Development: **email** (WXWWL34DPU) identityHash=E9CA415AC9AEB795772240047C20924B4E246354 provisioningProfile=none
  • building        target=macOS zip arch=x64 file=/Users/** path ***/Electron.Net/bin/Desktop/Electron.Net-1.0.0-mac.zip
  • building        target=DMG arch=x64 file=/Users/** path ***/Electron.Net/bin/Desktop/Electron.Net-1.0.0.dmg
  • building block map blockMapFile=/Users/** path ***/Electron.Net/bin/Desktop/Electron.Net-1.0.0.dmg.blockmap
  • building block map blockMapFile=/Users/** path ***/Electron.Net/bin/Desktop/Electron.Net-1.0.0-mac.zip.blockmap
  
... done
```



All works fine with .NET Core 6. You just need .NET Core 5.0 to electronize the app.


The Startup is different and now looks like this, and we add back the Startup.cs class


```c#

using ElectronNET.API;

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseElectron(args);
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .RunAsync();

```

I added the code to open the window in Startup.cs

```c#
     Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
```


# Multiple class library projects fail to build?

If you are trying to build a project that consists of multiple libraries you may get the following error:
```
error NETSDK1099: Publishing to a single-file is only supported for executable applications.
```
Use the following to fix:
```
electronize build /target win /PublishSingleFile false /PublishReadyToRun false
```
All Good in the universe.

