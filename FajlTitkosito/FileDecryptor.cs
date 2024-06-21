using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FajlTitkosito
{
    public class FileDecryptor
    {

        //Fájlból betöltjük
        private byte[] versionBin;
        private byte[] ivSizeBin;
        private byte[] fileNameSizeBin;
        private byte[] fileHashSizeBin;
        private byte[] fileEncodedDataSizeBin;
        private byte[] ivBin;
        private byte[] fileNameBin;
        private byte[] fileHashBin;
        private byte[] fileEncodedDataBin;



        private byte[] decodedDataBin;
        private byte[] allDataBin;


        public FileDecryptor(string fajl,string jelszo)
        {
            //betöltjük a titkosított fájlt, fejlécestől

            if (File.Exists(fajl))
            {
                allDataBin = File.ReadAllBytes(fajl);
            } else
            {
                Console.WriteLine($"{fajl} nem található");
            }

            ReadDecryptedFile();

            //kulcsot generálunk

            //kiolvassuk az adatmezőket

            //dekódoljuk a fájlt


        }

        private void ReadDecryptedFile()
        {
            using (MemoryStream ms=new MemoryStream(allDataBin))
            {
                using (BinaryReader br=new BinaryReader(ms))
                {
                    byte[] version = br.ReadBytes(4);
                    byte[] ivSizeBin = br.ReadBytes(4);
                    byte[] fileNameSizeBin = br.ReadBytes(4);
                    byte[] fileHashSizeBin = br.ReadBytes(4);
                    byte[] fileEncodedDataSizeBin = br.ReadBytes(4);

                    Console.WriteLine(BitConverter.ToInt32(version));
                    Console.WriteLine(BitConverter.ToInt32(ivSizeBin));
                    Console.WriteLine(BitConverter.ToInt32(fileNameSizeBin));
                    Console.WriteLine(BitConverter.ToInt32(fileHashSizeBin));
                    Console.WriteLine(BitConverter.ToInt32(fileEncodedDataSizeBin));

                }

            }
        }

        private byte[] GenerateKey256(string kulcsText)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(kulcsText));

                return data;
            }
        }

        private string FileHash(byte[] data)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hash = sha512.ComputeHash(data);

                return Encoding.UTF8.GetString(hash);
            }
        }

        private byte[] DataToBin(int data)
        {
            return BitConverter.GetBytes(data);
        }

        private byte[] DataToBin(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

    }
}
