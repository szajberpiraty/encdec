namespace FajlTitkosito
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Fájl titkosító");
            FileEncryptor enc = new FileEncryptor("ner_szotar.txt","Titok_12");


            Console.ReadKey();

        }
    }
}
