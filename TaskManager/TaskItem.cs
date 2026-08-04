using System.Text.Json.Serialization;

namespace TaskManager
{
    internal class TaskItem
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsCompleted { get; set; }
        public TaskPriority Priority { get; private set; }
        [JsonInclude]
        public DateTime CreatedAt { get; private set; }
        
        public TaskItem(string title, string description, TaskPriority priority)
                {
            Title = title;
            Description = description;
            Priority = priority;
            CreatedAt = DateTime.Now;
        }

        public void Update(string title, string description, TaskPriority priority)
        {
            Title = title;
            Description = description;
            Priority = priority;
        }
    }
}
