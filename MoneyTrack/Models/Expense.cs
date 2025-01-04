using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTrack.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        public int UserId { get; set; }
        public string Expensetitle { get; set; }
        public float Expenseamount { get; set; }
        public DateTime Expensedate { get; set; }

        public string Expensetags { get; set; }

        public string Expenseremarks { get; set; }
    }
}
