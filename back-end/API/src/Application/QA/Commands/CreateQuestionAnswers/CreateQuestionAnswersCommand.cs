using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Application.QA.Commands.CreateQuestionAnswers
{
    public class CreateQuestionAnswersCommand : IRequest
    {
        public string TextQuestion { get; set; }

        public List<Answer> Answers { get; set; }

        public long CreatorId { get; set; }
    }

    public class CreateQuestionAnswersCommandHandler : IRequestHandler<CreateQuestionAnswersCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateQuestionAnswersCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Unit> Handle(CreateQuestionAnswersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!_context.UserRoles.Any(ur => ur.UserId == request.CreatorId))
                    throw new Exception("Creator not found!");

                if (_context.UserRoles.Any(ur => ur.UserId == request.CreatorId && (ur.RoleId != (long)API.Domain.Enums.Roles.admin && ur.RoleId != (long)API.Domain.Enums.Roles.proffesor)))
                    throw new Exception("Not allowd to create a test!");

                foreach (var answer in request.Answers)
                {
                    if (answer.TextAnswer == null || answer.TextAnswer == "")
                        throw new Exception("Empty answer not allowed!");
                }

                var question = _context.Questions
                   .Add(new Question
                   {
                       TextQuestion = request.TextQuestion,
                       CreatedAt = _dateTime.UtcNow,
                   });

                await _context.SaveChangesAsync(cancellationToken);

                foreach (var answer in request.Answers)
                {
                    var answerDb = _context.Answers
                      .Add(new Answer
                      {
                          TextAnswer = answer.TextAnswer,
                          QuestionId = question.Entity.Id
                      });

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
