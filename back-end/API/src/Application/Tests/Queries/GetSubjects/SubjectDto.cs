using API.Application.Common.Mappings;
using API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Tests.Queries.GetSubjects
{
    public class SubjectDto : IMapFrom<Subject>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
