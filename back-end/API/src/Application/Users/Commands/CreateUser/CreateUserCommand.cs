using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public long RoleId { get; set; }

        public List<long> SubjectIds { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateUserCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var user = _context.Users
                   .Add(new User
                   {
                       Name = request.Name,
                       Surname = request.Surname,
                       Username = request.Username,
                       PasswordHash = passwordHash,
                       CreatedAt = _dateTime.UtcNow
                   });

                await _context.SaveChangesAsync(cancellationToken);

                _context.UserRoles.Add(new UserRole
                {
                    UserId = user.Entity.Id,
                    RoleId = request.RoleId
                });

                foreach (var subjectId in request.SubjectIds)
                {
                    _context.UserSubjects.Add(new UserSubject
                    {
                        UserId = user.Entity.Id,
                        SubjectId = subjectId
                    });
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
