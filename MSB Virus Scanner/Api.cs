using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;
using System.Management;
using ServiceStack.Text;
using Newtonsoft.Json;

namespace CustomExtensions
{
    public static class StringExtension
    {
        public static T Deserialize<T>(this String json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
    
}

namespace MSB_Virus_Scanner
{
    using CustomExtensions;

    public class Api 
    {
        private readonly Encoding _encoding = new UTF8Encoding();
        public string base_url = Program.config["api_base_url"].ToString();

        public async void get(string url)
        {
            HttpClient client = new HttpClient() ;

            client.BaseAddress = new Uri(base_url);

            HttpResponseMessage response = await client.GetAsync(url);  // Blocking call!
                
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> get<T>(string url)
        {
            HttpClient client = new HttpClient() ;

            client.BaseAddress = new Uri(base_url);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = await client.GetAsync(url);  // Blocking call!
                
            response.EnsureSuccessStatusCode();
                
            string data = await response.Content.ReadAsStringAsync();

            return data.Deserialize<T>();
        }

        public async Task post(string url)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(base_url);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = await client.PostAsync(url, null);  // Blocking call!

            response.EnsureSuccessStatusCode();
        }

        public async Task<T> post<T>(string url, Object payload)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(base_url);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = await client.PostAsJsonAsync(url, payload);  // Blocking call!

            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();

            return data.Deserialize<T>();
        }

        public async Task<HttpStatusCode> post(string url, List<KeyValuePair<string, string>> payload)
        {

            var client = new HttpClient();

            Console.WriteLine("Posting to {0}",url);

            var content = new FormUrlEncodedContent(payload);

            var response = await client.PostAsync(base_url + url, content);
            var contents = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return response.StatusCode;
        }

        public class Payload
        {
            private List<KeyValuePair<string, string>> _data;

            public Payload()
            {
                _data = new List<KeyValuePair<string, string>>();
            }

            public Payload(string key, string value)
            {
                _data = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>(key, value)
                };
            }

            public void Add(string key, string value)
            {
                _data.Add(new KeyValuePair<string, string>(key, value));
            }

            public List<KeyValuePair<string,string>> Data()
            {
                return _data;
            }
        }
    }
}
