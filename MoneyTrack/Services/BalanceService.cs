namespace MoneyTrack.Services
{
    public class BalanceService
    {
        private readonly string balanceFilePath = Path.Combine(AppContext.BaseDirectory, "user_balances.json");
        private readonly string creditBalanceFilePath = Path.Combine(AppContext.BaseDirectory, "user_credit_balances.json");

        private Dictionary<int, float> userBalances = new();
        private Dictionary<int, float> userCreditBalances = new();  // New dictionary for credit balances

        public BalanceService()
        {
            LoadBalances(); // Load balances when the service is initialized
            LoadCreditBalances(); // Load credit balances
        }

        // Balance methods (existing)
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

        // Credit balance methods (new)
        public float GetCreditBalance(int userId)
        {
            return userCreditBalances.ContainsKey(userId) ? userCreditBalances[userId] : 0;
        }

        public void UpdateCreditBalance(int userId, float amount)
        {
            if (userCreditBalances.ContainsKey(userId))
            {
                userCreditBalances[userId] += amount;
            }
            else
            {
                userCreditBalances[userId] = amount;
            }

            SaveCreditBalances(); // Save the updated credit balances to the file
        }

        public void AddCredit(int userId, float creditAmount)
        {
            UpdateCreditBalance(userId, creditAmount); // Add credit to the user's credit balance
        }

        public void DeductCredit(int userId, float creditAmount)
        {
            if (!HasSufficientCredit(userId, creditAmount))
            {
                throw new InvalidOperationException("Insufficient credit to deduct.");
            }
            UpdateCreditBalance(userId, -creditAmount); // Deduct credit from the user's credit balance
        }

        public bool HasSufficientCredit(int userId, float amount)
        {
            return userCreditBalances.ContainsKey(userId) && userCreditBalances[userId] >= amount;
        }

        // Load user balances from the file
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

        // Save user balances to the file
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

        // Load user credit balances from the file
        private void LoadCreditBalances()
        {
            if (File.Exists(creditBalanceFilePath))
            {
                try
                {
                    var json = File.ReadAllText(creditBalanceFilePath);
                    userCreditBalances = System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, float>>(json) ?? new Dictionary<int, float>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading credit balances: {ex.Message}");
                    userCreditBalances = new Dictionary<int, float>();
                }
            }
            else
            {
                userCreditBalances = new Dictionary<int, float>();
                SaveCreditBalances();
            }
        }

        // Save user credit balances to the file
        private void SaveCreditBalances()
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(userCreditBalances, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(creditBalanceFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving credit balances: {ex.Message}");
            }
        }
    }
}
