using API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Common.Models
{
    public class NodeDto : Node
    {
        public string CustomColor { get; set; }
    }
}
