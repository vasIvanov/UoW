﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UoW.Models.Contracts.Responses
{
    public class TeamResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int FacultyID { get; set; }
    }
}
