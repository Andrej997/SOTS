using API.Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Queries.GetSubjects
{
    public class GetSubjectsQuery : IRequest<List<SubjectDto>>
    {
    }

    public class GetSubjectsQueryHandler : IRequestHandler<GetSubjectsQuery, List<SubjectDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public GetSubjectsQueryHandler(IApplicationDbContext context,
            IMapper mapper,
            IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
            _mapper = mapper;
        }

        public async Task<List<SubjectDto>> Handle(GetSubjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Subjects
                            .AsNoTracking()
                            .ProjectTo<SubjectDto>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
