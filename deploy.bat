if exist "c:\scripts\MSB_Virus_Sentry" (
	cd c:\scripts\MSB_Virus_Sentry
	net stop MSB_Virus_Sentry
	"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" -u "MSB Virus Scanner.exe"
)

if not exist "c:\scripts\MSB_Virus_Sentry" mkdir "c:\scripts\MSBServiceMonitor"
copy "\\dsjkb\desoft$\MSB_Virus_Sentry\*.*" "c:\scripts\MSB_Virus_Sentry\"
cd c:\scripts\MSB_Virus_Sentry
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" "MSB Virus Scanner.exe"
net start MSB_Virus_Sentry