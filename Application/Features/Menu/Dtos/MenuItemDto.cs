using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Menu.Dtos
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; } = "";
        public string? Icon { get; set; }
        public string? Route { get; set; }
        public int DisplayOrder { get; set; }
        public List<MenuItemDto> Children { get; set; } = new();
    }

}
