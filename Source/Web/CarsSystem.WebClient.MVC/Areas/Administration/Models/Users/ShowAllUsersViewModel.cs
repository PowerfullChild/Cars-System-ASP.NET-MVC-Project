﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarsSystem.WebClient.MVC.Areas.Administration.Models.Users
{
    public class ShowAllUsersViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }
    }
}