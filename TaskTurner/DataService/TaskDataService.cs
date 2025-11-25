using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TaskTurner.Models;

namespace TaskTurner.DataService
{
    public class TaskDataService
    {
        private readonly string _filePath;
        private readonly string folderName = "TaskTurner";
        private readonly string filename = "tasks.json";

        public TaskDataService()
        {
            //Get the path to the app data
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //get the folder of the application in roaming
            string appFolder = Path.Combine(appDataPath, folderName);
            //get the data folder inside the app
            string dataFolder = Path.Combine(appFolder, "data");

            //check if the data folder exists
            if (!Directory.Exists(dataFolder)) {
                Directory.CreateDirectory(dataFolder);
            }

            //define the path to the json file
            _filePath = Path.Combine(dataFolder, filename);

            //ensure the json file exists
            InitializerFile();
        }

        private void InitializerFile()
        {
            //Check if the file exists
            if (!File.Exists(_filePath)) {
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(new List<Task>()));
            }

            //for debug purposes
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName));
        }

        public List<Task> LoadTasks()
        {
            //read and deserialise the JSON file
            string fileContent = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Task>>(fileContent);
        }

        public void SaveTasks(List<Task> tasks)
        { 
            //serialize and write the list of tasks to the JSON file
            string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public void AddTask(Task newTask)
        {
            newTask.Id = GenerateNewTaskId();

            var tasks = LoadTasks();
            tasks.Add(newTask);
            SaveTasks(tasks);
        }

        public void UpdateTask(Task updateTask)
        {
            var tasks = LoadTasks();
            var taskIndex = tasks.FindIndex(t=>t.Id == updateTask.Id);

            if (taskIndex != -1)
            {
                tasks[taskIndex] = updateTask;
                SaveTasks(tasks);
            }
        }

        public void DeleteTask(int taskId)
        {
            var tasks = LoadTasks();
            tasks.RemoveAll(t=>t.Id == taskId);
            SaveTasks(tasks);
        }

        public int GenerateNewTaskId()
        {
            var tasks = LoadTasks();
            if (!tasks.Any()) { 
                return 1;
            }
            int maxId = tasks.Max(task => task.Id);
            return maxId + 1;
        }

    }
}
