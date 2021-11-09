using System;

namespace API.Domain.Entities
{
    public class QuestionTime
    {
        public long QuestionId { get; set; }

        public long StudentTestsId { get; set; }

        public DateTime QuestionStart { get; set; }

        public DateTime QuestionEnd { get; set; }
    }
}
