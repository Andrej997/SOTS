using API.Application.Common.Mappings;
using API.Domain.Entities;
using System;

namespace API.Application.Users.Commands.Login
{
    public class UserDto : IMapFrom<User>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Username { get; set; }

        public int RoleId { get; set; }
    }
}
