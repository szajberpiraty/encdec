using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Encryptor2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Aes aes = Aes.Create();

            string kulcs = "Titok_21";
            string ivText = "Qrffmoi56Klp";
            string titkos = "Titkos tartalom, ez tényleg szigorúan titkos!";


            byte[] tartalom = new UTF8Encoding().GetBytes(titkos);
            byte[] binKulcs = GenerateKey(kulcs);
            byte[] binIv = GenerateKey16(ivText);
            byte[] binkulcsMeret = new byte[4];


            int valamiertek = 233456;
            byte[] valamiertekbytes=BitConverter.GetBytes(valamiertek);
            Buffer.BlockCopy(valamiertekbytes,0,binkulcsMeret,0,valamiertekbytes.Length);

            int visszaallit = BitConverter.ToInt32(valamiertekbytes);

            Console.WriteLine($"Bin tömbből érték:{visszaallit}");


            Console.WriteLine($"Kulcs mérete tárolva ennyi byte-on:{BitConverter.ToInt32(binkulcsMeret)}");


            Console.WriteLine($"Bin kulcs méret:{binkulcsMeret.Length}");



            aes.Key = binKulcs;
            aes.GenerateIV();
            //aes.IV = binIv;
            byte[] iv = aes.IV;
            
            

            ICryptoTransform encryptor = aes.CreateEncryptor(binKulcs, iv);

            byte[] encryptedData = encryptor.TransformFinalBlock(tartalom, 0, tartalom.Length);

            string encString = Convert.ToBase64String(encryptedData);
            string encString2=Encoding.UTF8.GetString(encryptedData);


            Console.WriteLine($"Kódolt szöveg Base64:{encString}");
            Console.WriteLine($"Kódolt szöveg utf8 getstring:{encString2}");

            SHA256 sha256 = SHA256.Create();
            byte[] tartalomHash = sha256.ComputeHash(tartalom);

            Console.WriteLine(encString);
            Console.WriteLine($"IV hossz:{iv.Length}");
            Console.WriteLine($"Saját IV hossz:{binIv.Length}");
            Console.WriteLine($"Kulcs tömb hossz:{binKulcs.Length}");
            Console.WriteLine($"BinIV hossz:{binIv.Length}");
            Console.WriteLine($"AES key size:{aes.KeySize}");
            Console.WriteLine($"Tartalom hash:{ByteToHash(tartalomHash)}");
            Console.WriteLine($"Encrypted tomb hossz:{encryptedData.Length}");


            byte[] ivHashEncData=new byte[iv.Length+tartalomHash.Length+encryptedData.Length];

            Buffer.BlockCopy(iv,0,ivHashEncData,0,iv.Length);
            Buffer.BlockCopy(tartalomHash,0,ivHashEncData,iv.Length,tartalomHash.Length);
            Buffer.BlockCopy(encryptedData, 0, ivHashEncData, tartalomHash.Length, encryptedData.Length);

            Console.WriteLine($"Egyesitett tomb hossz:{ivHashEncData.Length}");

            File.WriteAllBytes("titok.rac",ivHashEncData);


            ICryptoTransform decryptor = aes.CreateDecryptor(binKulcs,iv);

            byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData,0,encryptedData.Length);

            string decString = Encoding.UTF8.GetString(decryptedData);
            
            //Nem használható a dekódoláshoz
            //string decString2 = Convert.ToBase64String(decryptedData);

            
            Console.WriteLine($"Dekódolt szöveg:{decString}");
            //Console.WriteLine($"Dekódolt szöveg:{decString2}");


        }

        static byte[] GenerateKey(string kulcsText)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(kulcsText));

                return data;
            }
        }

        static byte[] GenerateKey16(string kulcsText)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(kulcsText));

                return data;
            }
        }

        private static string ByteToHash(byte[] data)
        {
            StringBuilder hash = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                hash.Append(data[i].ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
