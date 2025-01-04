using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoneyTrack.Services
{
    public class BalanceService
    {
        private readonly string balanceFilePath = Path.Combine(AppContext.BaseDirectory, "user_balances.json");
        private Dictionary<int, float> userBalances = new();

        public BalanceService()
        {
            LoadBalances(); // Load balances when the service is initialized
        }

        public float GetBalance(int userId)
        {
            return userBalances.ContainsKey(userId) ? userBalances[userId] : 0;
        }

        public bool HasSufficientBalance(int userId, float amount)
        {
            return userBalances.ContainsKey(userId) && userBalances[userId] >= amount;
        }

        public void UpdateBalance(int userId, float amount)
        {
            if (userBalances.ContainsKey(userId))
            {
                userBalances[userId] += amount;
            }
            else
            {
                userBalances[userId] = amount;
            }

            SaveBalances(); // Save the updated balances to the file
        }

        public void AddCreditToBalance(int userId, float creditAmount)
        {
            UpdateBalance(userId, creditAmount); // Add credit to the user's balance
        }

        public void DeductFromBalance(int userId, float expenseAmount)
        {
            if (!HasSufficientBalance(userId, expenseAmount))
            {
                throw new InvalidOperationException("Insufficient balance to deduct the expense.");
            }
            UpdateBalance(userId, -expenseAmount); // Deduct expense from the user's balance
        }

        private void LoadBalances()
        {
            if (File.Exists(balanceFilePath))
            {
                try
                {
                    var json = File.ReadAllText(balanceFilePath);
                    userBalances = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, float>>(json) ?? new Dictionary<int, float>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading balances: {ex.Message}");
                    userBalances = new Dictionary<int, float>();
                }
            }
            else
            {
                userBalances = new Dictionary<int, float>();
                SaveBalances();
            }
        }

        private void SaveBalances()
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(userBalances, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(balanceFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving balances: {ex.Message}");
            }
        }
    }
}
