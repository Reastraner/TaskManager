namespace TaskManager
{
    internal interface ITaskRepository
    {
        TaskData Load();
        void Save(TaskData data);
    }
}
