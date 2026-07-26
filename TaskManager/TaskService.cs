namespace TaskManager
{
    internal class TaskService
    {
        private readonly List<TaskItem> tasks = new List<TaskItem>();

        public void AddTask(TaskItem task)
        {
            tasks.Add(task);
        }

        public IReadOnlyList<TaskItem> GetTasks()
        {
            return tasks;
        }
    }
}
