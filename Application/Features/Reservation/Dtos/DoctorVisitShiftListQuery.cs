using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Reservation.Dtos
{
    public class DoctorVisitShiftListQuery
    {
        public DateOnly? FromDate { get; set; }

        public DateOnly? ToDate { get; set; }

        public bool FutureOnly { get; set; } = true;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }

}
