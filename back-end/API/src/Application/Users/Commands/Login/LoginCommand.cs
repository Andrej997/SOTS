using API.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Commands.Login
{
    public class LoginCommand : IRequest<UserDto>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LoginCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                var passwordHash = _context.Users
                    .Where(user => user.Username == request.Username)
                    .Select(user => user.PasswordHash)
                    .FirstOrDefault();

                if (passwordHash == null || passwordHash == "") throw new Exception("Unknown username or password");

                bool verified = BCrypt.Net.BCrypt.Verify(request.Password, passwordHash);
                if (verified == false) throw new Exception("Unknown username or password");

                var user = _context.Users
                    .Where(user => user.Username == request.Username)
                    .Select(user => new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Name = user.Name,
                        Surname = user.Surname,
                        RoleId = _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).FirstOrDefault()
                    })
                    .FirstOrDefault();

                if (user == null) throw new Exception("Unknown user");

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
