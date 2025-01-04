using MoneyTrack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyTrack.Services
{
    public interface ICreditService
    {
 
        Task AddCreditAsync(Credit credit);

      
        Task<List<Credit>> GetCreditsByUserIdAsync(int userId);

       
    }
}
