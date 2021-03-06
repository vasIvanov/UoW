﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UoW.Models.Contracts.Requests
{
    public class UserTaskRequest
    {
        public int OwnerId { get; set; }
        public int AssignedToUserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StoryId { get; set; }
        public int MinutesSpended { get; set; }
        public int TaskType { get; set; }
    }
}
