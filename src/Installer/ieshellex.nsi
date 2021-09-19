; areaData.xml -> PST, IWD, IWD2
; icon in control panel
; installation option page
; uninstallation option page


;----------------------------------------------------------------
; iese.nsi
;----------------------------------------------------------------

Name "Infinity Engine Shell Extensions"
Icon "icon.ico"
UninstallIcon "icon.ico"
Unicode True 
OutFile "ieshellex.exe"

;Require admin rights on NT6+ (When UAC is turned on)
RequestExecutionLevel admin

!include LogicLib.nsh

Function .onInit
UserInfo::GetAccountType
pop $0
${If} $0 != "admin" ;Require admin rights on NT4+
    MessageBox mb_iconstop "Administrator rights required!"
    SetErrorLevel 740 ;ERROR_ELEVATION_REQUIRED
    Quit
${EndIf}
FunctionEnd

Function un.onInit
UserInfo::GetAccountType
pop $0
${If} $0 != "admin" ;Require admin rights on NT4+
    MessageBox mb_iconstop "Administrator rights required!"
    SetErrorLevel 740 ;ERROR_ELEVATION_REQUIRED
    Quit
${EndIf}
FunctionEnd


InstallDir "$DOCUMENTS"
DirText "This will install Infinity Engine Shell Extensions on your computer. Choose a directory"

;----------------------------------------------------------------

Section ""

SetOutPath $INSTDIR

WriteRegStr HKCR ".2da" "Content Type" "text/plain"
WriteRegStr HKCR ".2da" "PerceivedType" "text"
WriteRegStr HKCR ".2da\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@" "{1531d583-8375-4d3f-b5fb-d23bbd169f22}"

WriteRegStr HKCR ".tp2" "Content Type" "text/plain"
WriteRegStr HKCR ".tp2" "PerceivedType" "text"
WriteRegStr HKCR ".tp2\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@" "{1531d583-8375-4d3f-b5fb-d23bbd169f22}"

WriteRegStr HKCR ".d" "Content Type" "text/plain"
WriteRegStr HKCR ".d" "PerceivedType" "text"
WriteRegStr HKCR ".d\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@" "{1531d583-8375-4d3f-b5fb-d23bbd169f22}"

WriteRegStr HKCR ".lua" "Content Type" "text/plain"
WriteRegStr HKCR ".lua" "PerceivedType" "text"
WriteRegStr HKCR ".lua\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@" "{1531d583-8375-4d3f-b5fb-d23bbd169f22}"

WriteRegStr HKCR ".tra" "Content Type" "text/plain"
WriteRegStr HKCR ".tra" "PerceivedType" "text"
WriteRegStr HKCR ".tra\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@" "{1531d583-8375-4d3f-b5fb-d23bbd169f22}"

WriteRegStr HKCR ".mus" "Content Type" "text/plain"
WriteRegStr HKCR ".mus" "PerceivedType" "text"
WriteRegStr HKCR ".mus\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@" "{1531d583-8375-4d3f-b5fb-d23bbd169f22}"

WriteRegStr HKCR ".bcs" "Content Type" "text/plain"
WriteRegStr HKCR ".bcs" "PerceivedType" "text"
WriteRegStr HKCR ".bcs\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@" "{1531d583-8375-4d3f-b5fb-d23bbd169f22}"

WriteRegStr HKCR ".baf" "Content Type" "text/plain"
WriteRegStr HKCR ".baf" "PerceivedType" "text"
WriteRegStr HKCR ".baf\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@" "{1531d583-8375-4d3f-b5fb-d23bbd169f22}"

WriteRegStr HKCR ".ids" "Content Type" "text/plain"
WriteRegStr HKCR ".ids" "PerceivedType" "text"
WriteRegStr HKCR ".ids\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@" "{1531d583-8375-4d3f-b5fb-d23bbd169f22}"

CreateDirectory $INSTDIR\bin
CreateDirectory $INSTDIR\config

File Readme.html

