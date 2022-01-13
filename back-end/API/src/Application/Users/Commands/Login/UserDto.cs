using API.Application.Common.Mappings;
using API.Domain.Entities;
using System;
using System.Collections.Generic;

namespace API.Application.Users.Commands.Login
{
    public class UserDto : IMapFrom<User>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public long RoleId { get; set; }

        public string Role { get; set; }

        public string Password { get; set; }

        public List<Subject> Subjects { get; set; }

        public List<long> SubjectIds { get; set; }
    }
}
