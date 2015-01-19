using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OXE_CDR_Reader
{
    class CDREntry
    {
        private Dictionary<string, string> _listCDREntryElements = new Dictionary<string, string>();
        private string _data = "";

        public CDREntry() { }

        public void setCDREntryElements(Dictionary<string, string> list)
        {
            _listCDREntryElements = list;
        }

        public Dictionary<string, string> getCDREntryElements()
        {
            return _listCDREntryElements;
        }

        public string getRawData() { return _data; }

        public void parseRawData(List<CDRColumn> columns) {
            this._listCDREntryElements.Clear();

            if (columns != null && _data != "") {
                foreach (CDRColumn column in columns)
                {
                    if (column.start >= 0 && column.start < _data.Length)
                    {
                        string tmpData = "";
                        if (column.start + column.length < _data.Length)
                        {
                            tmpData = _data.Substring(column.start, column.length);
                        }
                        else {
                            tmpData = _data.Substring(column.start, _data.Length - column.start);
                        }

                        tmpData = tmpData.Trim();

                        if (column.title != "" && this._listCDREntryElements.ContainsKey(column.title) )
                        {
                            //Element exist, update value
                            this._listCDREntryElements[column.title] = tmpData;
                        }
                        else { //New element 
                            this._listCDREntryElements.Add(column.title, tmpData);
                        }
                    }
                }
            }
        }

        public void parseRawData(List<CDRColumn> columns, string data)
        {
            _data = data;
            parseRawData(columns);
        }
    }
}
