using API.Application.Common.Mappings;
using API.Domain.Entities;

namespace API.Application.Tests.Queries.GetSubjects
{
    public class SubjectDto : IMapFrom<Subject>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
