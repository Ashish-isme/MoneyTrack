using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MoneyTrack.Models;

namespace MoneyTrack.Services
{
    public class DebtService : IDebtService
    {
        private readonly string debtsFilePath = Path.Combine(AppContext.BaseDirectory, "Debts.json");
        private readonly BalanceService balanceService;
        private readonly ITransactionService transactionService; // Added to handle transactions

        public DebtService(BalanceService balanceService, ITransactionService transactionService)
        {
            this.balanceService = balanceService;
            this.transactionService = transactionService;
        }

        // Add a new debt
        public async Task AddDebtAsync(Debt debt)
        {
            var debts = await LoadDebtsAsync();
            debt.DebtId = debts.Count > 0 ? debts.Max(d => d.DebtId) + 1 : 1;
            debts.Add(debt);
            await SaveDebtsAsync(debts);

            // Add the debt amount to the user's balance (like a loan)
            balanceService.AddCreditToBalance(debt.UserId, debt.Debtamount);

            // Create and add a transaction for the debt
            var transaction = new Transaction
            {
                UserId = debt.UserId,
                transactiontitle = "Debt Added",
                transactionamount = (float)debt.Debtamount,
                transactiondate = DateTime.Now,
                transactiontype = "Debt", // Specify the type of transaction
                transactiontags = debt.Debttags, // Optional
                transactionremarks = debt.Debtremarks // Optional
            };

            // Add the transaction using the ITransactionService
            transactionService.AddTransaction(transaction);
        }

        // Get all debts for a user
        public async Task<List<Debt>> GetDebtsByUserIdAsync(int userId)
        {
            var debts = await LoadDebtsAsync();
            return debts.Where(d => d.UserId == userId).ToList();
        }

        // Get only pending debts for a user (debts with an outstanding amount)
        public async Task<List<Debt>> GetPendingDebtsAsync(int userId)
        {
            var debts = await LoadDebtsAsync();
            return debts.Where(d => d.UserId == userId && d.Debtamount > 0).ToList();
        }

        // Pay a debt
        public async Task PayDebtAsync(int debtId, float paymentAmount)
        {
            var debts = await LoadDebtsAsync();
            var debtToPay = debts.FirstOrDefault(d => d.DebtId == debtId);

            if (debtToPay != null)
            {
                // Check if the payment amount exceeds the debt amount
                if (paymentAmount > debtToPay.Debtamount)
                {
                    throw new InvalidOperationException("Payment amount exceeds the debt amount.");
                }

                // Get the user's current credit balance from BalanceService
                float userCreditBalance = balanceService.GetCreditBalance(debtToPay.UserId);

                // Check if the user has enough credit balance
                if (paymentAmount > userCreditBalance)
                {
                    throw new InvalidOperationException("Insufficient Cre balance to pay the debt. Debts cant be cleared with debts");
                }

                // Proceed with the payment: Deduct from user's balance and credit balance
                balanceService.DeductFromBalance(debtToPay.UserId, paymentAmount); // Deduct from user's balance
                balanceService.DeductCredit(debtToPay.UserId, paymentAmount); // Deduct from user's credit balance

                // Reduce the debt amount or remove it if fully paid
                debtToPay.Debtamount -= paymentAmount;

                if (debtToPay.Debtamount <= 0)
                {
                    debts.Remove(debtToPay); // Fully paid, remove the debt
                }

                await SaveDebtsAsync(debts); // Save the updated debts list

                // Create and add a transaction for the debt payment
                var transaction = new Transaction
                {
                    UserId = debtToPay.UserId,
                    transactiontitle = "Debt Payment",
                    transactionamount = paymentAmount,
                    transactiondate = DateTime.Now,
                    transactiontype = "Debt Payment", // Specify the type of transaction
                    transactiontags = debtToPay.Debttags, // Optional
                    transactionremarks = $"Paid {paymentAmount:C} towards debt" // Optional
                };

                // Add the transaction using the ITransactionService
                transactionService.AddTransaction(transaction); // Assuming you have the AddTransaction method defined in ITransactionService
            }
            else
            {
                Console.WriteLine($"Debt with ID {debtId} not found.");
            }
        }


        // Load debts from the JSON file
        private async Task<List<Debt>> LoadDebtsAsync()
        {
            if (!File.Exists(debtsFilePath))
            {
                var emptyList = new List<Debt>();
                await SaveDebtsAsync(emptyList);
                return emptyList;
            }

            var json = await File.ReadAllTextAsync(debtsFilePath);
            return JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();
        }

        // Save debts to the JSON file
        private async Task SaveDebtsAsync(List<Debt> debts)
        {
            var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(debtsFilePath, json);
        }
    }
}