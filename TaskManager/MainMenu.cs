namespace TaskManager
{
    internal class MainMenu
    {
        private bool menuRunning = true;
        private TaskService taskService = new TaskService();
        
        public void Run()
        {
            while (menuRunning)
            {
                ShowMenu();
                int userChoice = ReadChoice(0, 2);

                switch (userChoice)
                {
                    case 0:
                        menuRunning = false;
                        break;
                    case 1:
                        CreateTask();
                        break;
                    case 2:
                        ShowTasks();
                        break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("ПЛАНИРОВЩИК ЗАДАЧ");
            Console.WriteLine("=================");
            Console.WriteLine();
            Console.WriteLine("Что вы хотите сделать?");
            Console.WriteLine("1 - Создать новую задачу.");
            Console.WriteLine("2 - Просмотреть задачи.");
            Console.WriteLine();
            Console.WriteLine("0 - Выход");
            Console.WriteLine();
            Console.Write("Ваш выбор: ");
        }

        private void CreateTask()
        {            
            string title = ReadRequiredText("Введите название задачи: ");
            string description = ReadRequiredText("Введите описание задачи: ");
            TaskItem newTask = new TaskItem(title, description);
            taskService.AddTask(newTask);
            Console.WriteLine("Задача добавлена!");
            WaitForKey();
        }

        private void ShowTasks()
        {
            IReadOnlyList<TaskItem> tasks = taskService.GetTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Задач пока нет");
                WaitForKey();
                return;
            }
            foreach (TaskItem task in tasks)
            {
                string status = task.IsCompleted ? "Выполнена" : "В процессе";

                Console.WriteLine($"Задача: {task.Title}");
                Console.WriteLine($"Описание: {task.Description}");
                Console.WriteLine($"Статус: {status}");
                Console.WriteLine("=======================");
                Console.WriteLine();
            }
            WaitForKey();
        }

        private int ReadChoice(int min, int max)
        {
            while (true)
            {
                bool isNumber = int.TryParse(Console.ReadLine(), out int choice);

                if (!isNumber || choice < min || choice > max)
                {
                    Console.WriteLine("Выберите вариант из списка.");
                    Console.Write("Ваш выбор: ");
                }
                else
                {
                    return choice;
                }

            }
        }

        private string ReadRequiredText(string prompt) 
        {
            while (true)
            {
                Console.Write(prompt);
                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Поле не может быть пустым");
                }
                else
                {
                    return userInput;
                }
            }
        }

        private void WaitForKey()
        {
            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
