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
                if (tasks[index].IsCompleted)
                {
                    return false;
                }

                tasks[index].IsCompleted = true;
                return true;
            }
            return false;
        }

        public bool EditTask(int taskNumber, string newTitle, string newDescription)
        {
            if (taskNumber >= 1 && taskNumber <= tasks.Count)
            {
                int index = taskNumber - 1;

                tasks[index].Update(newTitle, newDescription);
                return true;
            }
            return false;
        }
    }
}
