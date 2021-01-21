using System;
using System.IO;
using System.Collections.Generic;
//using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;

using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;



namespace Lab_9
{
    public class BillyTelegramBot
    {
        TelegramBotClient bot;
        string telegramToken, googleToken;
        string pathToLoadFile;

        ///TODO
        string userSeceret;
        string userId;
        string userName;
        private Google.Apis.Drive.v2.Data.File InsertFile(DriveService service, String title, String description, String parentId, String mimeType, String filename)
        {
            // File's metadata.
            Google.Apis.Drive.v2.Data.File body = new Google.Apis.Drive.v2.Data.File();
            body.Title = title;
            body.Description = description;
            body.MimeType = mimeType;

            // Set the parent folder.
            if (!String.IsNullOrEmpty(parentId))
            {
                body.Parents = new List<ParentReference>()
                {new ParentReference() {Id = parentId}};
            }

            // File's content.
            byte[] byteArray = System.IO.File.ReadAllBytes(filename);
            MemoryStream stream = new MemoryStream(byteArray);

            try
            {
                FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, mimeType);
                request.Upload();

                Google.Apis.Drive.v2.Data.File file = request.ResponseBody;

                // Uncomment the following line to print the File ID.
                // Console.WriteLine("File ID: " + file.Id);

                return file;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }
        ///TODO
        
        public BillyTelegramBot(string apathToLoadFile, string pathToTelegramToken, string pathToGoogleToken)
        {
/*            //https://i1.sndcdn.com/avatars-000411292317-dt2f28-t500x500.jpg
            string url = @"audio_api.mp3";
            WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 };
            webClient.BaseAddress = "https://pavelgrafit.github.io/#/";

            webClient.DownloadFile(@"img/webdesign.png", "webdesign.png");
            Console.WriteLine("Готово.");*/

            pathToLoadFile = apathToLoadFile;
            telegramToken = System.IO.File.ReadAllText(pathToTelegramToken);
            googleToken = System.IO.File.ReadAllText(pathToGoogleToken);

            #region exc

            //// https://hidemyna.me/ru/proxy-list/?maxtime=250#list

            // Содержит параметры HTTP-прокси для System.Net.WebRequest класса.
            var proxy = new WebProxy()
            {
                Address = new Uri($"http://88.198.50.103:8080"),
                UseDefaultCredentials = false,
                //Credentials = new NetworkCredential(userName: "login", password: "password")
            };

            // Создает экземпляр класса System.Net.Http.HttpClientHandler.
            var httpClientHandler = new HttpClientHandler() { /*Proxy = proxy*/ };

            // Предоставляет базовый класс для отправки HTTP-запросов и получения HTTP-ответов 
            // от ресурса с заданным URI.
            HttpClient hc = new HttpClient(httpClientHandler);

            bot = new TelegramBotClient(telegramToken, hc);

            #endregion

        }

