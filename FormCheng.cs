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
    public partial class FormCheng : Form
    {
        int id;
        public FormCheng(int id)
        {
            this.id = id;
            InitializeComponent();
            ControlBox = false;
        }
        OrderDB db = new OrderDB();
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void FormCheng_Load(object sender, EventArgs e)
        {
           Client cl = db.context.Clients.Where(p => (p.UserID == id)).FirstOrDefault();
            textBox1.Text = db.Decrypt(cl.ClientName);
            textBox2.Text = db.Decrypt(cl.Address);
            textBox3.Text = db.Decrypt(cl.Phone);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FormPS frm = new FormPS(id))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    UserStatu cl = db.context.UserStatus.Where(p => (p.UserID == id)).FirstOrDefault();
                    if(cl.Password == db.GetHash(frm.textBox1.Text))
                        cl.Password = db.GetHash(frm.textBox2.Text);
                    else
                    {
                        MessageBox.Show("Неверный старый пароль!");
                    }
                    db.context.SaveChanges();
                }
            }
        }
    }
}
