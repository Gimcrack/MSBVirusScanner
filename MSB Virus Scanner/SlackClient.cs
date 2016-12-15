using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace MSB_Virus_Scanner.Slack
{
    //A simple C# class to post messages to a Slack channel
    //Note: This class uses the Newtonsoft Json.NET serializer available via NuGet
    public class Client
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();

        public Client(string urlWithAccessToken)
        {
            _uri = new Uri(urlWithAccessToken);
        }

        //Post a message using simple strings
        public void PostMessage(string text, string username = null, string channel = null)
        {
            Payload payload = new Payload()
            {
                Channel = channel,
                Username = username,
                Text = text
            };

            PostMessage(payload);
        }

        //Post a message using a Payload object
        public void PostMessage(Payload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            //Program.log.write("Payload JSON");
            //Program.log.write(payloadJson);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;

                var response = client.UploadValues(_uri, "POST", data);

                //The response text is usually "ok"
                string responseText = _encoding.GetString(response);
            }
        }
    }

    //This class serializes into the Json payload required by Slack Incoming WebHooks
    public class Payload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }

        public Payload()
        {
            Attachments = new List<Attachment>();
        }

        public void Attach( Attachment a )
        {
            this.Attachments.Add(a);
        }
    }

    public class Attachment
    {
        [JsonProperty("fallback")]
        public string Fallback { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("pretext")]
        public string Pretext { get; set; }

        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_link")]
        public string AuthorLink { get; set; }

        [JsonProperty("author_icon")]
        public string AuthorIcon { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_link")]
        public string TitleLink { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("thumb_url")]
        public string ThumbUrl { get; set; }

        [JsonProperty("footer")]
        public string Footer { get; set; }

        [JsonProperty("footer_icon")]
        public string FooterIcon { get; set; }

        [JsonProperty("ts")]
        public int TimeStamp { get; set; }

        [JsonProperty("fields")]
        public List<Field> Fields { get; set; }

        public Attachment()
        {
            AuthorName = "ScanBot";
            AuthorIcon = "http://icons.iconarchive.com/icons/icons8/windows-8/512/Healthcare-Virus-icon.png";
            Color = "danger";
            Pretext = Environment.MachineName + " " + Program.GetLocalIPAddress();
            ThumbUrl = "https://cdn3.iconfinder.com/data/icons/ose/Warning.png";
            Fields = new List<Field>();
            TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Footer = "MSB Virus Scanner";
            FooterIcon = "http://cdn.publicsurplus.com/sms/docviewer/logo/173477/2681997";
        }

        public void AddField( string key, string val )
        {
            this.Fields.Add(new Field()
            {
                Title = key,
                Value = val,
                Short = true
            });
        }

        public void AddField( string key, string val, bool IsShort)
        {
            this.Fields.Add(new Field()
            {
                Title = key,
                Value = val,
                Short = IsShort
            });
        }
    }

    public class Field
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("short")]
        public Boolean Short { get; set; }
    }
}
