using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Entities
{
    public class StudentTest
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long TestId { get; set; }

        public double Points { get; set; }

        public long GradeId { get; set; }

        public DateTime TestStarted { get; set; }

        public DateTime TestFinished { get; set; }


        public virtual Grade Grade { get; set; }
        public virtual User User { get; set; }
        public virtual Test Test { get; set; }
    }
}
