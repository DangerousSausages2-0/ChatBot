using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using System.Drawing;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using System.IO;
using static System.IO.Path;
using System.Data.SQLite;

namespace DangerousSausages
{
    
    public partial class MainWindow : Window
    {
        string Token;
        private static Telegram.Bot.TelegramBotClient BOT;
        


        public  MainWindow()
        {
            InitializeComponent();
            if (!File.Exists(@"C:\Users\kaut1\Desktop\TestDB.db"))
            {
                SQLiteConnection.CreateFile(@"C:\Users\kaut1\Desktop\TestDB.db");
            }
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=C:\Users\kaut1\Desktop\jn(1)\TestDB.db; Version=3;")) // в строке указывается к какой базе подключаемся
            {
                // строка запроса, который надо будет выполнить
                string commandText = "CREATE TABLE IF NOT EXISTS [dbTableName] ( [id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, [name] NVARCHAR(128), [position] NVARCHAR(128), " +
       "[task] NVARCHAR(128))"; // создать таблицу, если её нет
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Connect.Open(); // открыть соединение
                Command.ExecuteNonQuery(); // выполнить запрос
                Connect.Close(); // закрыть соединение
            }
        }
        


private void EnterToken(object sender, EventArgs e)
        {
            Token = TokenText.Text;

            if (TokenText != null)
            {
                TokenLabel.Content = "Token received";
            }
        }

