namespace TaskManager
{
    internal class TaskData
    {
        public int LastId { get; set; }
        public List<TaskItem> Tasks { get; set; } = [];
    }
}
