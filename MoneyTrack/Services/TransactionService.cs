using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoneyTrack.Models;

namespace MoneyTrack.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly string transactionFilePath = Path.Combine(AppContext.BaseDirectory, "transactions.json");
        private List<Transaction> transactions = new();

        public TransactionService()
        {
            LoadTransactions(); // Load transactions when the service is initialized
        }

        // Method to get all transactions for a specific user
        public List<Transaction> GetTransactionsByUserId(int userId)
        {
            return transactions.Where(t => t.UserId == userId).ToList();
        }

        // Method to add a new transaction (Debt, Credit, Expense)
        public void AddTransaction(Transaction transaction)
        {
            transactions.Add(transaction);
            SaveTransactions(); // Save the updated transactions to the file
        }

        // Method to sort transactions by type (Debt, Credit, Expense)
        public List<Transaction> SortTransactionsByType(string type)
        {
            return transactions.Where(t => t.transactiontype.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Method to filter transactions by date range (e.g., recent transactions)
        public List<Transaction> FilterTransactionsByDate(DateTime startDate, DateTime endDate)
        {
            return transactions.Where(t => t.transactiondate >= startDate && t.transactiondate <= endDate).ToList();
        }

        // Method to sort transactions by amount (highest to lowest or vice versa)
        public List<Transaction> SortTransactionsByAmount(bool descending = true)
        {
            return descending
                ? transactions.OrderByDescending(t => t.transactionamount).ToList()
                : transactions.OrderBy(t => t.transactionamount).ToList();
        }

        // Load transactions from file
        private void LoadTransactions()
        {
            if (File.Exists(transactionFilePath))
            {
                try
                {
                    var json = File.ReadAllText(transactionFilePath);
                    transactions = System.Text.Json.JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading transactions: {ex.Message}");
                    transactions = new List<Transaction>();
                }
            }
            else
            {
                transactions = new List<Transaction>();
                SaveTransactions();
            }
        }

        // Save transactions to the file
        private void SaveTransactions()
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(transactions, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(transactionFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
            }
        }
    }
}
