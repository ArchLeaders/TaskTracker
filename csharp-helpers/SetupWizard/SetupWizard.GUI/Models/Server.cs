﻿using SetupWizard.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SetupWizard.GUI.Models
{
    public class Server
    {
        /// <summary>
        /// Timezone used when setting off the tasks
        /// </summary>
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = "";

        /// <summary>
        /// Bindable list of tasks
        /// </summary>
        [JsonPropertyName("tasks")]
        public List<TaskModel> Tasks { get; set; } = new();

        /// <summary>
        /// Bindable dictionary of vars
        /// </summary>
        [JsonPropertyName("vars")]
        public Dictionary<string, string> Vars { get; set; } = new();

        public ShellViewModel GetShellBase()
        {
            ShellViewModel shell = new(null);

            for (int i = 0; i < Tasks.Count; i++)
            {
                TaskModel task = Tasks[i];
                TaskViewModel viewTask = new(i);

                foreach (var user in TaskModel.Users)
                    if (user.Key == task.User)
                        viewTask.User = user;

                foreach (var role in TaskModel.Roles)
                    if (role.Key == task.Role)
                        viewTask.Role = role;

                foreach (var user in TaskModel.Channels)
                    if (user.Key == task.Channel)
                        viewTask.Channel = user;

                viewTask.Time = task.Time;
                viewTask.DateTime = DateTime.Parse(task.Time);
                viewTask.Message = task.Message;
                viewTask.Session = task.Session;

                viewTask.Mon = task.Days["Mon"];
                viewTask.Tue = task.Days["Tue"];
                viewTask.Wed = task.Days["Wed"];
                viewTask.Thu = task.Days["Thu"];
                viewTask.Fri = task.Days["Fri"];
                viewTask.Sat = task.Days["Sat"];
                viewTask.Sun = task.Days["Sun"];

                foreach (var var in task.Sequence)
                {
                    viewTask.Sequence += $"{var.Key}(";

                    for (int v = 0; v < var.Value.Count; v++)
                    {
                        if (v+1 == var.Value.Count)
                            viewTask.Sequence += $"{var.Value[v]}); ";
                        else
                            viewTask.Sequence += $"{var.Value[v]}, ";
                    }
                }

                shell.Tasks.Add(viewTask);
            }

            foreach (var var in Vars)
                shell.Vars.Add(new(var.Key, var.Value));

            return shell;
        }
    }
}
