using System;
using System.Collections.Generic;

namespace API.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; }


        public virtual List<StudentTest> StudentTests { get; set; }
    }
}
