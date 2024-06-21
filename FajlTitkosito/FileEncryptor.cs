using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace FajlTitkosito
{
    public class FileEncryptor
    {
        //fájl felépítés: verzió|iv méret|fájlnév méret|hash méret|fájl adat méret|IV|fájlnév|hash|adat

        //Hash
        SHA512 sha512 = SHA512.Create();

        //Encryptor
        Aes aes = Aes.Create();
        

        //bináris adatmezők

        //Fájlban eltároljuk
        private byte[] versionBin;       
        private byte[] ivSizeBin;
        private byte[] fileNameSizeBin;
        private byte[] fileHashSizeBin;
        private byte[] fileEncodedDataSizeBin;
        private byte[] ivBin;
        private byte[] fileNameBin;
        private byte[] fileHashBin;       
        private byte[] fileEncodedDataBin;


        //Nem kellenek
        //Nem tároljuk fájlban
        //private byte[] keySizeBin;
        //Nem tároljuk fájlban
        //private byte[] keyBin;
        
        
        private byte[] allDataBin;





        public FileEncryptor(string fajl,string jelszo)
        {
            var fajlNev = fajl.Split('\\').Last();
            //betöltjük a fájlt egy byte tömbbe
            byte[] fileData=File.ReadAllBytes(fajl);

            //Kulcs generálás
            
            
            aes.Key = GenerateKey256(jelszo);
            Console.WriteLine(aes.KeySize);
                                    
            //IV generálás
            aes.GenerateIV();

            

            //Encryptor létrehozása
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            //fájl adatok titkosítása
            //Ezt majd using-ba
            fileEncodedDataBin = encryptor.TransformFinalBlock(fileData, 0, fileData.Length);

            //byte adatmezők feltöltése
            ivBin = aes.IV;
            fileNameBin = DataToBin(fajlNev);
            fileHashBin = DataToBin(FileHash(fileData));
            

            //byte méretek feltöltése
            versionBin = DataToBin(1);
            ivSizeBin = DataToBin(aes.IV.Length);
            fileNameSizeBin = DataToBin(fileNameBin.Length);
            fileHashSizeBin = DataToBin(fileHashBin.Length);
            fileEncodedDataSizeBin = DataToBin(fileEncodedDataBin.Length);





            //Adatok kiírása

            AllDataWrite();

            //Adatok fájlba írása

            File.WriteAllBytes("titkos_ner_szotar.rzt", allDataBin);

        }

        private void AllDataWrite()
        {
            //fájl felépítés: verzió|iv méret|fájlnév méret|hash méret|fájl adat méret|IV|fájlnév|fájlhash|adat
            allDataBin = new byte[CalculateAllDataSize()];
            using (MemoryStream ms=new MemoryStream(allDataBin))
            {
                using (BinaryWriter wr=new BinaryWriter(ms))
                {
                    wr.Write(versionBin);
                    wr.Write(ivSizeBin);
                    wr.Write(fileNameSizeBin);
                    wr.Write(fileHashSizeBin);
                    wr.Write(fileEncodedDataSizeBin);
                    wr.Write(ivBin);
                    wr.Write(fileNameBin);
                    wr.Write(fileHashBin);
                    wr.Write(fileEncodedDataBin);

                }

            }
        }

        private byte[] GenerateKey(string kulcsText)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] data = sha512.ComputeHash(Encoding.UTF8.GetBytes(kulcsText));

                return data;
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
            using (SHA512 sha512=SHA512.Create() )
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

        private int CalculateAllDataSize()
        {
            return versionBin.Length+                                    
                   ivSizeBin.Length+
                   fileNameSizeBin.Length+
                   fileHashSizeBin.Length+
                   fileEncodedDataSizeBin.Length +
                   ivBin.Length+
                   fileNameBin.Length+
                   fileHashBin.Length+
                   fileEncodedDataBin.Length;
        }
    }
}
