using System;
using System.Collections.Generic;

class Program
{
    static string userName = "";
    static List<string> tasks = new List<string>();

    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать! Доступные команды: /start, /help, /info, /addtask, /showtasks, /removetask, /exit");

        while (true)
        {
            Console.Write("Введите команду: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "/start":
                    StartCommand();
                    break;
                case "/help":
                    HelpCommand();
                    break;
                case "/info":
                    InfoCommand();
                    break;
                case "/addtask":
                    AddTaskCommand();
                    break;
                case "/showtasks":
                    ShowTasksCommand();
                    break;
                case "/removetask":
                    RemoveTaskCommand();
                    break;
                case "/exit":
                    ExitCommand();
                    return;
                default:
                    if (input.StartsWith("/echo") && !string.IsNullOrEmpty(userName))
                    {
                        EchoCommand(input);
                    }
                    else
                    {
                        Console.WriteLine("Неизвестная команда или команда /echo недоступна. Введите /help для справки.");
                    }
                    break;
            }
        }
    }

    static void StartCommand()
    {
        Console.Write("Введите ваше имя: ");
        userName = Console.ReadLine();
        Console.WriteLine($"Привет, {userName}!");
    }

    static void HelpCommand()
    {
        Console.WriteLine("Справка по командам:");
        Console.WriteLine("/start - начать работу с ботом, ввести имя");
        Console.WriteLine("/help - показать справку");
        Console.WriteLine("/info - информация о программе");
        Console.WriteLine("/addtask - добавить задачу");
        Console.WriteLine("/showtasks - показать список задач");
        Console.WriteLine("/removetask - удалить задачу по номеру");
        Console.WriteLine("/exit - выйти из программы");
        if (!string.IsNullOrEmpty(userName))
        {
            Console.WriteLine("/echo [текст] - повторить введенный текст");
        }
    }

    static void InfoCommand()
    {
        Console.WriteLine("Версия программы: 1.0");
        Console.WriteLine("Дата создания: 2023-10-01");
    }

    static void EchoCommand(string input)
    {
        string echoText = input.Substring(6);
        Console.WriteLine($"{userName}, вы сказали: {echoText}");
    }

    static void AddTaskCommand()
    {
        Console.Write("Пожалуйста, введите описание задачи: ");
        string taskDescription = Console.ReadLine();
        tasks.Add(taskDescription);
        Console.WriteLine($"Задача \"{taskDescription}\" добавлена.");
    }

    static void ShowTasksCommand()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
        }
        else
        {
            Console.WriteLine("Список задач:");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }
    }

    static void RemoveTaskCommand()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Список задач пуст. Нечего удалять.");
            return;
        }

        ShowTasksCommand();
        Console.Write("Введите номер задачи для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
        {
            string removedTask = tasks[taskNumber - 1];
            tasks.RemoveAt(taskNumber - 1);
            Console.WriteLine($"Задача \"{removedTask}\" удалена.");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи. Пожалуйста, введите корректный номер.");
        }
    }

    static void ExitCommand()
    {
        Console.WriteLine("До свидания!");
    }
}