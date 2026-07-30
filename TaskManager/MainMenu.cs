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
                int userChoice = ReadChoice(0, 6);

                switch (userChoice)
                {
                    case 0:
                        menuRunning = false;
                        break;
                    case 1:
                        CreateTask();
                        break;
                    case 2:
                        ShowTasks(false);
                        break;
                    case 3:
                        ShowTasks(true);
                        break;
                    case 4:
                        TaskStatusChange();
                        break;
                    case 5:
                        EditTaskDialog();
                        break;
                    case 6:
                        DeleteTaskDialog();
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
            Console.WriteLine("2 - Просмотреть текущие задачи.");
            Console.WriteLine("3 - Просмотреть выполненные задачи.");
            Console.WriteLine("4 - Изменить статус задачи.");
            Console.WriteLine("5 - Изменить задачу.");
            Console.WriteLine("6 - Удалить задачу.");
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

        private void ShowTasks(bool showCompleted)
        {
            IReadOnlyList<TaskItem> tasks = taskService.GetTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                WaitForKey();
                return;
            }

            bool tasksFound = false;

            foreach (TaskItem task in tasks)
            {
                if (task.IsCompleted != showCompleted)
                {
                    continue;
                }
                tasksFound = true;
                string status = task.IsCompleted ? "Выполнена" : "В процессе";

                Console.WriteLine($"Задача: {task.Title}");
                Console.WriteLine($"Описание: {task.Description}");
                Console.WriteLine($"Статус: {status}");
                Console.WriteLine("=======================");
                Console.WriteLine();
            }
            if (!tasksFound)
            {
                if (showCompleted)
                {
                    Console.WriteLine("Выполненных задач нет.");
                }
                else
                {
                    Console.WriteLine("Текущих задач нет.");
                }
            }
            WaitForKey();
        }

        private void TaskStatusChange()
        {
            IReadOnlyList<TaskItem> tasks = taskService.GetTasks();
            if ( tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                WaitForKey();
                return;
            }

            ShowNumberedList(tasks);

            Console.Write("Введите номер задачи: ");
            int userChoice = ReadChoice(1, tasks.Count);

            bool result = taskService.MarkAsCompleted(userChoice);

            if (result)
            {
                Console.WriteLine($"Задача под номером {userChoice} помечена как выполненная");
            }
            else
            {
                Console.WriteLine("Эта задача уже выполнена.");
            }
            WaitForKey();
        }

        private void EditTaskDialog()
        {
            IReadOnlyList<TaskItem> tasks = taskService.GetTasks();

            if ( tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                WaitForKey();
                return;
            }

            ShowNumberedList(tasks);

            Console.Write("Введите номер задачи: ");
            int userChoice = ReadChoice(1, tasks.Count);

            string newTitle = ReadRequiredText("Введите новое название: ");
            string newDescription = ReadRequiredText("Введите новое описание: ");

            bool result = taskService.EditTask(userChoice, newTitle, newDescription);

            if (result)
            {
                Console.WriteLine("Задача успешно отредактирована.");
            }
            else
            {
                Console.WriteLine("Не удалось найти задачу.");
            }
            
            WaitForKey();
        }

        private void DeleteTaskDialog()
        {
            IReadOnlyList<TaskItem> tasks = taskService.GetTasks();
            
            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                WaitForKey();
                return;
            }

            ShowNumberedList(tasks);

            Console.Write("Введите номер задачи: ");
            int userChoice = ReadChoice(1, tasks.Count);
            bool result = taskService.DeleteTask(userChoice);
            if (result)
            {
                Console.WriteLine($"Задача под номером {userChoice} удалена из списка.");
            }
            else
            {
                Console.WriteLine($"Задача под номером {userChoice} не найдена.");
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

        private void ShowNumberedList(IReadOnlyList<TaskItem> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {tasks[i].Title}");
            }
        }
    }
}
