using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace OrderDataBase
{
    public partial class FormPassword : Form
    {
        public FormPassword()
        {
            InitializeComponent();
        }

        private string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);    
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
        private string Decrypt(string p)
        {
            byte[] decrContent;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            decrContent = rsa.Decrypt(Convert.FromBase64String(p), true);

            return _toString(decrContent);
        }
        private string Encrypt(string p)
        {
            byte[] encContent;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            encContent = rsa.Encrypt(_toByte(p), true);

            return Convert.ToBase64String(encContent);
        }
        private void button1_Click(object sender, EventArgs e)
        {

            //textBox2.Text = GetHash(textBox1.Text);
            textBox2.Text = Decrypt(textBox1.Text);
        }
    }
}
