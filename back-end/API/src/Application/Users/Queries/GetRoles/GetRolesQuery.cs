using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Users.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<List<Role>>
    {
    }

    public class GetTestsInfoQueryHandler : IRequestHandler<GetRolesQuery, List<Role>>
    {
        private readonly IApplicationDbContext _context;

        public GetTestsInfoQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            return _context.Roles.ToList();
        }
    }
}
