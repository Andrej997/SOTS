using System;

namespace API.Domain.Entities
{
    public class ChoosenAnswer
    {
        public long StudentTestId { get; set; }

        public long QuestionId { get; set; }

        public long AnswerId { get; set; }

        public DateTime AnswerDated { get; set; }
    }
}
