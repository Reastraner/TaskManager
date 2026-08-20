using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Desktop
{
    internal class MemoryTaskRepository : ITaskRepository
    {
        public TaskData Load()
        {
            return new TaskData();
        }
       
        public void Save(TaskData data)
        {

        }
    }
}
