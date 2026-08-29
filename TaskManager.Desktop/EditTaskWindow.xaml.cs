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
    /// Логика взаимодействия для EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        private readonly TaskItem task;
        private readonly TaskService taskService;
        public EditTaskWindow(TaskItem task, TaskService taskService)
        {
            InitializeComponent();

            this.task = task;
            this.taskService = taskService;

            PriorityComboBox.ItemsSource = Enum.GetValues<TaskPriority>();

            TitleTextBox.Text = task.Title;
            DescriptionTextBox.Text = task.Description;
            PriorityComboBox.SelectedItem = task.Priority;
            if (task.Deadline.HasValue)
            {
                DeadlineDatePicker.SelectedDate = task.Deadline.Value.Date;
                DeadlineTimeTextBox.Text = task.Deadline.Value.ToString("HH:mm");
            }

        }

        private void SaveTaskButton_Click(object sender, RoutedEventArgs e)
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
                    MessageBox.Show("Неверный формат времени");
                    return;
                }

                deadline = deadline.Value.Date + deadlineTime;
            }

            taskService.EditTitle(task.Id, title);
            taskService.EditDescription(task.Id, description);
            taskService.EditPriority(task.Id, priority);
            taskService.EditDeadline(task.Id, deadline);

            Close();
        }

    }
}
