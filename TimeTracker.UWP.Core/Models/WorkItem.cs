using System;
using System.Collections.Generic;

namespace TimeTracker.UWP.Core.Models
{
   
    public class WorkItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Completed { get; set; }

    }
}
