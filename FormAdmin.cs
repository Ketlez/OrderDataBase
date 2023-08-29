using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace OrderDataBase
{
    

    public partial class FormAdmin : Form
    {
        public int id;
        public FormAdmin(int id)
        {
            this.id = id;
            this.FormClosing += closingCallback;
            InitializeComponent();
        }

        OrderDB db = new OrderDB();

        public void closingCallback(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CurrentForm.f == this)
                CurrentForm.f = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FormAddProductType frm = new FormAddProductType())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (db.AddProductType(frm.textBox1.Text) == false)
                    {
                        MessageBox.Show("Ошибка вставки! Категория уже существует.");

                    };
                }
            }
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = db.context.ProductTypes.ToList();
        }
        int idProduct = -1;
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            idProduct = Convert.ToInt32(dataGridView2.CurrentRow.Cells["productTypeIDDataGridViewTextBoxColumn"].Value);
            ProductType prtype = db.context.ProductTypes.Where(p => (p.ProductTypeID == idProduct)).FirstOrDefault();
            dataGridView3.DataSource = null;
            dataGridView3.DataSource = prtype.Products.ToList();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
           dataGridView2.DataSource = db.context.ProductTypes.ToList();
            var k = db.context.Clients.Select(p => new ClientDG
            {
                ClientName = p.ClientName,
                Address = p.Address,
                Phone = p.Phone,
                UserID = p.UserID
            });


            // List<Client> cl = db.context.Clients.ToList();
            dataGridView1.DataSource = k.ToList();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["ClientName"].Value = db.Decrypt(row.Cells["ClientName"].Value.ToString());
                row.Cells["Address"].Value = db.Decrypt(row.Cells["Address"].Value.ToString());
                row.Cells["Phone"].Value = db.Decrypt(row.Cells["Phone"].Value.ToString());
            }
            //dataGridView1.DataSource = db.context.Clients.ToList();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int idProd = Convert.ToInt32(dataGridView3.CurrentRow.Cells["productIDDataGridViewTextBoxColumn"].Value);
            Product pr = db.context.Products.Where(p => (p.ProductID == idProd)).FirstOrDefault();

            if (e.ColumnIndex == 4)
            {
                string message = "Вы хотите удалить товар: " + pr.ToString() + "?";
                string caption = "Подтверждение выбора";
                var result = MessageBox.Show(message, caption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // пытаемся удалить ...
                    if (pr.OrderDetails.Count() == 0)
                    {
                        db.context.Products.Remove(pr);
                        db.context.SaveChanges();
                        MessageBox.Show("Товар: " + pr.ToString() + " успешно удален!");


                        dataGridView3.DataSource = null;
                        dataGridView2.DataSource = db.context.ProductTypes.ToList();
                    }
                    else MessageBox.Show("В удалении отказано!");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FormAddProduct frm = new FormAddProduct())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (db.AddProduct(idProduct, frm.textBox1.Text, Convert.ToInt32(frm.textBox2.Text), Convert.ToInt32(frm.textBox3.Text)) == false)
                    {
                        MessageBox.Show("Ошибка вставки! Не выбран тип");

                    };
                }
            }
            dataGridView3.DataSource = null;
            dataGridView3.DataSource = db.context.Products.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CurrentForm.f = new FormMain();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idProduct != -1)
            {

                ProductType pr = db.context.ProductTypes.Find(idProduct);
                string message = "Вы хотите удалить категорию: " + pr.ToString() + "?";
                string caption = "Подтверждение выбора";
                var result = MessageBox.Show(message, caption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // пытаемся удалить ...
                    if (pr.Products.Count()==0)
                    {
                        db.context.ProductTypes.Remove(pr);
                        db.context.SaveChanges();
                        MessageBox.Show("Категория: " + pr.ToString() + " успешно удалена!");

                        dataGridView2.DataSource = null;
                        dataGridView2.DataSource = db.context.ProductTypes.ToList();
                    }
                    else MessageBox.Show("В удалении отказано!");
                }

            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            var k = db.context.Clients.Select(p => new ClientDG
            {
                ClientName = p.ClientName,
                Address = p.Address,
                Phone = p.Phone,
                UserID = p.UserID
            });


            // List<Client> cl = db.context.Clients.ToList();
            dataGridView1.DataSource = k.ToList();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["ClientName"].Value = db.Decrypt(row.Cells["ClientName"].Value.ToString());
                row.Cells["Address"].Value = db.Decrypt(row.Cells["Address"].Value.ToString());
                row.Cells["Phone"].Value = db.Decrypt(row.Cells["Phone"].Value.ToString());
            }
            //dataGridView1.DataSource = db.context.Clients.ToList();
        }
        int idclient = -1;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            idclient = Convert.ToInt32(dataGridView1.CurrentRow.Cells["UserID"].Value);
            Client cl = db.context.Clients.Where(p => (p.UserID == idclient)).FirstOrDefault();
            label3.Text = "Выбран клиент: " + db.Decrypt(cl.ClientName) + " (" + cl.UserID + ").";
            dataGridView4.DataSource = null;
            dataGridView4.DataSource = cl.OrderPCs.ToList();
            label4.Text = "Выбран клиент: " + db.Decrypt(cl.ClientName) + " (" + cl.UserID + ").";
        }

        int idOrd = -1;
        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            idOrd = Convert.ToInt32(dataGridView4.CurrentRow.Cells["orderIDDataGridViewTextBoxColumn"].Value);
            OrderPC or = db.context.OrderPCs.Where(p => (p.OrderID == idOrd)).FirstOrDefault();
            dataGridView5.DataSource = null;
            dataGridView5.DataSource = or.OrderDetails.ToList();

            
            if (e.ColumnIndex == 3)
            {
                string message = "Вы хотите удалить заказ № " + or.OrderID + "?";
                string caption = "Подтверждение выбора";
                var result = MessageBox.Show(message, caption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    foreach (var line in or.OrderDetails.ToList())
                    {
                        or.OrderDetails.Remove(line);
                    };
                    db.context.SaveChanges();

                    db.context.OrderPCs.Remove(or);
                    db.context.SaveChanges();
                    MessageBox.Show("Заказ №: " + idOrd + " успешно удален!");

                    if (idclient == -1)
                    {
                        dataGridView4.DataSource = null;
                        dataGridView5.DataSource = null;
                        dataGridView4.DataSource = db.context.OrderPCs.ToList();
                    }
                    else
                    {
                        dataGridView4.DataSource = null;
                        dataGridView5.DataSource = null;
                        Client cl = db.context.Clients.Where(p => (p.UserID == idclient)).FirstOrDefault();
                        dataGridView4.DataSource = cl.OrderPCs.ToList();
                    }

                   
                }
            }
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int idOrderPr = Convert.ToInt32(dataGridView5.CurrentRow.Cells["OrderIDProduct"].Value);
            int idProductOr = Convert.ToInt32(dataGridView5.CurrentRow.Cells["ProductID"].Value);
            OrderDetail orDet = db.context.OrderDetails.Where(p => ((p.ProductID == idProductOr)&&(p.OrderID == idOrderPr))).FirstOrDefault();


            if (e.ColumnIndex == 5)
            {
                string message = "Вы хотите отправить товар?";
                string caption = "Подтверждение выбора";
                var result = MessageBox.Show(message, caption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    orDet.Total = "готов";
                    Employee em = db.context.Employees.Where(p => (p.UserID == id)).FirstOrDefault();
                    orDet.Employee = em;
                    orDet.UserID = id;
                    db.context.SaveChanges();
                    OrderPC or = db.context.OrderPCs.Where(p => (p.OrderID == idOrd)).FirstOrDefault();
                    dataGridView5.DataSource = null;
                    dataGridView5.DataSource = or.OrderDetails.ToList();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView5.DataSource = null;
            label4.Text = "Выбран клиент: Все!";
            label3.Text = "Выбран клиент: Все!";
            idclient = -1;
            dataGridView4.DataSource = null;
            dataGridView4.DataSource = db.context.OrderPCs.ToList();
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            var k = db.context.Clients.Select(p => new ClientDG
            {
                ClientName = p.ClientName,
                Address = p.Address,
                Phone = p.Phone,
                UserID = p.UserID
            });


           // List<Client> cl = db.context.Clients.ToList();
            dataGridView1.DataSource = k.ToList();
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["ClientName"].Value = db.Decrypt(row.Cells["ClientName"].Value.ToString());
                row.Cells["Address"].Value = db.Decrypt(row.Cells["Address"].Value.ToString());
                row.Cells["Phone"].Value = db.Decrypt(row.Cells["Phone"].Value.ToString());
            }
        }
    }
}
