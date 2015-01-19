using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


namespace OXE_CDR_Reader
{
    class CDRFile
    {
        public string filename = "";
        private List<CDRColumn> _listColumns = new List<CDRColumn>();
        public List<CDREntry> _listEntries = new List<CDREntry>();

        private int _parsedLines = -1;


        public CDRFile() { }
        public CDRFile(string filename) { this.filename = filename; }

        public void parsCDRFile() {
            _listColumns.Clear();
            _listEntries.Clear();

            if (File.Exists(this.filename))
            {
                this.parseColumnDefinition();
                this.parseData(this._listColumns);
            }
        }

        public void parsCDRFile(string filename) { 
            this.filename = filename;
            this.parsCDRFile();
        }

        public void parseColumnDefinition() {
            StreamReader reader = File.OpenText(this.filename);
            string data = reader.ReadLine();
            if (data != null) { this._parseColumnDefinition(data); }
            reader.Close();
        }

        private void _parseColumnDefinition(string data) {
            if (data != null && data.Length > 0 && data.Substring(0, 1) == "#")
            {
                string pattern = @"(\w*,\w*,\w*,\w*),*";
                MatchCollection matches = Regex.Matches(data, pattern);
                foreach (Match match in matches)
                {
                    string tmpTxt = match.Groups[1].Value;
                    string[] fields = tmpTxt.Split(',');
                    if(fields.Length==4)
                    {
                        _listColumns.Add(new CDRColumn(tmpTxt));
                    }
                }

            }

            
        }

        public void parseData(List<CDRColumn> columns) {
            if (this._listColumns != null && this._listColumns.Count > 0)
            {
                StreamReader reader = File.OpenText(this.filename);
                string data;
                int lineNumber = 0;

                while ((data = reader.ReadLine()) != null)
                {
                    if (lineNumber != 0)
                    {
                        this._parseData(this._listColumns, data);
                    }
                    lineNumber++;
                }

                reader.Close();
                _parsedLines = lineNumber-1;
            }
        }

        private void _parseData(List<CDRColumn> columns, string rowData) {
            CDREntry _tmpCDREntry = new CDREntry();
            _tmpCDREntry.parseRawData(columns, rowData);

            this._listEntries.Add(_tmpCDREntry);
        }

        public string getShortFilename()
        {
           return Path.GetFileName(this.filename);
        }

        public List<CDRColumn> getColumnsDefinition() { return this._listColumns; }

        public List<CDREntry> getCDREntries() { return this._listEntries;  }
    }
}
