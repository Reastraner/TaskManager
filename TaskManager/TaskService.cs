using System.Text.Json;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;

namespace TaskManager
{
    internal class TaskService
    {
        private readonly List<TaskItem> tasks = new List<TaskItem>();
        private int lastId;
        private static readonly string DataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "My Task Manager");
        private static readonly string FilePath = Path.Combine(DataFolderPath, "tasks.json");

        public TaskService()
        {
            LoadTasks();
        }

        public void AddTask(string title, string description, TaskPriority priority, DateTime? deadline)
        {
            lastId += 1;
            TaskItem task = new TaskItem(title, description,priority, deadline, lastId);

            tasks.Add(task);
            SaveTasks();
        }

        public IReadOnlyList<TaskItem> GetTasks()
        {
            return tasks.OrderByDescending(tasks => tasks.Priority).ToList();
        }

        public IReadOnlyList<TaskItem> FindTasks (string searchText)
        {
            return tasks.Where(task => task.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IReadOnlyList<TaskItem> FilterByPriority (TaskPriority priority)
        {
            return tasks.Where(task => task.Priority == priority).ToList();
        }

        public bool MarkAsCompleted(int taskNumber)
        {
            IReadOnlyList<TaskItem> sortedTasks = GetTasks();

            if (taskNumber >= 1 && taskNumber <= sortedTasks.Count)
            {
                TaskItem selectedTask = sortedTasks[taskNumber - 1];
                if (selectedTask.IsCompleted)
                {
                    return false;
                }

                selectedTask.IsCompleted = true;
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool EditTitle(int taskNumber, string newTitle)
        {
            IReadOnlyList<TaskItem> sortedTasks = GetTasks();
            if(taskNumber >= 1 && taskNumber <= sortedTasks.Count)
            {
                TaskItem selectedTask = sortedTasks[taskNumber - 1];

                selectedTask.UpdateTitle(newTitle);
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool EditDescription(int taskNumber, string newDescription)
        {
            IReadOnlyList<TaskItem> sortedTasks = GetTasks();
            if (taskNumber >= 1 && taskNumber <= sortedTasks.Count)
            {
                TaskItem selectedTask = sortedTasks[taskNumber - 1];

                selectedTask.UpdateDescription(newDescription);
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool EditPriority(int taskNumber, TaskPriority newPriority)
        {
            IReadOnlyList<TaskItem> sortedTasks = GetTasks();
            if (taskNumber >= 1 &&  taskNumber <= sortedTasks.Count)
            {
                TaskItem selectedTask = sortedTasks[taskNumber - 1];

                selectedTask.UpdatePriority(newPriority);
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool EditDeadline(int taskNumber, DateTime? newDeadline)
        {
            IReadOnlyList<TaskItem> sortedTasks = GetTasks();
            if (taskNumber >= 1 && taskNumber <= sortedTasks.Count)
            {
                TaskItem selectedTask = sortedTasks[taskNumber - 1];

                selectedTask.UpdateDeadline(newDeadline);
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool DeleteTask(int taskNumber)
        {
            IReadOnlyList<TaskItem> sortedTasks = GetTasks();

            if (taskNumber >= 1 && taskNumber <= sortedTasks.Count)
            {
                TaskItem selectedTask = sortedTasks[taskNumber - 1];
                tasks.Remove(selectedTask);
                SaveTasks();
                return true;
            }
            return false;
        }

        private void SaveTasks()
        {
            Directory.CreateDirectory(DataFolderPath);
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            TaskData taskData = new TaskData();
            taskData.Tasks = tasks;
            taskData.LastId = lastId;

            string json = JsonSerializer.Serialize(taskData, options);

            File.WriteAllText(FilePath, json);
        }

        private void LoadTasks()
        {
            if (!File.Exists(FilePath))
            {
                return;
            }

            string json = File.ReadAllText(FilePath);

            try
            {
                TaskData? loadedData = JsonSerializer.Deserialize<TaskData>(json);

                if (loadedData != null)
                {
                    lastId = loadedData.LastId;
                    tasks.AddRange(loadedData.Tasks);
                }
            }
            catch (JsonException)
            {
                tasks.Clear();
                SaveTasks();
            }
        }
    }
}
