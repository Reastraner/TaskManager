namespace TaskManager
{
    internal class MainMenu
    {
        private bool menuRunning = true;
        
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
                        Console.WriteLine("Создание задачи в процессе...");
                        break;
                    case 2:
                        Console.WriteLine("Задач еще не создано");
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
    }
}
