using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTrack.Models;

namespace MoneyTrack.Services
{
    public interface IDebtService
    {
        // Method to add a new debt
        Task AddDebtAsync(Debt debt);

        // Method to retrieve debts by user ID
        Task<List<Debt>> GetDebtsByUserIdAsync(int userId);

        // Method to remove a debt by its ID
        Task PayDebtAsync(int debtId, float paymentAmount);
        Task<List<Debt>> GetPendingDebtsAsync(int userId);
    }
}