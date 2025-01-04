using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MoneyTrack.Models
{
    public class Credit
    {
        public int CreditId { get; set; }

        public int UserId { get; set; }
        public string Credittitle { get; set; }
        public float Creditamount { get; set; }
        public DateTime Creditdate { get; set; }

        public string Credittags { get; set; }

        public string Creditremarks { get; set; }
    }
}

