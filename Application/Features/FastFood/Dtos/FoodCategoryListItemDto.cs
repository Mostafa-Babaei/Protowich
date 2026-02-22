using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FastFood.Dtos
{
    public class FoodCategoryListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public int ItemsCount { get; set; }
    }
}