File /oname=config\2da.ps1 2da.ps1
File /oname=config\are.ps1 are.ps1
File /oname=config\baf.ps1 baf.ps1
File /oname=config\bam.ps1 bam.ps1
File /oname=config\bcs.ps1 bcs.ps1
File /oname=config\chr.ps1 chr.ps1
File /oname=config\chu.ps1 chu.ps1
File /oname=config\cre.ps1 cre.ps1
File /oname=config\dlg.ps1 dlg.ps1
File /oname=config\eff.ps1 eff.ps1
File /oname=config\gam.ps1 gam.ps1
File /oname=config\glsl.ps1 glsl.ps1
File /oname=config\gui.ps1 gui.ps1
File /oname=config\ids.ps1 ids.ps1
File /oname=config\ini.ps1 ini.ps1
File /oname=config\itm.ps1 itm.ps1
File /oname=config\key.ps1 key.ps1
File /oname=config\lua.ps1 lua.ps1
File /oname=config\menu.ps1 menu.ps1
File /oname=config\mos.ps1 mos.ps1
File /oname=config\mus.ps1 mus.ps1
File /oname=config\pro.ps1 pro.ps1
File /oname=config\spl.ps1 spl.ps1
File /oname=config\sql.ps1 sql.ps1
File /oname=config\sto.ps1 sto.ps1
File /oname=config\tis.ps1 tis.ps1
File /oname=config\tlk.ps1 tlk.ps1
File /oname=config\var.ps1 var.ps1
File /oname=config\vef.ps1 vef.ps1
File /oname=config\vvc.ps1 vvc.ps1
File /oname=config\wed.ps1 wed.ps1
File /oname=config\wfx.ps1 wfx.ps1
File /oname=config\wmp.ps1 wmp.ps1
File /oname=config\MPAL256.BMP MPAL256.BMP
File /oname=config\tlkLocations.xml tlkLocations.xml
File /oname=config\areaDatas.xml areaDatas.xml

File /oname=bin\IE2DAInfotip.dll IE2DAInfotip.dll
File /oname=bin\IEAREInfotip.dll IEAREInfotip.dll
File /oname=bin\IEBAFInfotip.dll IEBAFInfotip.dll
File /oname=bin\IEBAMInfotip.dll IEBAMInfotip.dll
File /oname=bin\IEBAMPreview.dll IEBAMPreview.dll
File /oname=bin\IEBCSInfotip.dll IEBCSInfotip.dll
File /oname=bin\IECHRInfotip.dll IECHRInfotip.dll
File /oname=bin\IECHUInfotip.dll IECHUInfotip.dll
File /oname=bin\IECREInfotip.dll IECREInfotip.dll
File /oname=bin\IEDLGInfotip.dll IEDLGInfotip.dll
File /oname=bin\IEEFFInfotip.dll IEEFFInfotip.dll
File /oname=bin\IEGAMInfotip.dll IEGAMInfotip.dll
File /oname=bin\IEGLSLInfotip.dll IEGLSLInfotip.dll
File /oname=bin\IEGUIInfotip.dll IEGUIInfotip.dll
File /oname=bin\IEIDSInfotip.dll IEIDSInfotip.dll
File /oname=bin\IEINIInfotip.dll IEINIInfotip.dll
File /oname=bin\IEITMInfotip.dll IEITMInfotip.dll
File /oname=bin\IEKEYInfotip.dll IEKEYInfotip.dll
File /oname=bin\IELUAInfotip.dll IELUAInfotip.dll
File /oname=bin\IEMENUInfotip.dll IEMENUInfotip.dll
File /oname=bin\IEMOSInfotip.dll IEMOSInfotip.dll
File /oname=bin\IEMosPreview.dll IEMosPreview.dll
File /oname=bin\IEMUSInfotip.dll IEMUSInfotip.dll
File /oname=bin\IEPLTPreview.dll IEPLTPreview.dll
File /oname=bin\IEPROInfotip.dll IEPROInfotip.dll
File /oname=bin\IESPLInfotip.dll IESPLInfotip.dll
File /oname=bin\IESQLInfotip.dll IESQLInfotip.dll
File /oname=bin\IEStoInfotip.dll IEStoInfotip.dll
File /oname=bin\IETlkInfotip.dll IETlkInfotip.dll
File /oname=bin\IETisInfotip.dll IETisInfotip.dll
File /oname=bin\IETisPreview.dll IETisPreview.dll
File /oname=bin\IEVARInfotip.dll IEVARInfotip.dll
File /oname=bin\IEVEFInfotip.dll IEVEFInfotip.dll
File /oname=bin\IEVVCInfotip.dll IEVVCInfotip.dll
File /oname=bin\IEWEDInfotip.dll IEWEDInfotip.dll
File /oname=bin\IEWFXInfotip.dll IEWFXInfotip.dll
File /oname=bin\IEWMPInfotip.dll IEWMPInfotip.dll
File /oname=bin\ServerRegistrationManager.exe ServerRegistrationManager.exe
File /oname=bin\ServerRegistrationManager.exe.config ServerRegistrationManager.exe.config
File /oname=bin\Microsoft.Management.Infrastructure.dll Microsoft.Management.Infrastructure.dll
File /oname=bin\SharpShell.dll SharpShell.dll
File /oname=bin\System.Management.Automation.dll System.Management.Automation.dll
File /oname=bin\re.bat re.bat
File /oname=bin\install.cmd install.cmd
File /oname=bin\uninstall.cmd uninstall.cmd
File /oname=bin\icon.ico icon.ico

