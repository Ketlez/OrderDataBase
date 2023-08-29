using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDataBase
{
    public class Cart
    {
        public Cart() { }
        // Строка (элемент) корзины
        public class CartLine
        {
            public int ProdID { get; set; }
            public string ProdType { get; set; }
            public string ProdName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
        // Содержание корзины - коллекция строк CartLine
        private List<CartLine> lineCollection = new List<CartLine>();
        // Добавление элемента в корзину
        public void AddItem(int prodID,
        string prodType,
        string prodName,
        decimal price,
        int quantity)
        {
            // Ищем в корзине товар с данным 
            // идентификатором
            CartLine line = lineCollection.Where
            (p => p.ProdID == prodID).FirstOrDefault();
            
       
            // Если его нет, то добавляем в корзину
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    ProdID = prodID,
                    ProdType = prodType,
                    ProdName = prodName,
                    Price = price,
                    Quantity = quantity
                });
            }
            else
            // иначе увеличиваем количество 
            {
                line.Quantity += quantity;
            }
        }
        // Удаление из корзины
        public void RemoveLine(int prodID)
        {
            lineCollection.RemoveAll(l => l.ProdID == prodID);
        }
        //Сумма к оплате
        public decimal GetTotal()
        {
            return lineCollection.Sum(e => e.Price * e.Quantity);
        }
        // Очистка корзины
        public void Clear()
        {
            lineCollection.Clear();
        }
        // Возврат содержимого корзины
        public List<CartLine> GetLines()
        {
            return lineCollection;
        }

        public int GetCount(int id)
        {
            CartLine line = lineCollection.Where(l => l.ProdID == id).FirstOrDefault();
            if (line == null)
                return 0;
            return line.Quantity;
        }

        
        
        
        
    }

}
