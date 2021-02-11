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
using System.Data;
using System.Collections;

namespace Lab_9
{
    public class AlexeyTelegramBot : IAlexeyTelegramBot
    {
        TelegramBotClient bot;
        public string PathToSettingsData { get; set; }
        public string TelegramToken { get; set; }// Можно добавить сигналы на переподключение, но пока смысл такого функционала не вижу
        public string GoogleToken { get; set; }
        public string PathToLoadFile { get; set; }

        Settings settings = new Settings(true, true, true);
        public Settings Settings { get { return settings; } set { settings = value; NewSettingsEvent(); } }

        HashSet<User> users;//Вообще, тут также подошел бы map в качестве ключа userName(UserId), а Data все остальное
        public ICollection<User> Users { get => users; }

        Queue<string> logs;
        public ICollection Logs { get => logs; }

        bool enable = false;
        public bool Enable => enable;

        delegate void SettingsHandler();
        event SettingsHandler NewSettingsEvent;

        #region TODO
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
        #endregion

        public AlexeyTelegramBot(string apathToLoadFile, string pathToTelegramToken, string apathToSettingsData, string pathToGoogleToken = "")
        {
            PathToLoadFile = apathToLoadFile;
            TelegramToken = System.IO.File.ReadAllText(pathToTelegramToken);
            if (pathToGoogleToken != "")
                GoogleToken = System.IO.File.ReadAllText(pathToGoogleToken);
            else
                GoogleToken = "Nothing";

            PathToSettingsData = apathToSettingsData;


            //// https://hidemyna.me/ru/proxy-list/?maxtime=250#list
            var proxy = new WebProxy()
            {
                Address = new Uri($"http://88.198.50.103:8080"),
                UseDefaultCredentials = false,
            };

            var httpClientHandler = new HttpClientHandler() { /*Proxy = proxy*/ };

            HttpClient hc = new HttpClient(httpClientHandler);

            bot = new TelegramBotClient(TelegramToken, hc);

            NewSettingsEvent += UpdateSettings;

            users = new HashSet<User>(LoadUserFile(PathToSettingsData));
            logs = new Queue<string>();

        }

        ~AlexeyTelegramBot()
        {
            SaveUserFile(users, PathToSettingsData);
            SaveLogs(logs, PathToSettingsData);
        }

        private HashSet<User> LoadUserFile(string path)
        {
            return JSONController<HashSet<User>>.Deserialize($"{path}/usersData.json");
        }

        private void SaveUserFile(HashSet<User> collection, string path)
        {
            JSONController<HashSet<User>>.Serialize(collection, $"{path}/usersData.json");
        }

        private void SaveLogs(Queue<string> collection, string path)
        {
            JSONController<Queue<string>>.Serialize(collection, $"{path}/logsData.json");
        }



        /// <summary>
        /// Dangerous!!!
        /// </summary>
        /// <param name="message"></param>
        public async void SendAllMessage(string message)
        {
            foreach (var user in users)
            {
                if (user.mailing == true)
                {
                    await bot.SendTextMessageAsync(user.userId, message);
                }
            }
        }


        private async void OnInlineQuery(object si, Telegram.Bot.Args.InlineQueryEventArgs ei)
        {
            Console.WriteLine("StartOnInlineQuery");
            logs.Enqueue($"InlineQuery/{ei.InlineQuery.From.Id}/{ei.InlineQuery.From.Username}/{ei.InlineQuery.From.FirstName}/{ei.InlineQuery.From.LastName}/{DateTime.Now}/{ei.InlineQuery.Query}/{ei.InlineQuery.Location}");
            var query = ei.InlineQuery.Query;

            var msg = new Telegram.Bot.Types.InlineQueryResults.InputTextMessageContent(@"﻿Привет, это мое сообщение");//.InputMessageContents.InputTextMessageContent

            Telegram.Bot.Types.InlineQueryResults.InlineQueryResultBase[] results = {
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultArticle
                        (
                            "1",
                            "Alexey speech",
                            msg
                        ),
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultAudio
                        (
                            "2",
                            RandomizerLinks(),
                            "Random Sound"
                        ),
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultPhoto
                        (
                            "3",
                            "https://c4.wallpaperflare.com/wallpaper/321/583/696/pixel-cat-fantasy-art-digital-art-river-forest-hd-wallpaper-preview.jpg",
                            "https://c4.wallpaperflare.com/wallpaper/321/583/696/pixel-cat-fantasy-art-digital-art-river-forest-hd-wallpaper-preview.jpg"
                        ),
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultVideo
                        (
                            "4",
                            @"https://www.youtube.com/watch?v=a_IA-8nQ4FY&ab_channel=Kittisaurus",
                            "video/mp4",
                            @"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQWGurH-ZDzJ1Qn4AZio1SBETGWZFuJCcbGyw&usqp=CAU",
                            "Airplane"
                        ),
                        new Telegram.Bot.Types.InlineQueryResults.InlineQueryResultGif
                        (
                            "5",
                            "https://i.gifer.com/8YV8.gif",
                            "https://i.pinimg.com/originals/09/fb/9c/09fb9c99329d9c1b973dc2f4114ac1c0.jpg"
                        )
                    };
            await bot.AnswerInlineQueryAsync(ei.InlineQuery.Id, results);
        }

