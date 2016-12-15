@echo off

if exist "c:\scripts\MSB_Virus_Sentry" (
	ECHO SERVICE EXISTS. REMOVING...
	cd c:\scripts\MSB_Virus_Sentry
	net stop MSB_Virus_Sentry
	"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" -u "MSB Virus Scanner.exe"
	ECHO SERVICE REMOVED
)

ECHO INSTALLING SERVICE
if not exist "c:\scripts\MSB_Virus_Sentry" mkdir "c:\scripts\MSB_Virus_Sentry"
copy "\\dsjkb\desoft$\MSB_Virus_Sentry\*.*" "c:\scripts\MSB_Virus_Sentry\"
cd c:\scripts\MSB_Virus_Sentry
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "MSB Virus Scanner.exe"

ECHO SERVICE INSTALLED. STARTING...
net start MSB_Virus_Sentry
ECHO DONE