        private void StartOnInlineQuery()
        {
            bot.OnInlineQuery += async (object si, Telegram.Bot.Args.InlineQueryEventArgs ei) =>
            {
                Console.WriteLine("StartOnInlineQuery");
                var query = ei.InlineQuery.Query;

                var msg = new Telegram.Bot.Types.InlineQueryResults.InputTextMessageContent(@"My fellow brothers, I, Billy Herrington, stand here today, humbled by the task before us, mindful of the sacrifices born by our nico Nico ancestors. We are in the midst of crisis. Nico Nico Doga is at war against a far reaching storm of disturbance and of leash. Nico Nico's economy is badly weakened, a consequence of carelessness and irresponsibility of the part of management, but also on the collective failure to make hard choices and prepare for a new, mad age. Today, I say to you that the challenge is real, they are serious, and there are many. They will not be easily met or in a short span of time, but know that at Nico Nico, they will be met. In reaffirming the greatness of our site, we understand that greatness is never given, our journey has never been one of shortcuts. It has not been for the path, for the feint hearted, or seek only the fleshly pleasures. Rather, it has been the risk takers, the wasted genie, the creators of mad things. For us, they toiled in sweatshops, endured the lash of the spanking, time and time again. These men struggled and sacrificed so that we might LIVE BETTER. We remain the most powerful sight on the Internet and minds are no less inventive and services were no less needed, that they were last week, or yesterday, or the day before the day after tomorrow. Starting today, we must pull up our pants, dust ourselves off, and begin again the work of remaking Nico Nico Doga. Now, there are some who question the scale of our ambitions, who suggest that out service system cannot tolerate to many movies. There memories are short, for they have forgotten what Nico Nico already has done, what free men can achieve when imagination is joined to common purpose. And so, to all of the people that are watching this video, from the grandest cities to the small villages where Exile was born, know that Nico Nico is a friend of every man who seeks a future of love and peace. Now we will begin to leave authorized common materials to Nico Nico people and forge a hard, earned piece in this mad world. What is required of us now is a new era of responsibility. This is the price and the promise of Nico nicommon's citizenship. Nico Nico Doga in the face of are common dangers in this winter of our hardship, let us remember these timeless words: ASS WE CAN. Let it be said by our children's children that when we were tested by doss attacks, and refused by Youtube, we did not turn back, nor did we falter. And we were carried forth that the great gift of freedom be delivered and is safely to future generations. Thank You. God Bless. And God Bless Nico Nico Doga.﻿");//.InputMessageContents.InputTextMessageContent

                Telegram.Bot.Types.InlineQueryResults.InlineQueryResultBase[] results = {
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultArticle
                        (
                            /*Id = */"1",
                            /*Title = */"Billy's speech",
                            /*InputMessageContent = */msg
                        ),
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultAudio
                        (
                            /*Id = */"2",
                            /*Url = */RandomizerGachiLinks(),
                            /*Title = */"Random Gachi Sound"
                        ),
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultPhoto
                        (
                            /*Id = */"3",
                            /*Url = */"https://static.wikia.nocookie.net/gachimuchi/images/5/55/Cody_Cruze_Punk_Punishment.png/revision/latest?cb=20190923161456",
                            /*ThumbUrl = */"https://static.wikia.nocookie.net/gachimuchi/images/5/55/Cody_Cruze_Punk_Punishment.png/revision/latest?cb=20190923161456"
                        ),
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultVideo
                        (
                            /*Id = */"4",
                            /*Url = */@"https://cdn77-vid.xvideos-cdn.com/5UueIsAWcUaEE5PBvQs7pQ==,1610859426/videos/3gp/f/c/f/xvideos.com_fcf64c1ba17fb8e36f42256876c474f0.mp4?ui=NTEuOTEuOTguNi0vdmlkZW8zMDU1NDg5Ny9sb3Jkc19vZl90aGVfbG8=",
                            /*MimeType = */"video/mp4",
                            /*ThumbUrl = */@"https://pbs.twimg.com/media/ESjX3LkWoAY8N_o.jpg",
                            /*Title = */"Boss of this Gym"
                        ),
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultGif
                        (
                            /*Id = */"5",
                            /*Url = */"https://media1.tenor.com/images/2564a79824061e2d30ae6dadc343bc19/tenor.gif?itemid=18340528",
                            /*ThumbUrl = */"https://media.tenor.com/images/590064de09301b38babd1da86b2de9e3/tenor.png"
                        )
                    };
                await bot.AnswerInlineQueryAsync(ei.InlineQuery.Id, results);
            };

        }

        private void StartOnCallbackQuery()
        {
            // Callback'и от кнопок
            bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
            {
                Console.WriteLine("StartOnCallbackQuery");
                var message = ev.CallbackQuery.Message;
                long id = message.Chat.Id + 1;
                switch (ev.CallbackQuery.Data)
                {
                    case "Van Darkholm":
                        await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - Van, take it boy", replyToMessageId: message.MessageId);

                        break;
                    case "Billy Herrington":
                        await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - Billy, press F to respect", replyToMessageId: message.MessageId);

                        break;
                    case "Steve Rambo":
                        await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - Steve, Look out!", replyToMessageId: message.MessageId);

                        break;
                    case "Mark Wolf":
                        await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - Mark, You are the boss of this gym", replyToMessageId: message.MessageId);

                        break;
                    case "$T@$$ Lee":
                        await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - $T@$$, Prepare you fucking @$$ Boy-ец", replyToMessageId: message.MessageId);

                        break;
                    default: break;
                }
                await bot.AnswerCallbackQueryAsync(ev.CallbackQuery.Id); // отсылаем пустое, чтобы убрать "частики" на кнопке
            };
        }

        private async void LoadFile(string fileId, string fileName)
        {
            Console.WriteLine($"Файл {fileName} загрузка начата");
            var file = await bot.GetFileAsync(fileId);
            using (FileStream fs = new FileStream($"{pathToLoadFile}\\{fileName}", FileMode.Create))
            {
                await bot.DownloadFileAsync(file.FilePath, fs);
            }
        }

