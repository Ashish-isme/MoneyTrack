using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MoneyTrack.Models;

namespace MoneyTrack.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly string expensesFilePath = Path.Combine(AppContext.BaseDirectory, "Expenses.json");
        private readonly BalanceService balanceService;
        private readonly ITransactionService transactionService;

        public ExpenseService(BalanceService balanceService, ITransactionService transactionService)
        {
            this.balanceService = balanceService;
            this.transactionService = transactionService;
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            // Retrieve the current balance of the user
            var currentBalance = balanceService.GetBalance(expense.UserId);

            // Check if the balance is sufficient
            if (currentBalance < expense.Expenseamount)
            {
                throw new InvalidOperationException("Insufficient balance to fulfill the expense.");
            }

            // Load the existing expenses
            var expenses = await LoadExpensesAsync();
            expense.ExpenseId = expenses.Count > 0 ? expenses.Max(e => e.ExpenseId) + 1 : 1;
            expenses.Add(expense);

            // Save the updated expenses list
            await SaveExpensesAsync(expenses);

            // Deduct the expense amount from the balance
            balanceService.DeductFromBalance(expense.UserId, expense.Expenseamount);

            // Create and add a corresponding transaction for the expense
            var transaction = new Transaction
            {
                UserId = expense.UserId,
                transactiontitle = expense.Expensetitle,
                transactionamount = (float)expense.Expenseamount,
                transactiondate = DateTime.Now,
                transactiontype = "Expense", // Specify the type of transaction
                transactiontags = expense.Expensetags, // Optional
                transactionremarks = expense.Expenseremarks // Optional
            };

            // Add the transaction using the ITransactionService
             transactionService.AddTransaction(transaction);
        }

        public async Task<List<Expense>> GetExpensesByUserIdAsync(int userId)
        {
            var expenses = await LoadExpensesAsync();
            return expenses.Where(e => e.UserId == userId).ToList();
        }

        private async Task<List<Expense>> LoadExpensesAsync()
        {
            if (!File.Exists(expensesFilePath))
            {
                var emptyList = new List<Expense>();
                await SaveExpensesAsync(emptyList);
                return emptyList;
            }

            var json = await File.ReadAllTextAsync(expensesFilePath);
            return JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
        }

        private async Task SaveExpensesAsync(List<Expense> expenses)
        {
            var json = JsonSerializer.Serialize(expenses, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(expensesFilePath, json);
        }
    }
}
