@echo off

if exist "c:\scripts\MSB_Virus_Sentry" (
	ECHO SERVICE EXISTS. REMOVING...
	cd c:\scripts\MSB_Virus_Sentry
	net stop MSB_Virus_Sentry
	"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" -u "MSB Virus Scanner.exe"
	ECHO SERVICE REMOVED
)