        private async void OnCallbackQueryAsync(object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev)
        {
            Console.WriteLine("StartOnCallbackQuery");
            logs.Enqueue($"CallbackQuery/{ev.CallbackQuery.From.Id}/{ev.CallbackQuery.From.Username}/{ev.CallbackQuery.From.FirstName}/{ev.CallbackQuery.From.LastName}/{DateTime.Now}/{ev.CallbackQuery.Data}/{ev.CallbackQuery.Message.Location}");
            var message = ev.CallbackQuery.Message;
            long id = message.Chat.Id + 1;
            switch (ev.CallbackQuery.Data)
            {
                case "Олег":
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - Олег", replyToMessageId: message.MessageId);

                    break;
                case "Влад":
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - Влад, press F to respect", replyToMessageId: message.MessageId);

                    break;
                case "Игорь":
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - Игорь, Look out!", replyToMessageId: message.MessageId);

                    break;
                case "Кирилл":
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - Кирилл", replyToMessageId: message.MessageId);

                    break;
                case "Стас":
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Теперь {message.Chat.Username} - Стас, будет сложно", replyToMessageId: message.MessageId);

                    break;
                default: break;
            }
            await bot.AnswerCallbackQueryAsync(ev.CallbackQuery.Id); // отсылаем пустое, чтобы убрать "частики" на кнопке
        }

