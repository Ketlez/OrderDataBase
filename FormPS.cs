using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderDataBase
{
    public partial class FormPS : Form
    {
        int id;
        public FormPS(int id)
        {
            InitializeComponent();
            ControlBox = false;
            this.id = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void FormPS_Load(object sender, EventArgs e)
        {

        }
    }
}
