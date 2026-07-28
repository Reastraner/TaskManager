namespace TaskManager
{
    internal class TaskItem
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsCompleted { get; set; }
        public TaskItem(string title, string description)
        {
            Title = title;
            Description = description;            
        }

        public void Update(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
