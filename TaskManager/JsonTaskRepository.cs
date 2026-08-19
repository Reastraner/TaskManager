using System.Text.Encodings.Web;
using System.Text.Json;

namespace TaskManager
{
    internal class JsonTaskRepository : ITaskRepository
    {
        private static readonly string DataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "My Task Manager");
        private static readonly string FilePath = Path.Combine(DataFolderPath, "tasks.json");

        public TaskData Load()
        {
            if (!File.Exists(FilePath))
            {
                return new TaskData();
            }

            string json = File.ReadAllText(FilePath);

            try
            {
                TaskData? loadedData = JsonSerializer.Deserialize<TaskData>(json);

                if (loadedData == null)
                {
                    return new TaskData();
                }
                else
                {
                    return loadedData;
                }
            }
            catch(JsonException)
            {
                // Повреждённые данные считаем некорректным сохранением.
                // TaskService получает пустое состояние.
                return new TaskData();
            }
        }

        public void Save(TaskData data)
        {
            Directory.CreateDirectory(DataFolderPath);
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(data, options);

            File.WriteAllText(FilePath, json);
        }
    }
}
