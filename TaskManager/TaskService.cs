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

        public bool MarkAsCompleted(int taskNumber)
        {
            if (taskNumber >= 1 && taskNumber <= tasks.Count)
            {
                int index = taskNumber - 1;
                tasks[index].IsCompleted = true;
                return true;
            }
            return false;
        }
    }
}
