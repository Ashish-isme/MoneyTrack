using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTrack.Models;

namespace MoneyTrack.Services
{
    public interface IExpenseService
    {
 
        Task AddExpenseAsync(Expense expense);

        Task<List<Expense>> GetExpensesByUserIdAsync(int userId);
    }
}
