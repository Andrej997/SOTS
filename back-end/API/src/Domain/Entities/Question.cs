using System;
using System.Collections.Generic;

namespace API.Domain.Entities
{
    public class Question
    {
        public long Id { get; set; }

        public string TextQuestion { get; set; }

        public DateTime CreatedAt { get; set; }

        public long TestId { get; set; }

        public long Points { get; set; }

        public string Image { get; set; }


        public virtual List<Answer> Answers { get; set; }
    }
}