; Calling serverregistrationmanager directly seems to trigger A/V, so we'll register via a batch file
; which for some reason seems less likely to trigger A/V
ExecWait '"$INSTDIR\bin\install.cmd"'


; include for some of the windows messages defines
!include "winmessages.nsh"
; HKLM (all users) vs HKCU (current user) defines
!define env_hklm 'HKLM "SYSTEM\CurrentControlSet\Control\Session Manager\Environment"'
!define env_hkcu 'HKCU "Environment"'
; set variable for local machine
WriteRegExpandStr ${env_hklm} ieshellex $INSTDIR
; and current user
WriteRegExpandStr ${env_hkcu} ieshellex $INSTDIR
; make sure windows knows about the change
SendMessage ${HWND_BROADCAST} ${WM_WININICHANGE} 0 "STR:Environment" /TIMEOUT=3000


WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Infinity Engine Shell Extensions" "DisplayName" \
"Infinity Engine Shell Extensions (remove only)"

WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Infinity Engine Shell Extensions" "UninstallString" \
"$INSTDIR\Uninstall.exe"

WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Infinity Engine Shell Extensions" "DisplayIcon" \
"$INSTDIR\icon.ico"

WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Infinity Engine Shell Extensions" "NoModify" \
"1"

WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Infinity Engine Shell Extensions" "EstimatedSize" \
"2048"

WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Infinity Engine Shell Extensions" "URLInfoAbout " \
"https://github.com/btigi/ieshellex"

WriteUninstaller $INSTDIR\Uninstall.exe

MessageBox MB_OK "Installation was successful."

SectionEnd


;----------------------------------------------------------------
; The uninstall section
Section "Uninstall"

ExecWait '"$INSTDIR\bin\uninstall.cmd"'

Sleep 500

ExecWait '"$INSTDIR\bin\re.bat"'

Sleep 500

Delete $INSTDIR\Readme.html

Delete $INSTDIR\config\2da.ps1
Delete $INSTDIR\config\are.ps1
Delete $INSTDIR\config\baf.ps1
Delete $INSTDIR\config\bam.ps1
Delete $INSTDIR\config\bcs.ps1
Delete $INSTDIR\config\chr.ps1
Delete $INSTDIR\config\chu.ps1
Delete $INSTDIR\config\cre.ps1
Delete $INSTDIR\config\dlg.ps1
Delete $INSTDIR\config\eff.ps1
Delete $INSTDIR\config\gam.ps1
Delete $INSTDIR\config\glsl.ps1
Delete $INSTDIR\config\gui.ps1
Delete $INSTDIR\config\ids.ps1
Delete $INSTDIR\config\ini.ps1
Delete $INSTDIR\config\itm.ps1
Delete $INSTDIR\config\key.ps1
Delete $INSTDIR\config\lua.ps1
Delete $INSTDIR\config\menu.ps1
Delete $INSTDIR\config\mos.ps1
Delete $INSTDIR\config\mus.ps1
Delete $INSTDIR\config\pro.ps1
Delete $INSTDIR\config\spl.ps1
Delete $INSTDIR\config\sql.ps1
Delete $INSTDIR\config\sto.ps1
Delete $INSTDIR\config\tis.ps1
Delete $INSTDIR\config\tlk.ps1
Delete $INSTDIR\config\var.ps1
Delete $INSTDIR\config\vef.ps1
Delete $INSTDIR\config\vvc.ps1
Delete $INSTDIR\config\wed.ps1
Delete $INSTDIR\config\wfx.ps1
Delete $INSTDIR\config\wmp.ps1

Delete $INSTDIR\config\tlkLocations.xml
Delete $INSTDIR\config\areaDatas.xml
Delete $INSTDIR\config\MPAL256.BMP

