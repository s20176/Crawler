using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler
{
    class Program
    {
        public async static Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentNullException("Nie przekazano argumentu");
            }
            string websiteUrl = args[0];

            var regex = new Regex(@"^(http|https)[:][/][/][a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+");
            var match=regex.Match(websiteUrl);
            if (!match.Success)
            {
                throw new ArgumentException("Przekazano niepoprawny adres url");
            }
            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(websiteUrl);
                
                Console.WriteLine(response);

                var content = await response.Content.ReadAsStringAsync();

                regex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+");

                var matchCollection = regex.Matches(content);
                var set = new HashSet<string>();

                foreach (Match m in matchCollection)
                {
                    set.Add(m.ToString());
                }

                if (set.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono adresów email");
                }
                else
                {
                    foreach (string s in set)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }
            httpClient.Dispose();

        }
    }
}