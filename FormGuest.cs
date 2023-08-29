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
    public partial class FormGuest : Form
    {
        
        public FormGuest()
        {
            this.FormClosing += closingCallback;
            InitializeComponent();
        }

        public void closingCallback(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CurrentForm.f == this)
                CurrentForm.f = null;
        }

        OrderContext context = new OrderContext();

        private void FormGuest_Load(object sender, EventArgs e)
        {
            cbProductType.DisplayMember = "Type"; // Это свойство показывается
            cbProductType.ValueMember = "ProductTypeID"; // Это свойство выбирается
                                                  // привязка и загрузка данных
            cbProductType.DataSource = context.ProductTypes.ToList();

        }

        private void cbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductType t = (ProductType)cbProductType.SelectedItem;
            List<Product> prod = t.Products.OrderBy(o=>o.Price).ToList();
            dgCatalog.DataSource = prod.ToList();
            
        }

        private void dgCatalog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentForm.f = new FormMain();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProductType t = (ProductType)cbProductType.SelectedItem;
            dgCatalog.DataSource = null;
            dgCatalog.DataSource = t.Products.Where(p => (p.Name.Contains(textBox1.Text))).ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
