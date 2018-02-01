using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSB_Virus_Scanner
{
    public class Models
    {
        public class Client
        {
            public int id { get; set; }
            public string name { get; set; }
            public string version { get; set; }
            public DateTime updated_at { get; set; }
            public DateTime created_at { get; set; }
        }

        public class MatchedFile
        {
            public int id { get; set; }
            public int client_id { get; set; }
            public int pattern_id { get; set; }
            public string file { get; set; }
            public int times_matched { get; set; }
            public bool muted_flag { get; set; }
            public bool acknowledged_flag { get; set; }
            public Client client { get; set; }
            public Pattern pattern { get; set; }
            public DateTime updated_at { get; set; }
            public DateTime created_at { get; set; }
        }

        public class Definition
        {
            public int id { get; set; }
            public string pattern { get; set; }
            public string status { get; set; }
        }

        public class Pattern
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool published_flag { get; set; }
            public DateTime updated_at { get; set; }
            public DateTime created_at { get; set; }
        }

        public class Exemption
        {
            public int id { get; set; }
            public string pattern { get; set; }
            public bool published_flag { get; set; }
            public DateTime updated_at { get; set; }
            public DateTime created_at { get; set; }
        }

    }
}
