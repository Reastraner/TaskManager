using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskManager.Desktop
{
    /// <summary>
    /// Логика взаимодействия для AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private readonly TaskService taskService;
        public AddTaskWindow(TaskService taskService)
        {
            InitializeComponent();
            this.taskService = taskService;

            PriorityComboBox.ItemsSource = Enum.GetValues<TaskPriority>();
            PriorityComboBox.SelectedIndex = 1;
        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название задачи!");
                return;
            }
            
            string description = DescriptionTextBox.Text;

            TaskPriority priority = (TaskPriority)PriorityComboBox.SelectedItem;

            DateTime? deadline = DeadlineDatePicker.SelectedDate;

            if (deadline.HasValue)
            {
                bool isTimeValid = TimeSpan.TryParseExact(
                    DeadlineTimeTextBox.Text,
                    @"hh\:mm",
                    CultureInfo.InvariantCulture,
                    TimeSpanStyles.None,
                    out TimeSpan deadlineTime);

                if (!isTimeValid)
                {
                    MessageBox.Show("Неверный формат времени!");
                    return;
                }
                
                deadline = deadline.Value.Date + deadlineTime;
            }

            taskService.AddTask(title, description, priority, deadline);
            Close();
        }

    }
}
