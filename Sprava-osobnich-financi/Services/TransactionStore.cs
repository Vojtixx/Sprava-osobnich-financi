using System.Collections.ObjectModel;
using Sprava_osobnich_financi.Models;

namespace Sprava_osobnich_financi.Services
{
    // Seznam transakcí. Dashboard ho zobrazuje (BindableLayout) a formulář do něj může přidávat.
    public static class TransactionStore
    {
        public static ObservableCollection<Transaction> Transactions { get; } = new()
        {
            new Transaction { Title = "Výplata", Category = "Výplata", Amount = 38500, IsIncome = true, IsRecurring = true, Priority = 5, Date = DateTime.Today },
            new Transaction { Title = "Nákup potravin", Category = "Potraviny", Amount = 1284, Priority = 4, Date = DateTime.Today.AddDays(-1) },
            new Transaction { Title = "Nájem bytu", Category = "Bydlení", Amount = 14500, IsRecurring = true, Priority = 5, Date = DateTime.Today.AddDays(-2) },
            new Transaction { Title = "Oběd s kolegy", Category = "Restaurace", Amount = 465, Priority = 2, Date = DateTime.Today.AddDays(-3) },
            new Transaction { Title = "Tankování", Category = "Doprava", Amount = 1650, Priority = 3, Date = DateTime.Today.AddDays(-4) },
            new Transaction { Title = "Předplatné streamingu", Category = "Zábava", Amount = 259, IsRecurring = true, Priority = 1, Date = DateTime.Today.AddDays(-5) },
            new Transaction { Title = "Záloha na elektřinu", Category = "Energie", Amount = 1900, IsRecurring = true, Priority = 5, Date = DateTime.Today.AddDays(-7) },
        };
    }
}
