using System.Text.Json.Serialization;

namespace TaskManager
{
    internal class TaskItem
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsCompleted { get; set; }
        public TaskPriority Priority { get; private set; }
        public DateTime? Deadline { get; private set; }

        [JsonInclude]
        public DateTime CreatedAt { get; private set; }
        
        public TaskItem(string title, string description, TaskPriority priority, DateTime? deadline)
                {
            Title = title;
            Description = description;
            Priority = priority;
            CreatedAt = DateTime.Now;
            Deadline = deadline;
        }

        public void UpdateTitle(string title)
        {
            Title = title;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdatePriority(TaskPriority priority)
        {
            Priority = priority;
        }
    }
}
