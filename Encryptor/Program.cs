
using System.Security.Cryptography;
using System.Text;
namespace Encryptor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aes aes = Aes.Create();

            string kulcs = "Titok_21";
            string titkos = "Titkos tartalom";



            byte[] tartalom = new UTF8Encoding().GetBytes(titkos);

            byte[] key = GenerateKey(kulcs);
            //key=new UTF8Encoding().GetBytes(kulcs);
            //byte[] iv = new byte[16];
            //byte[] iv=new UTF8Encoding().GetBytes("Fika");

            
            aes.Key = key;
            //aes.IV = iv;
            aes.GenerateIV();
            byte[] iv = aes.IV;

            ICryptoTransform encryptor= aes.CreateEncryptor(key,iv);

            byte[] encryptedData = encryptor.TransformFinalBlock(tartalom, 0, tartalom.Length);

            string encString=Convert.ToBase64String(encryptedData);

            Console.WriteLine(encString);


            Console.ReadKey();

        }
        static byte[] GenerateKey(string kulcsText)
        {
            using (SHA256 sha256=SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(kulcsText));

                return data;
            }
        }
    }
}
