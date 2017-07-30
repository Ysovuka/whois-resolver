using System;
using System.Threading.Tasks;
using WhoIs.IANA;

namespace WhoIs.Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            string input = string.Empty;
            while (null != (input = System.Console.In.ReadLine()))
            {
                if (string.IsNullOrEmpty(input)) break;

                WHOISRequest request = new WHOISRequest();
                WHOISResponse response = await request.LookupAsync("google.com");
                System.Console.WriteLine(response.IANARecord.ToString());

                System.Console.WriteLine(" ");
                System.Console.WriteLine(" ");

                System.Console.WriteLine(response.WHOISRecord.ToString());
            }
            System.Console.WriteLine("Press any key to quit...");
            System.Console.ReadLine();
        }
    }
}