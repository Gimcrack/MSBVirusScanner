# MSB Virus Scanner

A simple Windows utility for detecting Crypto/Ransomware Viruses

![Main Menu](https://github.com/akmatsu/MSBVirusScanner/raw/master/screenshot.png "Main Menu")

## Operation
- Scanner Mode - Scans local and removable drives for files matching patterns known to be associated with crypto viruses.  
-- Automatically includes patterns from https://fsrm.experiant.ca/api/v1/combined.
-- Optionally specify your own list of patterns to scan for
-- Whitelist patterns to avoid false positives
- Sentry Mode - Monitors local drives for infected files
-- File Creation Events - Files are automatically scanned 
-- File Rename Events - Files are automatically scanned
- Service Mode - Fully Automated Protection
-- Runs Sentry Mode continuously
-- Runs a full scan daily 
- All Modes - Take action when an infected files is found
-- Alerts user of the infection
-- Optionally disables network connections
-- Optionally shuts down infected computer
- All Modes - Alert Service Desk or others
-- Send email notification (optional)
-- Send Slack notification (optional)
- All Modes - Logs Activity
-- Log to text file (Scanner Mode)
-- Log to Windows Event Log
-- Log to SQL Database (optional)

## Configuration

Configure the application by choosing option 5 at the Main Menu. You can also edit the .config file directly.

![Configuration](https://github.com/akmatsu/MSBVirusScanner/raw/master/config.png "Configuration") 

## Contributing
 I'm always open to suggestiosn. Send me a PR or open an issue with GitHub.

## Credits
 Thank you to https://fsrm.experiant.ca for providing the API.
 
## Author
 J. Bloomstrom

## License
 MIT
 
 