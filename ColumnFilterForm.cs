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
    public partial class ColumnFilterForm : Form
    {
        DataTable _listColumn = new DataTable();

        public ColumnFilterForm()
        {
            InitializeComponent();
            this.dataGridView1.DataSource = this._listColumn.DefaultView;
            DgvFilterManager filterManager = new DgvFilterManager(this.dataGridView1);
        }

        public void setListColumn(DataTable listColumn) {
            this._listColumn = listColumn;
            this.dataGridView1.DataSource = this._listColumn.DefaultView;
            DgvFilterManager filterManager = new DgvFilterManager(this.dataGridView1);
        }
        public DataTable getListColumn() { return _listColumn; }

        private void ColumnFilterForm_Load(object sender, EventArgs e)
        {

        }

        private void ColumnFilterForm_Activated(object sender, EventArgs e)
        {
            //this.dataGridView1.Refresh();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; 
            this.Close();
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataRow tmpDataRow in this._listColumn.Rows)
            {
                bool visible = true;
                tmpDataRow["Visible"] = visible;
            }
        }

        private void buttonDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (DataRow tmpDataRow in this._listColumn.Rows)
            {
                bool visible = false;
                tmpDataRow["Visible"] = visible;
            }
        }
    }
}
