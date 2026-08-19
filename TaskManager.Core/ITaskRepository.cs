namespace TaskManager
{
    public interface ITaskRepository
    {
        TaskData Load();
        void Save(TaskData data);
    }
}
