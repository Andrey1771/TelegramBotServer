﻿using System;

namespace Lab_9
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            //bot = new TelegramBotClient(token);
            //bot.OnMessage += BotListener;

            BillyTelegramBot bot = new BillyTelegramBot(@"E:\Visual Projects\Skillbox\Lab_9\BillyContent",
               @"C:\Users\Andrey\Desktop\BillyToken.txt", @"C:\Users\Andrey\Desktop\GoogleToken.txt", @"E:\Visual Projects\Skillbox\Lab_9\BillyContent\usersData.json");
            bot.StartBot();

            Console.ReadKey();
        }

        
    }
}
