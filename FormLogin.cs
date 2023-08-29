using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderDataBase
{
    public partial class FormLogin : Form
    {
       
        public FormLogin()
        {
            this.FormClosing += closingCallback;
            InitializeComponent();
        }

        OrderDB db = new OrderDB();

        public void closingCallback(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CurrentForm.f == this)
                CurrentForm.f = null;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }

        //Вход Логин\пароль
        private void button1_Click_1(object sender, EventArgs e)
        {
            string login = TextBoxLogin.Text;
            string password = GetHash(TextBoxPassword.Text);

            UserStatu user = db.FindEmployee(login, password);

            if (user == null)
            {
                string message = "Пользователь не найден!";
                var result = MessageBox.Show(message);
            }
            else
            {
                if (user.Client != null)
                {
                    CurrentForm.f = new FormUser(user.UserID);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    CurrentForm.f = new FormAdmin(user.UserID);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }

        }
    }
}
