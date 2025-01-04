using System;
using System.Collections.Generic;
using MoneyTrack.Models;

namespace MoneyTrack.Services
{
    public interface ITransactionService
    {
        
        List<Transaction> GetTransactionsByUserId(int userId);

       
        void AddTransaction(Transaction transaction);

        List<Transaction> SortTransactionsByType(string type);

 
        List<Transaction> FilterTransactionsByDate(DateTime startDate, DateTime endDate);

     
        List<Transaction> SortTransactionsByAmount(bool descending = true);
    }
}
