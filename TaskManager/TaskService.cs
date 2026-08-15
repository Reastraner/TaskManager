using System.Text.Json;
using System.Text.Encodings.Web;

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

        public bool MarkAsCompleted(int taskID)
        {
            TaskItem? selectedTask = FindTaskById(taskID);

            if (selectedTask != null)
            {
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

        public bool EditTitle(int taskID, string newTitle)
        {
            TaskItem? selectedTask = FindTaskById(taskID);

            if (selectedTask != null)
            {
                selectedTask.UpdateTitle(newTitle);
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool EditDescription(int taskID, string newDescription)
        {
            TaskItem? selectedTask = FindTaskById(taskID);

            if (selectedTask != null)
            {
                selectedTask.UpdateDescription(newDescription);
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool EditPriority(int taskID, TaskPriority newPriority)
        {
            TaskItem? selectedTask = FindTaskById(taskID);

            if (selectedTask != null)
            {
                selectedTask.UpdatePriority(newPriority);
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool EditDeadline(int taskID, DateTime? newDeadline)
        {
            TaskItem? selectedTask = FindTaskById(taskID);

            if (selectedTask != null)
            {
                selectedTask.UpdateDeadline(newDeadline);
                SaveTasks();
                return true;
            }
            return false;
        }

        public bool DeleteTask(int taskID)
        {
            TaskItem? selectedTask = FindTaskById(taskID);
            if (selectedTask != null )
            {
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

        private TaskItem? FindTaskById(int taskId)
        {
            return tasks.Find(task => task.Id ==  taskId);
        }

        public bool TaskExists(int taskId)
        {
            return FindTaskById != null;
        }
    }
}
