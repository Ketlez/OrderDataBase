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
    public partial class FormRegistration : Form
    {
        public FormRegistration()
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

        private void buttonReg_Click(object sender, EventArgs e)
        {
            bool flagpassword = true;
            string password = textBoxPassword.Text;
            if(password.Length<8)
                flagpassword = false;


            bool flagint = false;
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] >= '0' && password[i] <= '9')
                {
                    flagint = true;
                    break;
                }
            }
            if (!flagint)
                flagpassword = false;

            bool flagchar = false;
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] >= 'A' && password[i] <= 'Z')
                {
                    flagchar = true;
                    break;
                }
            }
            if (!flagint)
                flagpassword = false;

            if (flagpassword)
            {
                bool flag = db.CreateUser(textBoxLogin.Text, textBoxPassword.Text, textBoxName.Text, textBoxAdress.Text, textBoxPhone.Text);
                if (flag == false)
                {
                    string message = "Такой логин уже сужествует!";
                    var result = MessageBox.Show(message);
                }
                else
                {
                    string message = "Пользователь успешно создан!";
                    var result = MessageBox.Show(message);
                    this.Close();
                }
            }
            else
            {
                string message = "Пароль должен состоять минимум из 8 символов, иметь цифры и заглавные буквы!";
                var result = MessageBox.Show(message);
            }
        }
    }
}
