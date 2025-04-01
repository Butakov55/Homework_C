using System;
using System.Collections.Generic;

class Program
{
    static string userName = "";
    static List<string> tasks = new List<string>();
    static int maxTaskCount = 0;
    static int maxTaskLength = 0;

    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Добро пожаловать! Доступные команды: /start, /help, /info, /addtask, /showtasks, /removetask, /exit");

            // Ввод максимального количества задач
            Console.Write("Введите максимально допустимое количество задач (1-100): ");
            maxTaskCount = ParseAndValidateInt(Console.ReadLine(), 1, 100);

            // Ввод максимальной длины задачи
            Console.Write("Введите максимально допустимую длину задачи (1-100): ");
            maxTaskLength = ParseAndValidateInt(Console.ReadLine(), 1, 100);

            while (true)
            {
                try
                {
                    Console.Write("Введите команду: ");
                    string input = Console.ReadLine();

                    HandleCommand(input);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (TaskCountLimitException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (TaskLengthLimitException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DuplicateTaskException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла непредвиденная ошибка: {ex.GetType()}");
                    Console.WriteLine($"Message: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                    Console.WriteLine($"InnerException: {ex.InnerException}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла непредвиденная ошибка: {ex.GetType()}");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            Console.WriteLine($"InnerException: {ex.InnerException}");
        }
    }

    static void HandleCommand(string input)
    {
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
                Environment.Exit(0);
                break;
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

    static int ParseAndValidateInt(string? str, int min, int max)
    {
        if (!int.TryParse(str, out int result))
        {
            throw new ArgumentException("Введенное значение должно быть числом.");
        }

        if (result < min || result > max)
        {
            throw new ArgumentException($"Введенное значение должно быть в диапазоне от {min} до {max}.");
        }

        return result;
    }

    static void ValidateString(string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentException("Строка не может быть пустой или состоять только из пробелов.");
        }
    }

    static void StartCommand()
    {
        Console.Write("Введите ваше имя: ");
        userName = Console.ReadLine();
        ValidateString(userName);
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
        ValidateString(taskDescription);

        if (tasks.Count >= maxTaskCount)
        {
            throw new TaskCountLimitException(maxTaskCount);
        }

        if (taskDescription.Length > maxTaskLength)
        {
            throw new TaskLengthLimitException(taskDescription.Length, maxTaskLength);
        }

        if (tasks.Contains(taskDescription))
        {
            throw new DuplicateTaskException(taskDescription);
        }

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

class TaskCountLimitException : Exception
{
    public TaskCountLimitException(int taskCountLimit)
        : base($"Превышено максимальное количество задач равное {taskCountLimit}")
    {
    }
}

class TaskLengthLimitException : Exception
{
    public TaskLengthLimitException(int taskLength, int taskLengthLimit)
        : base($"Длина задачи '{taskLength}' превышает максимально допустимое значение {taskLengthLimit}")
    {
    }
}

class DuplicateTaskException : Exception
{
    public DuplicateTaskException(string task)
        : base($"Задача '{task}' уже существует")
    {
    }
}