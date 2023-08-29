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
    public partial class FormUser : Form
    {
        public int id;
        public FormUser(int id)
        {
            this.id = id;
            this.FormClosing += closingCallback;
            InitializeComponent();
        }
        Cart cart = new Cart();
        OrderDB db = new OrderDB();

        public void closingCallback(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CurrentForm.f == this)
                CurrentForm.f = null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            comboBox1.DisplayMember = "Type";
            comboBox1.ValueMember = "ProductType";
            comboBox1.DataSource = db.context.ProductTypes.ToList();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            ProductType t = (ProductType)comboBox1.SelectedItem;
            dataGridView1.DataSource = t.Products.ToList();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView2.DataSource = cart.GetLines();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = cart.GetLines();
            label2.Text = "Сумма: " + cart.GetTotal()+"р.";
            dataGridView3.DataSource = null;
            Client clien = db.context.Clients.Where(p => (p.UserID == id)).FirstOrDefault();
            dataGridView3.DataSource = clien.OrderPCs.ToList();
            

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string name = dataGridView1.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString();
            int price = Convert.ToInt32(dataGridView1.CurrentRow.Cells["priceDataGridViewTextBoxColumn"].Value);
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["productIDDataGridViewTextBoxColumn"].Value);
            string prodtype; 
            
            if (e.ColumnIndex == 4)
            {
            
                ProductType t = (ProductType)comboBox1.SelectedItem;
                prodtype = t.Type;
                Product pr = db.context.Products.Find(id);

                if ((pr.Count) >= (cart.GetCount(id)+1))
                {
                    cart.AddItem(id, prodtype, name, price, 1);
                    string message = "Товар добавлен в корзину!";
                    var result = MessageBox.Show(message);
                }
                else
                {
                    string message = "Ошибка: Товаров больше нет!";
                    var result = MessageBox.Show(message);
                }
            }
            
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView2.CurrentRow.Cells["PrID"].Value);
            if (e.ColumnIndex == 5)
            {
                cart.RemoveLine(id);
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = cart.GetLines();
                label2.Text = "Сумма: " + cart.GetTotal() + "р.";
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            cart.Clear();
            label2.Text = "Сумма: " + cart.GetTotal() + "р.";
            dataGridView2.DataSource = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string message;
            OrderPC o = db.CommitTrans(id, cart, out message);
            MessageBox.Show(message);
        }
        int idorder = -1;
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            idorder = Convert.ToInt32(dataGridView3.CurrentRow.Cells["orderIDDataGridViewTextBoxColumn"].Value);
            OrderPC ord = db.context.OrderPCs.Where(p => (p.OrderID == idorder)).FirstOrDefault();
            dataGridView4.DataSource = null;
            dataGridView4.DataSource = ord.OrderDetails.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CurrentForm.f = new FormMain();
            this.Close();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            

            ProductType t = (ProductType)comboBox1.SelectedItem;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = t.Products.Where(p => (p.Name.Contains(textBox1.Text))).ToList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (FormCheng frm = new FormCheng(id))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    Client cl = db.context.Clients.Where(p => (p.UserID == id)).FirstOrDefault();
                    cl.ClientName = db.Encrypt(frm.textBox1.Text);
                    cl.Address = db.Encrypt(frm.textBox2.Text);
                    cl.Phone = db.Encrypt(frm.textBox3.Text);
                    db.context.SaveChanges();
                }
            }
        }
    }
}
