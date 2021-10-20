using API.Domain.Common;
using System.Threading.Tasks;

namespace API.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
