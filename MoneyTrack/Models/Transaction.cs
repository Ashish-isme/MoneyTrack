using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTrack.Models
{
    public class Transaction
    {
   
        public int UserId { get; set; }
        public string transactiontitle { get; set; }
        public float transactionamount { get; set; }
        public DateTime transactiondate { get; set; }
        public string transactiontype { get; set; }  // This for type "Credit", "Debt", or "Expense"
        public string transactiontags { get; set; }
        public string transactionremarks { get; set; }
    }
}
