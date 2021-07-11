:: Ensure we have prerequisites available
where /q msbuild.exe || ECHO Cound not find msbuild. && EXIT /B
where /q makensis.exe || ECHO Cound not find makensis. && EXIT /B

:: Build all projects (in parallel)
msbuild ..\IEInfotips.sln /property:Configuration=Release /m

:: Create the installer output directory if required
mkdir output

:: Delete any previous installer build
del output /q

:: Documentation
copy ..\Documentation\Readme.html output\

:: Config files
copy areaDatas.xml output\
copy tlkLocations.xml output\

:: Resources
copy icon.ico output\
copy ieshellex.nsi output\
copy re.bat output\
copy install.cmd output\
copy uninstall.cmd output\
copy ServerRegistrationManager.exe output\
copy ServerRegistrationManager.exe.config output\
copy SharpShell.dll output\
copy System.Management.Automation.dll output\
copy Microsoft.Management.Infrastructure.dll output\

:: DLL files
copy ..\IE2DAInfotip\bin\Release\IE2DAInfotip.dll output\
copy ..\IEAREInfotip\bin\Release\IEAREInfotip.dll output\
copy ..\IEBAFInfotip\bin\Release\IEBAFInfotip.dll output\
copy ..\IEBAMInfotip\bin\Release\IEBAMInfotip.dll output\
copy ..\IEBCSInfotip\bin\Release\IEBCSInfotip.dll output\
copy ..\IECHRInfotip\bin\Release\IECHRInfotip.dll output\
copy ..\IECHUInfotip\bin\Release\IECHUInfotip.dll output\
copy ..\IECREInfotip\bin\Release\IECREInfotip.dll output\
copy ..\IEDLGInfotip\bin\Release\IEDLGInfotip.dll output\
copy ..\IEEFFInfotip\bin\Release\IEEFFInfotip.dll output\
copy ..\IEGAMInfotip\bin\Release\IEGAMInfotip.dll output\
copy ..\IEGLSLInfotip\bin\Release\IEGLSLInfotip.dll output\
copy ..\IEGUIInfotip\bin\Release\IEGUIInfotip.dll output\
copy ..\IEIDSInfotip\bin\Release\IEIDSInfotip.dll output\
copy ..\IEINIInfotip\bin\Release\IEINIInfotip.dll output\
copy ..\IEITMInfotip\bin\Release\IEITMInfotip.dll output\
copy ..\IEKEYInfotip\bin\Release\IEKEYInfotip.dll output\
copy ..\IELUAInfotip\bin\Release\IELUAInfotip.dll output\
copy ..\IEMENUInfotip\bin\Release\IEMENUInfotip.dll output\
copy ..\IEMOSInfotip\bin\Release\IEMOSInfotip.dll output\
copy ..\IEMosPreview\bin\Release\IEMOSPreview.dll output\
copy ..\IEMUSInfotip\bin\Release\IEMUSInfotip.dll output\
copy ..\IEPROInfotip\bin\Release\IEPROInfotip.dll output\
copy ..\IESPLInfotip\bin\Release\IESPLInfotip.dll output\
copy ..\IESQLInfotip\bin\Release\IESQLInfotip.dll output\
copy ..\IESTOInfotip\bin\Release\IEStoInfotip.dll output\
copy ..\IETISInfotip\bin\Release\IETISInfotip.dll output\
copy ..\IETISPreview\bin\Release\IETISPreview.dll output\
copy ..\IETLKInfotip\bin\Release\IETlkInfotip.dll output\
copy ..\IEVARInfotip\bin\Release\IEVARInfotip.dll output\
copy ..\IEVEFInfotip\bin\Release\IEVEFInfotip.dll output\
copy ..\IEVVCInfotip\bin\Release\IEVVCInfotip.dll output\
copy ..\IEWEDInfotip\bin\Release\IEWEDInfotip.dll output\
copy ..\IEWFXInfotip\bin\Release\IEWFXInfotip.dll output\
copy ..\IEWMAPInfotip\bin\Release\IEWMPInfotip.dll output\

:: PS1 files
copy ..\IE2DAInfotip\bin\Release\Powershell\2da.ps1 output\
copy ..\IEAREInfotip\bin\Release\Powershell\are.ps1 output\
copy ..\IEBAFInfotip\bin\Release\Powershell\baf.ps1 output\
copy ..\IEBAMInfotip\bin\Release\Powershell\bam.ps1 output\
copy ..\IEBCSInfotip\bin\Release\Powershell\bcs.ps1 output\
copy ..\IECHRInfotip\bin\Release\Powershell\chr.ps1 output\
copy ..\IECHUInfotip\bin\Release\Powershell\chu.ps1 output\
copy ..\IECREInfotip\bin\Release\Powershell\cre.ps1 output\
copy ..\IEDLGInfotip\bin\Release\Powershell\dlg.ps1 output\
copy ..\IEEFFInfotip\bin\Release\Powershell\eff.ps1 output\
copy ..\IEGAMInfotip\bin\Release\Powershell\gam.ps1 output\
copy ..\IEGLSLInfotip\bin\Release\Powershell\glsl.ps1 output\
copy ..\IEGUIInfotip\bin\Release\Powershell\gui.ps1 output\
copy ..\IEIDSInfotip\bin\Release\Powershell\ids.ps1 output\
copy ..\IEINIInfotip\bin\Release\Powershell\ini.ps1 output\
copy ..\IEITMInfotip\bin\Release\Powershell\itm.ps1 output\
copy ..\IEKEYInfotip\bin\Release\Powershell\key.ps1 output\
copy ..\IELUAInfotip\bin\Release\Powershell\lua.ps1 output\
copy ..\IEMENUInfotip\bin\Release\Powershell\menu.ps1 output\
copy ..\IEMOSInfotip\bin\Release\Powershell\mos.ps1 output\
copy ..\IEMUSInfotip\bin\Release\Powershell\mus.ps1 output\
copy ..\IEPROInfotip\bin\Release\Powershell\pro.ps1 output\
copy ..\IESPLInfotip\bin\Release\Powershell\spl.ps1 output\
copy ..\IESQLInfotip\bin\Release\Powershell\sql.ps1 output\
copy ..\IESTOInfotip\bin\Release\Powershell\sto.ps1 output\
copy ..\IETISInfotip\bin\Release\Powershell\tis.ps1 output\
copy ..\IETLKInfotip\bin\Release\Powershell\tlk.ps1  output\
copy ..\IEVARInfotip\bin\Release\Powershell\var.ps1 output\
copy ..\IEVEFInfotip\bin\Release\Powershell\vef.ps1 output\
copy ..\IEVVCInfotip\bin\Release\Powershell\vvc.ps1 output\
copy ..\IEWEDInfotip\bin\Release\Powershell\wed.ps1 output\
copy ..\IEWFXInfotip\bin\Release\Powershell\wfx.ps1 output\
copy ..\IEWMAPInfotip\bin\Release\Powershell\wmp.ps1 output\

:: build installer
makensis output\ieshellex.nsi