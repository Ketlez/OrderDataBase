using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderDataBase
{
    

    class OrderDB
    {
        public OrderContext context;
        public OrderDB()
        {
            context = new OrderContext();
        }

        public UserStatu FindEmployee(string login, string password)
        {
            
            UserStatu user =  context.UserStatus.Where(p => ((p.UserName == login)&&(p.Password == password))).FirstOrDefault();
            if (user == null)
                return null;
            return user;
        }

        public string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
        public void CreateClient
        (string login, 
         string FIO,
         string adress,
         string Phone)
        {
            OrderContext ctx = new OrderContext();
            UserStatu user = ctx.UserStatus.Where(p => (p.UserName == login)).FirstOrDefault();
            Client cl = new Client();
            cl.UserID = user.UserID;
            cl.ClientName = Encrypt(FIO);
            cl.Address = Encrypt(adress);
            cl.Phone = Encrypt(Phone);
            context.Clients.Add(cl);
            context.SaveChanges();
        }
        public bool CreateUser
        (string login, 
         string password, 
         string FIO,
         string adress,
         string Phone)
        {
            UserStatu user = context.UserStatus.Where(p => (p.UserName == login)).FirstOrDefault();
            if (user != null)
            {
                return false; // вставка неудачна
            }
            else
            {
                UserStatu us = new UserStatu() ;
                us.UserName = login;
                us.Password = GetHash(password);
                context.UserStatus.Add(us);
                context.SaveChanges();
                CreateClient(login, FIO, adress, Phone);
                return true;
            }
        }


        // Формирование заказа
        public OrderPC AddOrder(int CustID, Cart myCart)
        {
            // Создаем и инициализируем новый заказ
            OrderPC o = new OrderPC();
            o.OrderData = DateTime.Today; // Текущая дата
            o.UserID = CustID; // Ид-р заказчика
            o.Price = Convert.ToInt32(myCart.GetTotal());
            context.OrderPCs.Add(o); // Добавляем заказ в сущность
                              // Проходим по строкам корзины и добавляем их в детали заказа
            foreach (var line in myCart.GetLines())
            {
                // Создаем и инициализируем новую позицию в заказе
                OrderDetail od = new OrderDetail();
                Product pr = context.Products.Find(line.ProdID);
                pr.Count -= line.Quantity;
                od.ProductID = line.ProdID;
                od.Total = "ожидает";
                od.Count = line.Quantity;
                // Через навигационное свойство добавляем позицию в заказ
                o.OrderDetails.Add(od);
            };
            
        context.SaveChanges();
            // Возвращаем новый вставленный заказ
            return o;
        }
        public OrderPC CommitTrans(int custID, Cart myCart, out string message)
        {
            message = "Транзакция прошла успешно";
            // Запускаем транзакцию
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    if (myCart.GetTotal() == 0)
                    {
                        throw new ApplicationException("Вы забили заполнить корзину!");
                    }
                    // Сохраняем изменения во всех таблицах
                    OrderPC o = AddOrder(custID, myCart);
                    // Фиксируем транзакцию
                    trans.Commit();
                    return o;
                }
                catch (Exception ex)
                {
                    // Откатывем транзакцию
                    message = "Транзакция откатилась со следующей ошибкой: " +
                    ex.Message;
                    trans.Rollback();
                    return null;
                }
                finally
                {
                    
                    context.Dispose();
                    context = new OrderContext();
                }
            }
        }

        public bool AddProductType(string type)
        {
            ProductType typeprod = context.ProductTypes.Where(p => (p.Type == type)).FirstOrDefault();
            if (typeprod != null)
            {
                return false; // вставка неудачна
            }
            else
            {
                ProductType t = new ProductType();
                t.Type = type;
                context.ProductTypes.Add(t);
                context.SaveChanges();
                return true;
            }

        }

        public bool AddProduct(int idprod, string name, int price, int count)
        {
            if(idprod == -1)
                return false;
            Product prodT = context.Products.Where(p => (p.Name == name)).FirstOrDefault();
            if (prodT != null)
            {
                prodT.Count += count;
                return true; 
            }
            else
            {
                Product p = new Product();
                p.ProductTypeID = idprod;
                p.Name = name;
                p.Price = price;
                p.Count = count;
                context.Products.Add(p);
                context.SaveChanges();
                return true;
            }

        }

        private static string key = "<RSAKeyValue><Modulus>zULfP5Y7amoGxrKu9uCVrV38f1u1ga2yECWNS1JaJOuOi9OZLoKZFq0U1GdGL0t7jFG+3U+TPELP6j8aIkwIxKNyT0NquPTV4FXpCsseqz6reTuQy7XQYF1zvL2ApzeW73bulYw/uouur7EVddnt+0v12kZ52+iL/B5avqhm8r0=</Modulus><Exponent>AQAB</Exponent><P>6AcGsBuDgITwZj2ieAMH1C+z2MSP/mdY1OP88xZT0PGFLRLnDy0E7hPNJ7V7yJLA0Yytra4u3mfwu03XQUepCw==</P><Q>4nflk4n7/439DMXWZ1WZmJGL/rvcnf8ZDqirWy7qt8g2GIMMP9SUdYgkTfTPXzZiJmwbMuXU8ECLQhyYt/WAVw==</Q><DP>rRGwl2Oubwq6FkkbCtGX4VnDmIjlryl/RSzZ3Khm1I+SetCCsPsvljYG7Pud3To5wRRh6A7ovtRg6BVj3jmJiQ==</DP><DQ>PDhrqM7xXqRQHNxixfmiLUrOsj8cTDswW5CIeGfCbHplwCDg2fxaOeKo3L3zgrsAYH0wwlkRRY20OjFGfuxeYw==</DQ><InverseQ>TdUDQgafbkTsbx1aTZwGXdAvXj71LVAtlCShdLkde40bGfBhEF8llfNhPtWdnCRIaPuLHDTc+x/4vKAbAM7RSw==</InverseQ><D>K9UK7X48Y+YOWmIP4OJmtCXs5JmF8hJQgwgx2xLT8yxmPU/LV1ZGMMR3PUBsiW76DCXstz/l9iliUuh0wTwxZsKDfvGVgUaA1vNDCN0rZi9wcB8BK/22VFmt0fxK2Hj2/c1H2yVu3g+7qOnxYaW3HocY9J0o7vPgVB00k+jt6HU=</D></RSAKeyValue>";
        private static string _toString(byte[] p)
        {
            return Encoding.UTF8.GetString(p);
        }
        private static byte[] _toByte(string p)
        {
            return Encoding.UTF8.GetBytes(p);
        }
        public string Decrypt(string p)
        {
            byte[] decrContent;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            decrContent = rsa.Decrypt(Convert.FromBase64String(p), true);

            return _toString(decrContent);
        }
        public string Encrypt(string p)
        {
            byte[] encContent;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            encContent = rsa.Encrypt(_toByte(p), true);

            return Convert.ToBase64String(encContent);
        }
    }
}