        private void StartOnUpdate()
        {
            bot.OnUpdate += async (object su, Telegram.Bot.Args.UpdateEventArgs evu) =>
            {
                Console.WriteLine("StartOnUpdate");
                if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null) return; // в этом блоке нам келлбэки и инлайны не нужны
                var update = evu.Update;
                var message = update.Message;
                if (message == null) return;

                if (message.Type == MessageType.Document)
                {
                    LoadFile(message.Document.FileId, message.Document.FileName);
                }


                if (message.Type == MessageType.Text)
                {
                    if (message.Text == "/help")
                    {
                        // в ответ на команду /saysomething выводим сообщение
                        await bot.SendTextMessageAsync(message.Chat.Id,
                            "Что может Билли, кроме Fisting?\n" +
                            "Введи @BillyHerringhton, чтобы посмотреть на подсказки от Билли\n" +
                            "А также попробуй сказать что-то Билли\n" +
                            "Команды:\n" +
                            "/help - окно справки начинающего GachiMuchenika\n" +
                            "/saysomething - Спроси что-нибудь у Билли\n" +
                            "/getimage - Скинуть фотку из качалки\n" +
                            "/ibuttons - Выбери своего босса\n" +
                            "/rbuttons - Поговорить с другом Билли\n" +
                            "/playgachi - ass we can't\n" +
                            "/loadgachi <Название файла> - Загружает файл конкретного названия с сервера (Названия заготовленных имеющихся файлов:" +
                            "43.erwin, Chrono Trigger rus.smc, Snake.7z312, 4.jpg), ass we can\n",// Можно добавить возможность смотреть файлы и дописывать сюда
                            replyToMessageId: message.MessageId);
                    }

                    if (message.Text == "/saysomething")
                    {
                        // в ответ на команду /saysomething выводим сообщение
                        await bot.SendTextMessageAsync(message.Chat.Id, "Show you boss of this chat", replyToMessageId: message.MessageId);
                    }

                    if (message.Text == "/getimage")
                    {
                        // в ответ на команду /getimage выводим картинку
                        await bot.SendPhotoAsync(message.Chat.Id, "https://www.meme-arsenal.com/memes/dd4bab1c8cd9571bb0e62b7d579cf4c8.jpg", "Oh my shoulder!");
                    }

                    Regex regex = new Regex(@"/loadgachi");
                    if (regex.Matches(message.Text).Count > 0)//var fileResult = (TLInputFile)await client.UploadFile("cat.jpg", new StreamReader("data/cat.jpg"));
                    {
                        string pattern = "/loadgachi [A-Za-z0-9А-Яа-я ,\\.&\\$#№@':;\\{\\}\\[\\]\\(\\)\\*\\^\\%\\ _ \\-\\=]+";
                        regex = new Regex(pattern);
                        var srewr = regex.Match(message.Text).Value;
                        var strArr = regex.Match(message.Text).Value.Split(' ');
                        if (strArr.Length > 1)
                        {
                            string fileName = "";
                            bool first = true;
                            bool second = false;
                            foreach (var str in strArr)
                            {
                                if (first)
                                {
                                    first = false;
                                    second = true;
                                    continue;
                                }
                                if (!second)
                                {
                                    fileName += " ";
                                }
                                else
                                    second = false;
                                fileName += str;
                            }

                            using (FileStream fStream = new FileStream($@"E:\Visual Projects\Skillbox\Lab_9\BillyContent\{fileName}", FileMode.Open))
                            {
                                await bot.SendDocumentAsync(message.Chat, new InputOnlineFile(fStream, fileName), "BillyMailCorparation");
                            }
                        }
                    }

                    // inline buttons
                    if (message.Text == "/ibuttons")
                    {
                        var keyboard = new InlineKeyboardMarkup(
                                                new InlineKeyboardButton[][]//
                                                {
                                                            // First row
                                                            new [] {
                                                                // First column
                                                                new InlineKeyboardButton{ Text = "Van Darkholm", CallbackData = "Van Darkholm"  },//("раз","callback1"),

                                                                // Second column
                                                                new InlineKeyboardButton{ Text = "Billy Herrington", CallbackData = "Billy Herrington"  },
                                                            },
                                                            new [] {
                                                                // First column
                                                                new InlineKeyboardButton{ Text = "Steve Rambo", CallbackData = "Steve Rambo"  },//("раз","callback1"),

                                                                // Second column
                                                                new InlineKeyboardButton{ Text = "Mark Wolf", CallbackData = "Mark Wolf"  },
                                                            },
                                                            new [] {
                                                                // First column
                                                                new InlineKeyboardButton{ Text = "$T@$$ Lee", CallbackData = "$T@$$ Lee"  },//("раз","callback1"),
                                                            }
                                                }
                                            ); ;

                        await bot.SendTextMessageAsync(message.Chat.Id, "Выбери своего сигнатурного Boyца:\n" +
                            "Van Darkholm - Быстрый и молненосный Boy-ец, умеет проводить быстрые атаки в тыл, но также сложен в освоении, в начале он будет всего лишь Van, которого не побоится даже Slave, потом можно стать настоящей грозой Slaves и получить титут DungeonMaster\n" +
                            "Billy Herrington - Хороший тяжеловесный Boy-ец, имеет большой запас Semen, проводить хорошие серии мощных атак, а также обладает сильным перком уворота Barrel roll, неплохой выбор для новичка, главное не забывать, что Semen не бесконечен, а если она будет закончиваться Press F для постепенной регенерации\n" +
                            "Steve Rambo - Отличный разведчик, специализируется на дальнем Boy-е, обладает перком Steve look out!, благодаря чему способен избежать CumShot один раз, хороший герой, если умеешь держать врагов на дистанции, в ближнем бою есть только один перк Suck some dick\n" +
                            "Mark Wolf - Закаленный в боях специалист по Wrong doors, единственный Boy-ец, который разбирается в том, что такое Jabroni, в основном испульзует атаки ближнего боя, но также обладает особыми dirty приемами для получения приемущества над противником\n" +
                            "$T@$$ Lee - Выбор настоящих безумцев, имеет только одно оружие дальнего Boy-а - водянной пистолет, урон каждого выстрела представляет из себя геометричискую прогрессию, не имеет больше перков, зато у него есть классная гифка\n",
                            ParseMode.Default, false, false, 0, keyboard);
                    }

                    // reply buttons
                    if (message.Text == "/rbuttons")
                    {
                        var keyboard = new ReplyKeyboardMarkup
                        {
                            Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new KeyboardButton("No Fuck you, leatherman!"),
                                                    new KeyboardButton("Oh yes sir!")
                                                },
                                            },
                            ResizeKeyboard = true
                        };

                        await bot.SendTextMessageAsync(message.Chat.Id, "Fuck you!", ParseMode.Default, false, false, 0, keyboard);
                    }
                    // обработка reply кнопок
                    if (message.Text.ToLower() == "no fuck you, leatherman!")
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, "Show you boss of this Gym!", replyToMessageId: message.MessageId);
                    }
                    if (message.Text.ToLower() == "oh yes sir!")
                    {
                        await bot.SendTextMessageAsync(message.Chat.Id, "Take it boy!", replyToMessageId: message.MessageId);
                    }
                }
            };
        }

        public async void StartBot()
        {
            try
            {
                await bot.SetWebhookAsync(""); // Обязательно! убираем старую привязку к вебхуку для бота

                StartOnInlineQuery();
                StartOnCallbackQuery();
                StartOnUpdate();

                bot.StartReceiving();
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message); // если ключ не подошел - пишем об этом в консоль отладки
            }
        }

        private string RandomizerGachiLinks()
        {
            Random rand = new Random();

            switch (rand.Next(0, 6))
            {
                case 0:
                    return "https://www.myinstants.com/media/sounds/orgasm-6.mp3";
                case 1:
                    return "https://www.myinstants.com/media/sounds/oh-shit-iam-sorry.mp3";
                case 2:
                    return "https://www.myinstants.com/media/sounds/boy-next-door.mp3";
                case 3:
                    return "https://www.myinstants.com/media/sounds/rip-ears.mp3";
                case 4:
                    return "https://www.myinstants.com/media/sounds/right-version5.mp3";
                case 5:
                    return "https://www.myinstants.com/media/sounds/fisting-is-300.mp3";
                case 6:
                    return "https://www.myinstants.com/media/sounds/slaves-get-your-ass-back-here_0lGpNz0.mp3";
                default: Console.WriteLine("Error Gachi Links"); break;
            }
            return "https://www.myinstants.com/media/sounds/stick-your-finger-in-my-ass.mp3";
        }
    }
}
