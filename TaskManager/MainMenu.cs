using System.Globalization;

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
                int userChoice = ReadChoice(0, 8);

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
                    case 7:
                        FindTasks();
                        break;
                    case 8:
                        PriorityFinder();
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
            Console.WriteLine("7 - Поиск задачи.");
            Console.WriteLine("8 - Фильтрация по приоритету.");
            Console.WriteLine();
            Console.WriteLine("0 - Выход");
            Console.WriteLine();
            Console.Write("Ваш выбор: ");
        }

        private void CreateTask()
        {            
            string title = ReadRequiredText("Введите название задачи: ");
            string description = ReadRequiredText("Введите описание задачи: ");
            TaskPriority priority = ReadPriority();
            DateTime? deadline = SetDeadline(false);

            TaskItem newTask = new TaskItem(title, description, priority, deadline);
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
                string status;
                if (task.IsCompleted)
                {
                    status = "Выполнена";
                }
                else if (task.Deadline != null && task.Deadline < DateTime.Now)
                {
                    status = "В процессе - ПРОСРОЧЕНА";
                }
                else
                {
                    status = "В процессе";
                }
                string hasDeadline = task.Deadline == null ? "Срок: не задан." : $"Срок: {task.Deadline:dd.MM.yyyy HH:mm}"; 

                Console.WriteLine($"Задача: {task.Title}");
                Console.WriteLine($"Описание: {task.Description}");
                Console.WriteLine($"Приоритет: {GetPriorityText(task.Priority)}");
                Console.WriteLine($"Статус: {status}");
                Console.WriteLine($"Создана: {task.CreatedAt:dd.MM.yyyy HH:mm}");
                Console.WriteLine(hasDeadline);
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

            Console.WriteLine("1 - Изменить название.");
            Console.WriteLine("2 - Изменить описание.");
            Console.WriteLine("3 - Изменить приоритет.");
            Console.WriteLine("4 - Изменить срок выполнения.");
            Console.WriteLine();
            Console.WriteLine("0 - Назад.");
            int editChoice = ReadChoice(0, 4);

            switch (editChoice)
            {
                case 0:
                    return;
                case 1:
                    string newTitle = ReadRequiredText("Введите новое название: ");
                    bool resultTitle = taskService.EditTitle(userChoice, newTitle);
                    if (resultTitle)
                    {
                        Console.WriteLine("Название задачи успешно изменено!");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка!");
                    }
                    break;
                case 2:
                    string newDescription = ReadRequiredText("Введите новое описание: ");
                    bool resultDescription = taskService.EditDescription(userChoice, newDescription);
                    if (resultDescription)
                    {
                        Console.WriteLine("Описание задачи успешно изменено!");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка!");
                    }
                    break;
                case 3:
                    TaskPriority newPriority = ReadPriority();
                    bool resultPriority = taskService.EditPriority(userChoice, newPriority);
                    if (resultPriority)
                    {
                        Console.WriteLine("Приоритет задачи успешно изменён!");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка!");
                    }
                    break;
                case 4:
                    DateTime? newDeadline = SetDeadline(true);
                    taskService.EditDeadline(userChoice,newDeadline);
                    break;
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

        private void FindTasks()
        {
            string searchText = ReadRequiredText("Введите текст для поиска: ");
            IReadOnlyList<TaskItem> foundTasks = taskService.FindTasks(searchText);
            if (foundTasks.Count == 0)
            {
                Console.WriteLine("Нет задач соответствующих условиям поиска...");
                WaitForKey();
                return;
            }
            ShowNumberedList(foundTasks);
            WaitForKey();
        }

        private void PriorityFinder()
        {
            TaskPriority priority = ReadPriority();
            IReadOnlyList<TaskItem> filteredTasks = taskService.FilterByPriority(priority);
            
            if (filteredTasks.Count == 0)
            {
                Console.WriteLine("Задач с выбранным приоритетом нет.");
                WaitForKey();
                return;
            }
            ShowNumberedList(filteredTasks);
            WaitForKey();
            return;
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

        private DateTime? SetDeadline(bool isEdit)
        {
            while (true)
            {
                Console.WriteLine("Желаете добавить дату и время выполнения задачи?");
                Console.WriteLine("1 - Да.");
                Console.WriteLine("2 - Нет.");
                Console.WriteLine();
                Console.Write("Ваш ответ: ");
                int userChoice = ReadChoice(1, 2);
                switch (userChoice)
                {
                    case 1:
                        string userDeadlineChoice;
                        if (isEdit)
                        {
                            Console.Write("Введите новую дату выполнения в формате число.месяц.год: ");
                            userDeadlineChoice = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(userDeadlineChoice))
                            {
                                return null;
                            }
                        }
                        else 
                        { 
                            userDeadlineChoice = ReadRequiredText("Введите дату выполнения: "); 
                        }
                        bool isDate = DateTime.TryParseExact(userDeadlineChoice, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime deadlineDate);
                        while (!isDate)
                        {
                            Console.WriteLine("Неверный формат ввода даты");
                            Console.WriteLine();
                            userDeadlineChoice = ReadRequiredText("Введите дату выполнения: ");
                            isDate = DateTime.TryParseExact(userDeadlineChoice, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out deadlineDate);
                        }
                        Console.Write("Введите время выполнения задачи в формате часы:минуты, или нажмите Enter, чтобы оставить 23:59.");
                        string userTimeChoice = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(userTimeChoice))
                        {
                            deadlineDate = deadlineDate.AddHours(23).AddMinutes(59);
                        }
                        else
                        {
                            bool isTime = DateTime.TryParseExact(userTimeChoice, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime deadlineTime);

                            while (!isTime)
                            {
                                Console.WriteLine("Неверный формат ввода времени.");
                                Console.WriteLine();
                                Console.Write("Введите время в формате часы:минуты: ");
                                userTimeChoice = Console.ReadLine();
                                isTime = DateTime.TryParseExact(userTimeChoice, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out deadlineTime);
                            }
                            deadlineDate = deadlineDate.AddHours(deadlineTime.Hour).AddMinutes(deadlineTime.Minute);
                        }

                        if (deadlineDate < DateTime.Now)
                        {
                            Console.WriteLine("Указанные дата и время уже прошли, желаете создать задачу с таким сроком выполнения?");
                            Console.WriteLine("1 - Да");
                            Console.WriteLine("2 - Нет");
                            int dateChoice = ReadChoice(1, 2);
                            switch (dateChoice)
                            {
                                case 1:
                                    return deadlineDate;
                                case 2:
                                    continue;
                            }
                        }
                        return deadlineDate;
                    case 2:
                        return null;
                }
                return null;
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
                Console.WriteLine($"{i + 1} - {tasks[i].Title} [{GetPriorityText(tasks[i].Priority)}]");
            }
        }

        private TaskPriority ReadPriority()
        {
            Console.WriteLine("Выберите приоритет для задачи: ");
            Console.WriteLine("1 - Низкий.");
            Console.WriteLine("2 - Средний.");
            Console.WriteLine("3 - Высокий. ");

            int userChoice = ReadChoice(1, 3);

            switch (userChoice) 
            {
                case 1:
                    return TaskPriority.Low;
                case 2:
                    return TaskPriority.Medium;
                case 3:
                    return TaskPriority.High;
            }
            return TaskPriority.Medium;
        }

        private string GetPriorityText(TaskPriority priority) 
        {
            switch (priority)
            {
                case TaskPriority.Low:
                    return "Низкий";
                case TaskPriority.Medium:
                    return "Средний";
                case TaskPriority.High:
                    return "Высокий";
            }
            return "Неизвестный";
        }
    }
}
