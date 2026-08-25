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
            LoadTasks();
        }

        private void AddTaskButton_Click(Object sender, RoutedEventArgs e)
        {
            AddTaskWindow addTask = new AddTaskWindow(taskService);
            addTask.Owner = this;
            addTask.ShowDialog();

            LoadTasks();
        }

        private void LoadTasks()
        {
            TaskList.ItemsSource = taskService.GetTasks();
            ActiveTaskList.ItemsSource = taskService.GetTasks().Where(task => !task.IsCompleted);
            CompletedTaskList.ItemsSource = taskService.GetTasks().Where(task => task.IsCompleted);
        }
    }
}