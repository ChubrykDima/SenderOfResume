using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SenderOfResume.Email;
using System.Threading;

namespace SenderOfResume.Parsing
{
    class Parser
    {
        readonly string companiesUrl = "https://companies.dev.by";

        private string[] adress;

        public string GetHtmlPage()
        {
            var page = File.ReadAllText(@"c:\devby");
            return page;
        }

        public async Task<string> GetHtmlPageEmail(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                return await response.Content.ReadAsStringAsync();
            }                
        }


        public async Task<string[]> ParseLinks()
        {
            var list = new List<string>();
            await Task.Run(() =>
            {
                var parser = new HtmlParser();
                var html = GetHtmlPage();
                var document = parser.Parse(html);
                var source = document.QuerySelectorAll("tr").Where(item => (item.ClassName != null && item.ClassName == "odd") || (item.ClassName == "even" && item.ClassName != null));
                var result = source.Select(x => x.GetElementsByTagName("a").FirstOrDefault().GetAttribute("href").ToString()).ToList();

                foreach (var item in result)
                {
                    list.Add(item);
                }

            });
            return list.ToArray();
        }

        public static async void ParseEmail(string html)
        {
            try {
                Letter letter = new Letter();
                await Task.Run(() =>
                {
                    var parser = new HtmlParser();

                    var document = parser.Parse(html);
                    var source = document.QuerySelectorAll("a").Where(item => item.Attributes["href"] != null && item.ClassName == "email" && item.ClassName != null).Select(x => x.Attributes["href"].Value).ToList();
                    foreach (var item in source)
                    {
                        var items = item.Substring(7);
                        if (!String.IsNullOrEmpty(items) && items.Contains("@"))
                        {
                            Console.WriteLine(items + " - резюме отправлено");                           
                            letter.SendEmail(items);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка:" + ex.Message);
                throw;
            }
        }


        public async void Worker()
        {
            adress = await ParseLinks();

            foreach (var item in adress)
            {
                var url = companiesUrl + item;
                var html = await GetHtmlPageEmail(url);

                ParseEmail(html);
            }
            Thread.Sleep(500);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Отправка резюме завершена");
        }
    }
}
