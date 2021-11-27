using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Tests.Queries.GetTakeTest
{
    public class TakeTestDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string SubjectName { get; set; }

        public string CreatorName { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public long MaxPoints { get; set; }

        public List<TakeTestQuestionDto> Questions { get; set; }
    }

    public class TakeTestQuestionDto
    {
        public long Id { get; set; }

        public string Question { get; set; }

        public string ProblemNodeId { get; set; }

        public List<TakeTestQuestionAnswerDto> Answers { get; set; }
    }

    public class TakeTestQuestionAnswerDto
    {
        public long Id { get; set; }

        public string Answer { get; set; }


    }
}
