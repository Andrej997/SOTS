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
                byte[] salt = new byte[128 / 8];
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: request.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                var user = _context.Users
                    .Where(user => user.Username == request.Username && user.PasswordHash == request.Password)
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
