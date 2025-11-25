using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TaskTurner.DataService;
using TaskTurner.Models;

namespace TaskTurner.ViewModels
{
    internal class TaskViewModel : INotifyPropertyChanged
    {
        private readonly TaskDataService _taskDataService;

        private ObservableCollection<Task> _task;

        public ObservableCollection<Task> Tasks
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadTasks()
        {
            var taskList = _taskDataService.LoadTasks();
            Tasks = new ObservableCollection<Task>(taskList);
        }

        public void AddNewTask(Task task)
        {
            _taskDataService.AddTask(task);
            LoadTasks();
        }

        public void UpdateTask(Task task)
        {
            _taskDataService.UpdateTask(task);
            LoadTasks();
        }

        public void DeleteTask(int taskId)
        {
            _taskDataService.DeleteTask(taskId);
            LoadTasks();
        }

        public TaskViewModel()
        {
            _taskDataService = new TaskDataService();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
