using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MoneyTrack.Models;

namespace MoneyTrack.Services
{
    public class CreditService : ICreditService
    {
        private readonly string creditsFilePath = Path.Combine(AppContext.BaseDirectory, "Credits.json");
        private readonly BalanceService balanceService;
        private readonly ITransactionService transactionService;

        public CreditService(BalanceService balanceService, ITransactionService transactionService)
        {
            this.balanceService = balanceService;
            this.transactionService = transactionService;
        }

        public async Task AddCreditAsync(Credit credit)
        {
            // Load existing credits
            var credits = await LoadCreditsAsync();

            // Assign a new CreditId
            credit.CreditId = credits.Count > 0 ? credits.Max(c => c.CreditId) + 1 : 1;

            // Add the new credit to the list
            credits.Add(credit);
            await SaveCreditsAsync(credits);

            // Update user's balance
            balanceService.AddCreditToBalance(credit.UserId, credit.Creditamount);
            balanceService.AddCredit(credit.UserId, credit.Creditamount);

            // Create and add a corresponding transaction
            var transaction = new Transaction
            {
                UserId = credit.UserId,
                transactiontitle = credit.Credittitle,
                transactionamount = (float)credit.Creditamount,
                transactiondate = DateTime.Now,
                transactiontype = "Credit", // Specify the type of transaction
                transactiontags = credit.Credittags, // Optional
                transactionremarks = credit.Creditremarks // Optional
            };
            transactionService.AddTransaction(transaction);
        }

        public async Task<List<Credit>> GetCreditsByUserIdAsync(int userId)
        {
            var credits = await LoadCreditsAsync();
            return credits.Where(c => c.UserId == userId).ToList();
        }

        private async Task<List<Credit>> LoadCreditsAsync()
        {
            if (!File.Exists(creditsFilePath))
            {
                var emptyList = new List<Credit>();
                await SaveCreditsAsync(emptyList);
                return emptyList;
            }

            var json = await File.ReadAllTextAsync(creditsFilePath);
            return JsonSerializer.Deserialize<List<Credit>>(json) ?? new List<Credit>();
        }

        private async Task SaveCreditsAsync(List<Credit> credits)
        {
            var json = JsonSerializer.Serialize(credits, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(creditsFilePath, json);
        }
    }
}