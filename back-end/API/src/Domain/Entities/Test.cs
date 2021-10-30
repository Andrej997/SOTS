using System;
using System.Collections.Generic;

namespace API.Domain.Entities
{
    public class Test
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long SubjectId { get; set; }

        public long CreatorId { get; set; }

        public DateTime CreatedAt { get; set; }

        public long TestTimeId { get; set; }

        public long MaxPoints { get; set; }


        public virtual List<Question> Questions { get; set; }
        public virtual TestTime TestTime { get; set; }
    }
}