        private async void OnUpdate(object su, Telegram.Bot.Args.UpdateEventArgs evu)
        {
            Console.WriteLine("StartOnUpdate");
            if (evu.Update.CallbackQuery != null || evu.Update.InlineQuery != null) return; // в этом блоке нам келлбэки и инлайны не нужны
            var update = evu.Update;
            var message = update.Message;
            if (message == null) return;
            logs.Enqueue($"Update/{evu.Update.Message.From.Id}/{evu.Update.Message.From.Username}/{evu.Update.Message.From.FirstName}/{evu.Update.Message.From.LastName}/{DateTime.Now}/{evu.Update.Message.Text}/{evu.Update.Message.Location}");


            if (!Settings.isRegistrationNewUsers && message.Chat.Type == ChatType.Private)
            {
                bool have = false;
                foreach (var user in users)// Почему не Contains? Тк он сравнивает все элементы, а мне нужен только userId, может что-то можно перегрузить, но я пока не нашел
                {                       //Вообще, сменил бы СД на map
                    if (user.userId == message.From.Id)
                    {
                        have = true;
                        break;
                    }
                }
                if (have == false)
                    users.Add(new User(message.From.Id, message.From.FirstName, message.From.LastName, false));
            }

            foreach (var user in users)
            {
                Console.WriteLine(user.userId + "  " + message.From.Id);
            }

            if (message.Type == MessageType.Document && settings.isSendData)
            {
                LoadFile(message.Document.FileId, message.Document.FileName);
            }


            if (message.Type == MessageType.Text)
            {
                if (message.Text == "/help")
                {
                    // в ответ на команду /saysomething выводим сообщение
                    await bot.SendTextMessageAsync(message.Chat.Id,
                        "Что может Алексей\n" +
                        "Введи @Alexey, чтобы посмотреть на подсказки от Алексея\n" +
                        "А также попробуй сказать что-то Билли\n" +
                        "Команды:\n" +
                        "/help - окно справки\n" +
                        "/saysomething - Спроси что-нибудь у Алексея\n" +
                        "/getimage - Скинуть фотографиюи\n" +
                        "/ibuttons - Выбери своего героя\n" +
                        "/rbuttons - Поговорить\n" +
                        "/playmusic - Пока не реализована\n" +
                        "/load <Название файла> - Загружает файл конкретного названия с сервера (Названия заготовленных имеющихся файлов:" +
                        "43.erwin, Chrono Trigger rus.smc, Snake.7z312, 4.jpg)\n",// Можно добавить возможность смотреть файлы и дописывать сюда
                        replyToMessageId: message.MessageId);
                }

                if (message.Text == "/saysomething")
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Привет, я бот Алексей", replyToMessageId: message.MessageId);
                }

                if (message.Text == "/getimage")
                {
                    await bot.SendPhotoAsync(message.Chat.Id, "https://www.kauergames.com/wp-content/uploads/2016/10/pixel-landscape.png", "Forest!");
                }

                Regex regex = new Regex(@"/load");
                if (regex.Matches(message.Text).Count > 0 && settings.isLoadData)//var fileResult = (TLInputFile)await client.UploadFile("cat.jpg", new StreamReader("data/cat.jpg"));
                {
                    string pattern = "/load [A-Za-z0-9А-Яа-я ,\\.&\\$#№@':;\\{\\}\\[\\]\\(\\)\\*\\^\\%\\ _ \\-\\=]+";
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

                        using (FileStream fStream = new FileStream($@"{PathToLoadFile}\{fileName}", FileMode.Open))
                        {
                            await bot.SendDocumentAsync(message.Chat, new InputOnlineFile(fStream, fileName), "AlexeyMailCorparation");
                        }
                    }
                }

                // inline buttons
                if (message.Text == "/ibuttons")
                {
                    var keyboard = new InlineKeyboardMarkup(
                                            new InlineKeyboardButton[][]//
                                            {
                                                            new [] {
                                                                new InlineKeyboardButton{ Text = "Олег", CallbackData = "Олег"  },

                                                                new InlineKeyboardButton{ Text = "Влад", CallbackData = "Влад"  },
                                                            },
                                                            new [] {
                                                                new InlineKeyboardButton{ Text = "Игорь", CallbackData = "Игорь"  },

                                                                new InlineKeyboardButton{ Text = "Кирилл", CallbackData = "Кирилл"  },
                                                            },
                                                            new[] {
                                                                new InlineKeyboardButton{ Text = "Стас", CallbackData = "Стас"  },
                                                            }
                                            }
                                        ); ;

                    await bot.SendTextMessageAsync(message.Chat.Id, "Выбери своего сигнатурного Бойца:\n" +
                        "Олег - Быстрый и молненосный Боец, умеет проводить быстрые атаки, но также сложен в освоении, в начале он будет всего лишь Олежа, которого не побоится даже Иван, потом можно стать настоящей грозой Иванов и получить титут Чемпион\n" +
                        "Влад - Хороший тяжеловесный Боец, имеет большой запас здоровья, проводить хорошие серии мощных атак, а также обладает сильным перком уворота Barrel roll, неплохой выбор для новичка, главное не забывать, что Здоровье не бесконечен, а если оно будет закончиваться Press F для постепенной регенерации\n" +
                        "Игорь - Отличный разведчик, специализируется на дальнему бою, обладает перком look out!, благодаря чему способен избежать смерти один раз, хороший герой, если умеешь держать врагов на дистанции, в ближнем бою есть только один перк удар рукой\n" +
                        "Кирилл - Закаленный в боях специалист по Минам, единственный Боец, который разбирается в том, что такое Взрывчатка, в основном использует атаки ближнего боя, но также обладает особыми взрывными приемами для получения приемущества над противником\n" +
                        "Стас - Выбор настоящих безумцев, имеет только одно оружие дальнего боя - водянной пистолет, урон каждого выстрела представляет из себя геометричискую прогрессию, не имеет больше перков, зато у него есть классная фотка\n",
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
                                                    new KeyboardButton("Hello!"),
                                                    new KeyboardButton("Привет!")
                                                },
                                            },
                        ResizeKeyboard = true
                    };

