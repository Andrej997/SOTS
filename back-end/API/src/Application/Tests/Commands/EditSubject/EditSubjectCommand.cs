using API.Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.Tests.Commands.EditSubject
{
    public class EditSubjectCommand : IRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
    public class EditSubjectCommandHandler : IRequestHandler<EditSubjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public EditSubjectCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(EditSubjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sub = _context.Subjects.Where(s => s.Id == request.Id).FirstOrDefault();

                if (sub != null)
                {
                    if (!_context.UserSubjects.Any(us => us.SubjectId == request.Id))
                    {
                        sub.Name = request.Name;
                        sub.Description = request.Description;
                    }
                    else
                    {
                        sub.Description = request.Description;
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else throw new Exception("Unknown id");

                return Unit.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
