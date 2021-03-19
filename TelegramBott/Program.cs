using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using HtmlAgilityPack;
using System.Net;

namespace TelegramBott
{
    class Program
    {
       public static List<string> Subscribers = new List<string> {};
       // string[] Subscribers;

        static ITelegramBotClient botClient;
        static async Task Main(string[] args)
        {
            

            Calistir();
            

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
        }

        public static string sonLik = "";
        public static string mesaj = "";

        static bool dondur = false;


        public static async void Calistir()
        {
            while (true)
            {

                Uri url = new Uri("https://www.euronews.com/just-in");



                WebClient client = new WebClient();
                string html = client.DownloadString(url);

                HtmlDocument dokuman = new HtmlDocument();
                dokuman.LoadHtml(html);

                HtmlNodeCollection node = dokuman.DocumentNode.SelectNodes("/html/body/main/section[2]/div[2]/ul/li[1]/article/div[2]/h3/a");




                

                foreach (var item in node)
                {
                    //item.InnerText != sonLik
                    if (item.InnerText != sonLik)
                    {
                        Console.WriteLine(item.InnerText);
                        var link = dokuman.DocumentNode.SelectNodes("/html/body/main/section[2]/div[2]/ul/li[1]/article/div[2]/h3/a").LastOrDefault();
                        var href = link.Attributes["href"].Value;

                        mesaj = item.InnerText + "\n" + "https://www.euronews.com" + href;

                        foreach (var sub in Subscribers)
                        {
                            botClient.SendTextMessageAsync(sub, mesaj);
                        }


                        sonLik = item.InnerText;

                        


                    }
                    else
                    {
                        
                    }
                }





            }



        }


        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == "!Subscribe")
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                if (!Subscribers.Contains(Convert.ToString(e.Message.Chat.Id)))
                {
                    Subscribers.Add(Convert.ToString(e.Message.Chat.Id));
                }

                foreach (var item in Subscribers)
                {
                    Console.WriteLine(item);
                }

                

                await botClient.SendTextMessageAsync(
                   e.Message.Chat,
                   "Hello! You have signed up to our news list \n Thank you!"
                );
            }


           
            

        }
    }
}