Delete $INSTDIR\bin\Uninstall.exe
Delete $INSTDIR\bin\IE2DAInfotip.dll
Delete $INSTDIR\bin\IEAREInfotip.dll
Delete $INSTDIR\bin\IEBAFInfotip.dll
Delete $INSTDIR\bin\IEBAMInfotip.dll
Delete $INSTDIR\bin\IEBAMPreview.dll
Delete $INSTDIR\bin\IEBCSInfotip.dll
Delete $INSTDIR\bin\IECHRInfotip.dll
Delete $INSTDIR\bin\IECHUInfotip.dll
Delete $INSTDIR\bin\IECREInfotip.dll
Delete $INSTDIR\bin\IEDLGInfotip.dll
Delete $INSTDIR\bin\IEEFFInfotip.dll
Delete $INSTDIR\bin\IEGAMInfotip.dll
Delete $INSTDIR\bin\IEGLSLInfotip.dll
Delete $INSTDIR\bin\IEGUIInfotip.dll
Delete $INSTDIR\bin\IEIDSInfotip.dll
Delete $INSTDIR\bin\IEINIInfotip.dll
Delete $INSTDIR\bin\IEITMInfotip.dll
Delete $INSTDIR\bin\IEKEYInfotip.dll
Delete $INSTDIR\bin\IELUAInfotip.dll
Delete $INSTDIR\bin\IEMENUInfotip.dll
Delete $INSTDIR\bin\IEMOSInfotip.dll
Delete $INSTDIR\bin\IEMosPreview.dll
Delete $INSTDIR\bin\IEMUSInfotip.dll
Delete $INSTDIR\bin\IEPLTPreview.dll
Delete $INSTDIR\bin\IEPROInfotip.dll
Delete $INSTDIR\bin\IESPLInfotip.dll
Delete $INSTDIR\bin\IESQLInfotip.dll
Delete $INSTDIR\bin\IEStoInfotip.dll
Delete $INSTDIR\bin\IETlkInfotip.dll
Delete $INSTDIR\bin\IETisInfotip.dll
Delete $INSTDIR\bin\IETisPreview.dll
Delete $INSTDIR\bin\IEVARInfotip.dll
Delete $INSTDIR\bin\IEVEFInfotip.dll
Delete $INSTDIR\bin\IEVVCInfotip.dll
Delete $INSTDIR\bin\IEWEDInfotip.dll
Delete $INSTDIR\bin\IEWFXInfotip.dll
Delete $INSTDIR\bin\IEWMPInfotip.dll
Delete $INSTDIR\bin\ServerRegistrationManager.exe
Delete $INSTDIR\bin\ServerRegistrationManager.exe.config
Delete $INSTDIR\bin\Microsoft.Management.Infrastructure.dll
Delete $INSTDIR\bin\SharpShell.dll
Delete $INSTDIR\bin\System.Management.Automation.dll
Delete $INSTDIR\bin\re.bat
Delete $INSTDIR\bin\install.cmd
Delete $INSTDIR\bin\uninstall.cmd
Delete $INSTDIR\bin\icon.ico

Delete $INSTDIR\uninstall.exe

RMDir $INSTDIR\bin
RMDir $INSTDIR\config
RMDir $INSTDIR

DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.2da" "Content Type"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.2da" "PerceivedType"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.2da\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@"

DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.tp2" "Content Type"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.tp2" "PerceivedType"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.tp2\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@"

DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.d" "Content Type"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.d" "PerceivedType"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.d\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@"

DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.lua" "Content Type"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.lua" "PerceivedType"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.lua\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@"

DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.tra" "Content Type"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.tra" "PerceivedType"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.tra\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@"

DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.mus" "Content Type"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.mus" "PerceivedType"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.mus\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@"

DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.bcs" "Content Type"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.bcs" "PerceivedType"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.bcs\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@"

DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.baf" "Content Type"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.baf" "PerceivedType"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.baf\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@"

DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.ids" "Content Type"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.ids" "PerceivedType"
DeleteRegValue HKCR "HKEY_CLASSES_ROOT\.ids\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}" "@"

DeleteRegValue ${env_hklm} IESE_INSTALL

DeleteRegKey HKEY_LOCAL_MACHINE "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Infinity Engine Shell Extensions"

; delete variable
DeleteRegValue ${env_hklm} ieshellex
DeleteRegValue ${env_hkcu} ieshellex
; make sure windows knows about the change
SendMessage ${HWND_BROADCAST} ${WM_WININICHANGE} 0 "STR:Environment" /TIMEOUT=3000
   
SectionEnd

;----------------------------------------------------------------