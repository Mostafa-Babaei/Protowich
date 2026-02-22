using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FastFood.Dtos
{
    public class FoodImageUpsertDto
    {
        public int FoodItemId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }

}
