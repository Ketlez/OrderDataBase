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
    public partial class FormMain : Form
    {
        
        public FormMain()
        {

            this.FormClosing += closingCallback;
            InitializeComponent();
        }
        public void closingCallback(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CurrentForm.f == this)
                CurrentForm.f = null;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            using (FormLogin form = new FormLogin())
            {
                if(form.ShowDialog()==DialogResult.OK)
                    this.Close();
            };
        }

        private void buttonNotLogin_Click(object sender, EventArgs e)
        {
            CurrentForm.f = new FormGuest();
            this.Close();
        }

        private void buttonRegistration_Click(object sender, EventArgs e)
        {
            FormRegistration form = new FormRegistration();
            form.ShowDialog();
        }
    }
}
