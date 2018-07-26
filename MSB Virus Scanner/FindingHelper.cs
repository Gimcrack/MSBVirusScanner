using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSB_Virus_Scanner
{
    public class FindingHelper
    {
        private List<Finding> _findings;


        public FindingHelper()
        {
            Reset();
        }

        public void Reset()
        {
            _findings = new List<Finding>();
        }

        public void Add(Finding finding)
        {
            _findings.Add(finding);
        }

        public void Add(IEnumerable<Finding> findings, string pattern)
        {
            foreach(Finding finding in findings)
            {
                finding.pattern = pattern;
                Add(finding);
            }
        }


        public void Add(string path, string pattern)
        {
            Add(new Finding() { pattern = pattern, path = path, type = Finding.finding_type.File });
        }

        public void Add(string path, string pattern, Finding.finding_type finding_type)
        {
            Add(new Finding() { pattern = pattern, path = path, type = finding_type });
        }

        public void Add(IEnumerable<string> paths, string pattern)
        {
            foreach(string path in paths)
            {
                Add(path, pattern);
            }
        }

        public Boolean HasFindings()
        {
            return _findings.Count() > 0;
        }

        public List<Finding> Get()
        {
            return _findings;
        }

        public string GetFormatted()
        {
            string r = Environment.NewLine;

            foreach(Finding f in _findings)
            {
                r += f.GetFormatted() + Environment.NewLine;
            }

            return r;
        }

        public void Clean()
        {

            // clean all findings
            foreach(Finding f in _findings)
            {
                f.Clean();
            }
        }
    }
}
