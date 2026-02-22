using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Menu.Dtos
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? ParentId { get; set; }
    }
}
