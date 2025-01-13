using Microsoft.Extensions.Logging;
using MoneyTrack.Services;
using MudBlazor.Services;

namespace MoneyTrack
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Registering User Services
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<BalanceService>();
            builder.Services.AddSingleton<TagService>();
            builder.Services.AddSingleton<ITransactionService, TransactionService>();
            builder.Services.AddSingleton<AuthenticationStateService>();

            // Registering Debt Services
            builder.Services.AddSingleton<IDebtService, DebtService>();
            builder.Services.AddSingleton<ICreditService, CreditService>();
            builder.Services.AddSingleton<IExpenseService, ExpenseService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
