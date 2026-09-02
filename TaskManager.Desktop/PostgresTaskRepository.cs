using Npgsql;
using System.Windows;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace TaskManager.Desktop
{    
    internal class PostgresTaskRepository
    {
        public readonly string connectionString;

        public PostgresTaskRepository()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true).AddUserSecrets<PostgresTaskRepository>().Build();

            connectionString = configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException("Строка подключения не найдена");
        }

        public void TestSelect()
        {
            using NpgsqlConnection select = new NpgsqlConnection(connectionString);
            select.Open();

            using NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM tasks", select);

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                MessageBox.Show($"{reader["id"]} | {reader["title"]} | {reader["priority"]}");
            }
        }

        public TaskData Load()
        {
            TaskData data = new TaskData();

            using NpgsqlConnection connection = new NpgsqlConnection( connectionString);
            connection.Open();

            using NpgsqlCommand command = new NpgsqlCommand("SELECT id, title, description, priority, deadline, is_completed, created_at FROM tasks", connection);
            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                TaskItem task = new TaskItem(
                    reader.GetString(reader.GetOrdinal("title")),
                    reader.GetString(reader.GetOrdinal("description")),
                    Enum.Parse<TaskPriority>(
                        reader.GetString(reader.GetOrdinal("priority"))),
                    reader.IsDBNull(reader.GetOrdinal("deadline")) ? null : reader.GetDateTime(reader.GetOrdinal("deadline")),
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")));

                task.IsCompleted = reader.GetBoolean(reader.GetOrdinal("is_completed"));

                data.Tasks.Add(task);

                if (task.Id > data.LastId)
                {
                    data.LastId = task.Id;
                }
            }

            return data;
        }
    }
}
