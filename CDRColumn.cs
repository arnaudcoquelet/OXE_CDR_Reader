using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OXE_CDR_Reader
{
    class CDRColumn
    {
        public string title = "";
        public int start = -1;
        public int end = -1;
        public int length = -1;
        public string dataAlignment = "";

        public CDRColumn() { }

        public CDRColumn(string definition)
        {
            string[] fields = definition.Split(',');

            if (fields.Length == 4)
            {
                title = fields[0];
                start = Convert.ToInt32(fields[1]) - 1;
                end = Convert.ToInt32(fields[2]);
                length = end - start;
                dataAlignment = fields[3];
            }
        }
    }
}
