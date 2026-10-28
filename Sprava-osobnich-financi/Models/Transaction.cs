using System.Globalization;

namespace Sprava_osobnich_financi.Models
{
    public class Transaction
    {
        private static readonly CultureInfo Czech = CultureInfo.GetCultureInfo("cs-CZ");

        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = "Ostatní";

        public decimal Amount { get; set; }

        public bool IsIncome { get; set; }

        public bool IsRecurring { get; set; }

        public int Priority { get; set; } = 3;

        public DateTime Date { get; set; } = DateTime.Now;

        public string AmountText => $"{(IsIncome ? "+" : "−")}{Amount.ToString("N0", Czech)} Kč";

        public string Subtitle => $"{Category} · {Date.ToString("d. M.", Czech)}";

        public string IconSource => Category switch
        {
            "Výplata" => "ic_salary.png",
            "Potraviny" => "ic_cart.png",
            "Restaurace" => "ic_food.png",
            "Doprava" => "ic_car.png",
            "Bydlení" => "ic_home.png",
            "Zábava" => "ic_movie.png",
            "Energie" => "ic_bolt.png",
            _ => "ic_wallet.png"
        };

        public Color CategoryColor => Color.FromArgb(Category switch
        {
            "Výplata" => "#1E8E3E",
            "Potraviny" => "#E8710A",
            "Restaurace" => "#D93F6A",
            "Doprava" => "#1A73E8",
            "Bydlení" => "#7B5EA7",
            "Zábava" => "#C2185B",
            "Energie" => "#B06000",
            _ => "#5F6368"
        });
    }
}
