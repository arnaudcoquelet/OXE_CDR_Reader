using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DgvFilterPopup;

namespace OXE_CDR_Reader
{
    public partial class Form1 : Form
    {
        List<CDRFile> _listCDRFiles = new List<CDRFile>();
        DataTable _dataTableCDRs = new DataTable();
        DataTable _dataTableCDRColumns = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void importCDRData() {
            //For each Files, import them
            this._dataTableCDRs.Clear();
            if (this._listCDRFiles != null && this._listCDRFiles.Count > 0)
            {
                //Create Columns
                List<CDRColumn> listColumns = this._listCDRFiles[0].getColumnsDefinition();
                if (listColumns != null && listColumns.Count > 0)
                {
                    //Create Filter Column Table
                    this._dataTableCDRColumns.Clear();
                    this._dataTableCDRColumns.Columns.Add("Visible", typeof(bool));
                    this._dataTableCDRColumns.Columns.Add("Name", typeof(string));

                    DataRow tmpColumnFilterRow = this._dataTableCDRColumns.NewRow();
                    tmpColumnFilterRow["Visible"] = true;
                    tmpColumnFilterRow["Name"] = "Filename";
                    this._dataTableCDRColumns.Rows.Add(tmpColumnFilterRow);



                    this._dataTableCDRs.Columns.Clear();
                    this._dataTableCDRs.Columns.Add("Filename", typeof(string));
                    
                    foreach (CDRColumn column in listColumns)
                    {
                        this._dataTableCDRs.Columns.Add(column.title, typeof(string));

                        //Create Filter Column Entry
                        tmpColumnFilterRow = this._dataTableCDRColumns.NewRow();
                        tmpColumnFilterRow["Visible"] = true;
                        tmpColumnFilterRow["Name"] = column.title;
                        this._dataTableCDRColumns.Rows.Add(tmpColumnFilterRow);
                    }
                }

                //Add the Data
                foreach (CDRFile cdrFile in this._listCDRFiles)
                {
                    string tmpCDRFileName = cdrFile.getShortFilename();
                    List<CDREntry> tmpCDREntries = cdrFile.getCDREntries();

                    if (tmpCDREntries != null && tmpCDREntries.Count > 0)
                    {
                        foreach (CDREntry tmpCDREntry in tmpCDREntries)
                        {
                            DataRow tmpDataRow = this._dataTableCDRs.NewRow();
                            tmpDataRow["Filename"] = cdrFile.getShortFilename();

                            Dictionary<string, string> tmpListCDREntryElements = tmpCDREntry.getCDREntryElements();
                            if (tmpListCDREntryElements != null && tmpListCDREntryElements.Count > 0)
                            {
                                string[] tmpKeys = tmpListCDREntryElements.Keys.ToArray();
                                if (tmpKeys.Length > 0)
                                {
                                    foreach (string tmpKey in tmpKeys)
                                    {
                                        tmpDataRow[tmpKey] = tmpListCDREntryElements[tmpKey];
                                    }
                                }
                            }

                            this._dataTableCDRs.Rows.Add(tmpDataRow);
                        }
                    }
                }

                dataGridViewCDRs.DataSource = this._dataTableCDRs.DefaultView;

                DgvFilterManager filterManager = new DgvFilterManager(dataGridViewCDRs);
            }
        }

        private void createCDRFilters() { 
        
        
        }

        private void importCDRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Request Files to load
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _listCDRFiles.Clear();

                //if (openFileDialog1.Multiselect) { 
                //Open MULTIPLE files
                string[] filenames = openFileDialog.FileNames;

                if (filenames.Length > 0)
                {
                    foreach (string filename in filenames)
                    {
                        CDRFile tmpCDRFile = new CDRFile(filename);
                        tmpCDRFile.parsCDRFile();
                        _listCDRFiles.Add(tmpCDRFile);
                    }
                }


                this.importCDRData();
            }



        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._listCDRFiles.Clear();
            this._listCDRFiles.TrimExcess();
            this._dataTableCDRs.Clear();
            this._dataTableCDRs = new DataTable();

            this._dataTableCDRColumns.Clear();
            this._dataTableCDRColumns = new DataTable();

            GC.Collect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void columnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColumnFilterForm columnFilterFrm = new ColumnFilterForm();
            columnFilterFrm.setListColumn(this._dataTableCDRColumns);
            if (columnFilterFrm.ShowDialog() == DialogResult.OK)
            { 
                //Filter the view
                //this.Refresh();


                foreach(DataRow tmpDataRow in this._dataTableCDRColumns.Rows)
                {
                    bool visible = (bool) tmpDataRow["Visible"];
                    string field = (string) tmpDataRow["Name"];
                
                    dataGridViewCDRs.Columns[field].Visible = visible;
                }
            
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "CSV|*.csv";
            saveFileDialog1.Title = "Save an CSV File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                var sb = new StringBuilder();
                var headers = dataGridViewCDRs.Columns.Cast<DataGridViewColumn>();
                sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));
                foreach (DataGridViewRow row in dataGridViewCDRs.Rows)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>();
                    sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
                }

                System.IO.StreamWriter fs = new System.IO.StreamWriter(saveFileDialog1.OpenFile() );
                fs.Write(sb.ToString());

                fs.Close();
            }



        }
    }
}