        private void StartBot(object sender, RoutedEventArgs e)
        {
            if (TokenLabel.Content.Equals("Token received"))
            {
                TokenLabel.Content = "Bot alive";
            }
            BOT = new Telegram.Bot.TelegramBotClient(Token);
            BOT.OnMessage += BotOnMessageReceived;
            BOT.StartReceiving(new UpdateType[] { UpdateType.Message });
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            

            String Answer = "";

            
            Telegram.Bot.Types.Message msg = messageEventArgs.Message;
            if (msg == null || msg.Type != MessageType.Text) return;
                
            

                switch (msg.Text)
            {
                case "/tasks":
                    Answer = "Введите имя и фамилию"; 
                    break;
                case "/salary": Answer = "У вас зарплата маленькая, но хорошая. В принципе, ее хватает, чтобы доехать до гипермаркета, погулять в нем и уехать обратно.";
                break;
                case "/link": Answer = "https://www.rosatom.ru/"; break;
                case "/start": Answer = "/info - Информация о компании\r\n\r\n" +
                                        "/regulations - Нормативно правовые документы\r\n\r\n" +
                                        "/career_growth - Возможности карьерного роста\r\n\r\n" +
                                        "/tasks - Ваши задачи на ближайшие 20 дней\r\n\r\n" +
                                        "/salary - Узнать вашу зарплату\r\n\r\n" +
                                        "/link - Ссылка на нашу официальную веб-страницу" +
                                        "/reg - Регистрация"; break;

                case "/info": Answer = "Ниже приведена информация о компании:"; break;
                case "/regulations": Answer = "Выше предоставлен список нормативных документов.(В качестве примера использованы документы публичной отчётности)";
                   await BOT.SendDocumentAsync(msg.Chat.Id, "https://rosatom.ru/upload/iblock/adc/adc15613bd61b329a802b434d1f8046e.pdf", "Единая отраслевая политика в области публичной отчетности");
                   await BOT.SendDocumentAsync(msg.Chat.Id, "https://rosatom.ru/upload/iblock/033/03395b2a9751b4fcd385d746a2f9df15.pdf", "Единая отраслевая политика в области публичной отчетности");
                    break;
                case "/reg": Answer = "Пользователь зарегистрирован"; break;
                case "/career_growth": Answer = "Ниже приведены возможности карьерного роста в нашей компании:"; break;
                case "Студент":
                    Answer = "Росатом ведет систематическую работу с профильными вузами. Для студентов и выпускников действуют программы, которые позволяют в ходе обучения получить практический опыт взаимодействия с предприятиями отрасли, сформировать четкое, объективное видение будущей специальности. В частности, на Отраслевом карьерном портале студентов и выпускников (ссылка находится ниже) описаны возможности прохождения практики на предприятиях Росатома.\n\r\n\rСсылка: http://rosatom-career.ru/";
                    break;
                case "Профессионал":
                    Answer = "Профессионалам с опытом работы и развитыми управленческими компетенциями Госкорпорация «Росатом» предлагает попробовать свои силы в масштабных, интересных проектах. Вы можете ознакомиться с текущими вакансиями и конкурсами на замещение должностей.\n\r\n\rСсылка: https://rosatom.ru/career/soiskatelyam/";
                    break;
                case "Стремление компании": Answer = "Компания стремится быть лидером на глобальных рынках. Компания всегда на шаг впереди в технологиях, знаниях и качествах наших сотрудников. Предвидит, что будет завтра, и готова к этому сегодня, постоянно развивается и учится. Каждый день представители компании стараются работать лучше, чем вчера.";
                    break;
                case "Ответственность сотрудников": Answer = "Каждый из сотрудников несет личную ответственность за результат своей работы и качество своего труда перед государством, отраслью, коллегами и заказчиками. В работе предъявляются самые высокие требования. Оцениваются не затраченные усилия, а достигнутый результат. Успешный результат – основа для новых достижений. ";                
                    break;
                case "Работа в команде": Answer = "В компании Росатом нет Я. Все сотрудники - это большая семья и дружная команда. Вместе мы сильнее и можем добиваться самых высоких целей. Успехи сотрудников – успехи компании.";
                    break;
                case "Уважение к своей работе": Answer = "Представители компании с уважением относятся к заказчикам, партнерам и поставщикам. Всегда внимательно слушают и стараются услышать друг друга вне зависимости от занимаемых должностей и места работы. В компании уважают историю и традиции отрасли. Достижения прошлого вдохновляют представителей компании на новые победы.";
                    break;
                case "Безопасность сотрудников": Answer = "Безопасность – наивысший приоритет. В работе в первую очередь обеспечивается полная безопасность людей и окружающей среды. В безопасности нет мелочей – представители компании знают правила безопасности и выполняют их, пресекая нарушения.";
                    break;
                case "Эффективность труда": Answer = "Компания находит наилучшие варианты решения задач. Представители компании эффективны во всем, что делают – при выполнении поставленных целей максимально рационально используются ресурсы компании и постоянно совершенствуются рабочие процессы. Нет препятствий, которые могут помешать находить самые эффективные решения. ";
                    break;
                case "Скрыть панель":
                    Answer = "напишите команду заново";
                    await BOT.SendTextMessageAsync(msg.Chat.Id, "Для повторного вызова панели", replyMarkup: new ReplyKeyboardRemove());
                    break;
                default:
                    using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=C:\Users\kaut1\Desktop\jn(1)\TestDB.db; Version=3;"))
                    {
                        string commandText = "SELECT * FROM [dbTableName] WHERE [name] NOT NULL";
                        SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                        Connect.Open(); // открыть соединение
                        SQLiteDataReader sqlReader = Command.ExecuteReader();
                        while (sqlReader.Read()) // считываем и вносим в лист все параметры
                        {
                            object all_task = " ";
                            object position = " ";
                            object name = sqlReader["name"];
                            if (msg.Text == name.ToString())
                            {
                                all_task = sqlReader["task"];
                                position = sqlReader["position"];
                                Answer = "Специализация:\n" + position.ToString() + "\nЗадания на месяц:\n" + all_task.ToString();
                                await BOT.SendTextMessageAsync(msg.Chat.Id, Answer);
                            }
                            if (msg.Text != name.ToString() ) {
                                Answer = "Перейдите в /start";
                            }
                        }
                        Connect.Close(); // закрыть соединение

                    }
                    break;
            }
            await BOT.SendTextMessageAsync(msg.Chat.Id, Answer);
            if (msg.Text == "/info")
            {
                var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                {
                    Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new KeyboardButton("Стремление компании"),
                                                    new KeyboardButton("Ответственность сотрудников"),
                                                },
                                                new[] // row 2
                                                {
                                                    new KeyboardButton("Работа в команде"),
                                                    new KeyboardButton("Уважение к своей работе"),
                                                },
                                                new[] // row 3
                                                {
                                                    new KeyboardButton("Безопасность сотрудников"),
                                                    new KeyboardButton("Эффективность труда"),
                                                },
                                                new[] // row 4
                                                {
                                                    new KeyboardButton("Скрыть панель"),
                                                },
                                             },
                    ResizeKeyboard = true
                };
                await BOT.SendTextMessageAsync(msg.Chat.Id, "О чём хотите узнать?", replyMarkup: keyboard);
            }
            // Поиск имени в БД
            

            if (msg.Text == "/career_growth")
            {
                var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                {
                    Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new KeyboardButton("Студент"),
                                                    new KeyboardButton("Профессионал"),
                                                },
                                                new[] // row 2
                                                {
                                                    new KeyboardButton("Скрыть панель"),
                                                },
                                             },
                    ResizeKeyboard = true
                };
                await BOT.SendTextMessageAsync(msg.Chat.Id, "Кем вы являетесь?", replyMarkup: keyboard);
            }
        }

        private void TokenText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=C:\Users\kaut1\Desktop\jn(1)\TestDB.db; Version=3;"))
            {
                string nameUser = NameTextBOX.Text;
                string positionUser = positionTextBox.Text;
                string taskUser = TasksTextBox.Text;
                // в запросе есть переменные, они начинаются на @, обратите на это внимание
                string commandText = "INSERT INTO [dbTableName] ([name],[position],[task]) VALUES(@name, @position, @task)";
                SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@name", nameUser); // присваиваем переменной значение
                Command.Parameters.AddWithValue("@position", positionUser);
                Command.Parameters.AddWithValue("@task", taskUser);
                Connect.Open();
                Command.ExecuteNonQuery(); // выполнить запрос
                Connect.Close();
            }
        }
    }
}
