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
        
        private void CompleteTaskButton_Click(Object sender, RoutedEventArgs e)
        {
            TaskItem? selectedTask = ActiveTaskList.SelectedItem as TaskItem;

            if (selectedTask == null)
            {
                MessageBox.Show("Выберите задачу из списка!");
                return;
            }

            taskService.MarkAsCompleted(selectedTask.Id);
            LoadTasks();
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskItem? selectedTask = ActiveTaskList.SelectedItem as TaskItem;

            if (selectedTask == null)
            {
                selectedTask = CompletedTaskList.SelectedItem as TaskItem;
            }

            if (selectedTask == null)
            {
                MessageBox.Show("Выберите задачу из списка!");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить задачу", "Подтверждение удаления", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                taskService.DeleteTask(selectedTask.Id);
                LoadTasks();
                return;
            }
        }

        private void ActiveTaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtons();
        }

        private void CompletedTaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            CompleteTaskButton.Visibility = Visibility.Collapsed;
            DeleteTaskButton.Visibility = Visibility.Collapsed;

            if (ActiveTaskList.SelectedItem != null)
            {
                CompleteTaskButton.Visibility = Visibility.Visible;
                DeleteTaskButton.Visibility = Visibility.Visible;
            }

            else if (CompletedTaskList.SelectedItem != null)
            {
                DeleteTaskButton.Visibility= Visibility.Visible;
            }
        } 
    }
}