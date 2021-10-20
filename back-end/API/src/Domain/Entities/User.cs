﻿using System;

namespace API.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }

        public int RoleId { get; set; }
    }
}
