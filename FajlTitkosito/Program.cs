namespace FajlTitkosito
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fájl titkosító");
            FileEncryptor enc = new FileEncryptor("ner_szotar.txt","Titok_12");

            //D:\rud\c#_projects_git\encdec\FajlTitkosito\bin\Debug\net7.0\titkos_ner_szotar.rzt
            //FileDecryptor dec = new FileDecryptor("titkos_net_szotar.rzt", "Titok_12");
            FileDecryptor dec = new FileDecryptor(@"D:\rud\c#_projects_git\encdec\FajlTitkosito\bin\Debug\net7.0\titkos_ner_szotar.rzt", "Titok_12");




            Console.ReadKey();

        }
    }
}
