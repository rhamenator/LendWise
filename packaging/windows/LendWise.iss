#define AppName "LendWise"
#ifndef AppVersion
  #define AppVersion "0.1.0"
#endif
[Setup]
UninstallDisplayIcon={app}\app-icon.ico
SetupIconFile=app-icon.ico
AppId={{3F6C03FD-E062-407F-B802-BDA33B11B977}
AppName={#AppName}
AppVersion={#AppVersion}
DefaultDirName={autopf}\LendWise
DefaultGroupName={#AppName}
OutputDir=..\..\artifacts
OutputBaseFilename=LendWise-{#AppVersion}-windows-x64-setup
Compression=lzma2
SolidCompression=yes
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
[Files]
Source: "app-icon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
[Icons]
Name: "{group}\LendWise"; Filename: "{app}\LendWise.Web.exe"; IconFilename: "{app}\app-icon.ico"