                    await bot.SendTextMessageAsync(message.Chat.Id, "Hi!", ParseMode.Default, false, false, 0, keyboard);
                }
                // обработка reply кнопок
                if (message.Text.ToLower() == "Hello!")
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "How are you?", replyToMessageId: message.MessageId);
                }
                if (message.Text.ToLower() == "Привет!")
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Как дела?", replyToMessageId: message.MessageId);
                }

                if (message.Text == "/subscribe")
                {

                    foreach (var user in users)
                    {
                        if (user.userId == message.From.Id)
                        {
                            if (user.mailing)
                            {
                                await bot.SendTextMessageAsync(message.Chat.Id, "You are already in the boy band!", replyToMessageId: message.MessageId);
                            }
                            else
                            {
                                User newUserData = new User(user);
                                users.Remove(user);
                                newUserData.mailing = true;
                                users.Add(newUserData);
                                await bot.SendTextMessageAsync(message.Chat.Id, "Welcome to the club buddy!", replyToMessageId: message.MessageId);
                            }
                            break;
                        }
                    }
                }

                if (message.Text == "/unsubscribe")
                {
                    foreach (var user in users)
                    {
                        if (user.userId == message.From.Id)
                        {
                            if (user.mailing)
                            {
                                User newUserData = new User(user);
                                users.Remove(user);
                                newUserData.mailing = false;
                                users.Add(newUserData);
                                await bot.SendTextMessageAsync(message.Chat.Id, $"Press F to {user.firstName} {user.lastName}", replyToMessageId: message.MessageId);
                            }
                            else
                            {
                                await bot.SendTextMessageAsync(message.Chat.Id, "You don't have a contract in the bot band", replyToMessageId: message.MessageId);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void StartOnInlineQuery()
        {
            bot.OnInlineQuery -= OnInlineQuery;
            bot.OnInlineQuery += OnInlineQuery;
        }

        private void StartOnCallbackQuery()
        {
            bot.OnCallbackQuery -= OnCallbackQueryAsync;
            bot.OnCallbackQuery += OnCallbackQueryAsync;
        }

        private void StartOnUpdate()
        {
            bot.OnUpdate -= OnUpdate;
            bot.OnUpdate += OnUpdate;
        }

        private async void LoadFile(string fileId, string fileName)
        {
            Console.WriteLine($"Файл {fileName} загрузка начата");
            var file = await bot.GetFileAsync(fileId);
            using (FileStream fs = new FileStream($"{PathToLoadFile}\\{fileName}", FileMode.Create))
            {
                await bot.DownloadFileAsync(file.FilePath, fs);
            }
        }

        public async void StartBot()
        {
            try
            {
                await bot.SetWebhookAsync(""); // Убираем старую привязку к вебхуку для бота

                UpdateSettings();

                bot.StartReceiving();
                enable = true;
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message); // если ключ не подошел - пишем об этом в консоль отладки
            }
        }

        public void StopBot()
        {
            bot.StopReceiving();
            enable = false;
        }


        private void UpdateSettings()
        {
            StartOnInlineQuery();
            StartOnCallbackQuery();
            StartOnUpdate();
        }

        private string RandomizerLinks()
        {
            Random rand = new Random();

            switch (rand.Next(0, 6))
            {
                case 0:
                    return "https://www.soundjay.com/button/sounds/button-1.mp3";
                case 1:
                    return "https://www.soundjay.com/button/sounds/button-2.mp3";
                case 2:
                    return "https://www.soundjay.com/button/sounds/button-3.mp3";
                case 3:
                    return "https://www.soundjay.com/button/sounds/button-4.mp3";
                case 4:
                    return "https://www.soundjay.com/button/sounds/button-5.mp3";
                case 5:
                    return "https://www.soundjay.com/button/sounds/button-6.mp3";
                case 6:
                    return "https://www.soundjay.com/button/sounds/button-7.mp3";
                default: Console.WriteLine("Error Links"); break;
            }
            return "https://www.soundjay.com/button/sounds/button-09.mp3";
        }
    }
}
