using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum VisitStatusEnum 
    {
        InTreatment = 1,   // در حال درمان
        Completed = 2,     // تکمیل شده
        Cancelled = 3,     // لغو شده
        NoShow = 4         // مراجعه نکرد (غیبت)
    }
}
