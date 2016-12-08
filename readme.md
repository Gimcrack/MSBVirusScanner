# MSB Crypto Virus Scanner

Scans local and removable drives for files matching patterns known to be associated with crypto viruses.  Automatically includes patterns from https://fsrm.experiant.ca/api/v1/combined.

## Usage

 - Clone repo
 - Copy App.config.example App.config
 - Setup initial parameters in App.config
   - email_to
   - email_from
   - email_server
   - email_username
   - email_password
   - email_port
   - slack_hook - Slack webhook url. Send a message to Slack.
   - debug - Control debug mode. Debug mode disables user notifications and directs alerts to a different email
   - debug_email - The email that will receive debug alerts.
 - Set runtime parameters in App.config
   - patterns 
     - additional patterns to search for
	 - separate multiple patterns with pipe | character.
   - whitelist 
     - patterns to whitelist
	 - separate multiple patterns with pipe | character.
   - action
     - alert 
	   - alerts the user and sends notification email
     - disconnect 
	   - alerts the user and disconnects target computer from network
     - shutdown 
	   - alerts the user and shuts down the target computer
 - Build Project
 - Run executable on target computer.
 

## Credits
 
 Thank you to https://fsrm.experiant.ca for providing the API.
 
## Author
 J. Bloomstrom
 
## License
 MIT
 
 