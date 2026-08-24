using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManager.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TaskService taskService;
        public MainWindow()
        {
            InitializeComponent();

            taskService = new TaskService(new MemoryTaskRepository());
            taskService.AddTask("Первая задача для WPF","Тестовая задача для WPF", TaskPriority.Medium, null);

            TaskList.ItemsSource = taskService.GetTasks();
            ActiveTaskList.ItemsSource = taskService.GetTasks().Where(task => !task.IsCompleted);
            CompletedTaskList.ItemsSource = taskService.GetTasks().Where(task => task.IsCompleted);
        }
    }
}