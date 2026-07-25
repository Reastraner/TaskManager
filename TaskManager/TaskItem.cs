namespace TaskManager
{
    internal class TaskItem
    {
        public string Title { get; }
        public string Description { get; }
        public bool IsCompleted { get; set; }
        public TaskItem(string title, string description)
        {
            Title = title;
            Description = description;            
        }
    }
}
