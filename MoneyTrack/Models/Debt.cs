using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTrack.Models
{
    public class Debt
    {
        public int DebtId { get; set; }
        public string Debttitle { get; set; }

        public int UserId { get; set; }
        public float Debtamount { get; set; }
        public DateTime Debtdate { get; set; }

        public string Debttags { get; set; }

        public string Debtremarks { get; set; }
    }
}
