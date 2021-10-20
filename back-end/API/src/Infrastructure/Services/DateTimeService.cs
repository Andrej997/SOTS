using API.Application.Common.Interfaces;
using System;

namespace API.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
