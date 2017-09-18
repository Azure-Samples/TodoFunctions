using System;

namespace ToDoFunctions.Entities
{
    public class ToDo
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        
        public DateTime? due { get; set; }
        public bool isComplete { get; set; }
    }
}
