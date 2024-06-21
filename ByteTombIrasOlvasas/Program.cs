using System.IO;

using System.Text;

namespace ByteTombIrasOlvasas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string tartalom = "Valami hosszabb string változó";
            string initVektor = "Ezzel a szöveggel helyettesítem";

            byte[] tartalomBin=new UTF8Encoding().GetBytes(tartalom);
            byte[] initBin = new UTF8Encoding().GetBytes(initVektor);

            Console.WriteLine(tartalomBin.Length);
            Console.WriteLine(initBin.Length);

            //4 byte-os tömböket csinál
            byte[] tartalomHosszBin = BitConverter.GetBytes(tartalomBin.Length);

            //4 byte-os tömböket csinál
            byte[] initHosszBin = BitConverter.GetBytes(initBin.Length);

            
            byte[] meretek = new byte[tartalomHosszBin.Length+initHosszBin.Length];

            Buffer.BlockCopy(tartalomHosszBin, 0, meretek,0, tartalomHosszBin.Length);

            Buffer.BlockCopy(initHosszBin, 0, meretek, tartalomHosszBin.Length, initHosszBin.Length);

            Console.WriteLine($"Beírva{tartalomHosszBin.Length},{initHosszBin.Length}");


           

            byte[] osszAdat= new byte[meretek.Length+tartalomBin.Length+initBin.Length];

            Buffer.BlockCopy(meretek,0,osszAdat,0,meretek.Length);
            Buffer.BlockCopy(tartalomBin, 0, osszAdat, meretek.Length, tartalomBin.Length);
            //ez a jó
            Buffer.BlockCopy(initBin, 0, osszAdat, meretek.Length+tartalomBin.Length, initBin.Length);

            //Itt elcsúszik a tartalom, a hibás offset miatt
            //Buffer.BlockCopy(initBin, 0, osszAdat, tartalomBin.Length, initBin.Length);

            //Visszanyerjük a méreteket, majd azok értéke szerint olvassuk vissza az adatokat.

            //visszanyerjük az adatokat
            byte[] vTartalom = new byte[4];
            byte[] vInit = new byte[4];

            Buffer.BlockCopy(osszAdat, 0, vTartalom, 0, vTartalom.Length);
            Buffer.BlockCopy(osszAdat, vTartalom.Length, vInit, 0, vInit.Length);

            Console.WriteLine($"Tartalom hossza:{BitConverter.ToInt32(vTartalom)},Init hossza:{BitConverter.ToInt32(vInit)}");
            int initMeret = BitConverter.ToInt32(vInit);
            int tartalomMeret = BitConverter.ToInt32(vTartalom);

            byte[] eredetiTartalom = new byte[tartalomMeret];
            byte[] eredetiInit = new byte[initMeret];

            //Kiolvassuk a szöveges adatokat
            Buffer.BlockCopy(osszAdat, vTartalom.Length + vInit.Length, eredetiTartalom,0, tartalomMeret);
            Buffer.BlockCopy(osszAdat, vTartalom.Length + vInit.Length + tartalomMeret,eredetiInit, 0, initMeret);

            Console.WriteLine($"Eredeti szöveg:{Encoding.UTF8.GetString(eredetiTartalom)}");
            Console.WriteLine($"Eredeti initvektor:{Encoding.UTF8.GetString(eredetiInit)}");

            using (MemoryStream ms=new MemoryStream(osszAdat))
            {
                using (BinaryReader binReader=new BinaryReader(ms))
                {
                    byte[] tartMeret = binReader.ReadBytes(4);
                    byte[] inMeret = binReader.ReadBytes(4);
                    Console.WriteLine($"Tart:{BitConverter.ToInt32(tartMeret)}"); 
                    Console.WriteLine($"Init:{BitConverter.ToInt32(inMeret)}");
                }
            }
            byte[] osszadat2 = new byte[8];

            using (MemoryStream ms=new MemoryStream(osszadat2))
            {
                using (BinaryWriter binWriter=new BinaryWriter(ms))
                {

                    binWriter.Write(112);
                    binWriter.Write(556);
                    


                }

                
            }

            using (MemoryStream ms = new MemoryStream(osszadat2))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    byte[] szam1 = br.ReadBytes(4);
                    byte[] szam2 = br.ReadBytes(4);

                    Console.WriteLine($"Szam1:{BitConverter.ToInt32(szam1)}");
                    Console.WriteLine($"Szam2:{BitConverter.ToInt32(szam2)}");

                }

            }

        }
    }
}
