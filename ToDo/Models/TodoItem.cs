using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo
{
    internal class TodoItem
    {
        public int Id { get; set; }
        public string Project {  get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public Status Status { get; set; }

        static public int currentId = 0;

        public TodoItem (string Project, string Title, DateTime DueDate, Status Status) {
            this.Project = Project;
            this.Title = Title;
            this.DueDate = DueDate;
            this.Status = Status;
            currentId++;
            this.Id = currentId;
        }
    }
}
