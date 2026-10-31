using Sprava_osobnich_financi.Services;
using System.Globalization;
using Sprava_osobnich_financi.Models;

namespace Sprava_osobnich_financi.Pages
{
    public partial class DashboardPage : ContentPage
    {
        private static readonly CultureInfo Czech = CultureInfo.GetCultureInfo("cs-CZ");

        private const int PocatecniZustatek = 40000;
        private const int Limit = 25000;

        private int zustatek, prijmy, vydaje, usetreno, pravidelnePlatby;
        private string filtr = "vse"; // "vse", "prijmy" nebo "vydaje"
        private bool pravidelne = false;
        private bool schovat = false;

        public DashboardPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateOverview();
            ApplyFilter();
        }

        // Relativní URI – formulář se otevře navrch záložky Přehled (trasa je registrovaná v AppShell.xaml.cs)
        private async void OnAddTransactionClicked(object? sender, EventArgs e) =>
            await NavigationService.GoToAsync(nameof(AddTransactionPage));

        // Relativní URI s parametrem – formulář si podle "type" předvybere Příjem / Výdaj
        private async void OnAddIncomeClicked(object? sender, EventArgs e) =>
            await NavigationService.GoToAsync($"{nameof(AddTransactionPage)}?type=income");

        private async void OnAddExpenseClicked(object? sender, EventArgs e) =>
            await NavigationService.GoToAsync($"{nameof(AddTransactionPage)}?type=expense");

        // Absolutní URI – přepne záložku v TabBaru
        private async void OnOpenRatesClicked(object? sender, EventArgs e) =>
            await NavigationService.GoToAsync("///ExchangeRatesTab");

        private async void OnProfileTapped(object? sender, TappedEventArgs e) =>
            await NavigationService.GoToAsync("///SettingsTab");

        private void NacistPenize()
        {
            int zbyva = Limit - vydaje;
            if (schovat)
            {
                BalanceLabel.Text = "***** Kč";
                IncomeLabel.Text = "***** Kč";
                ExpenseLabel.Text = "***** Kč";
                SavingsLabel.Text = "***** Kč";
                RecurringLabel.Text = "***** Kč";

                if (zbyva >= 0)
                    BudgetRemainingLabel.Text = "zbývá ***** Kč";
                else
                    BudgetRemainingLabel.Text = "rozpočet překročen o ***** Kč";

                BudgetValueLabel.Text = "***** Kč";
            }
            else
            {
                BalanceLabel.Text = $"{zustatek.ToString("N0", Czech)} Kč";
                IncomeLabel.Text = $"{prijmy.ToString("N0", Czech)} Kč";
                ExpenseLabel.Text = $"{vydaje.ToString("N0", Czech)} Kč";
                SavingsLabel.Text = $"{usetreno.ToString("N0", Czech)} Kč";
                RecurringLabel.Text = $"{pravidelnePlatby.ToString("N0", Czech)} Kč";

                if (zbyva >= 0)
                    BudgetRemainingLabel.Text = $"zbývá {zbyva.ToString("N0", Czech)} Kč";
                else
                    BudgetRemainingLabel.Text = $"rozpočet překročen o {(zbyva * -1).ToString("N0", Czech)} Kč";

                BudgetValueLabel.Text = $"{vydaje.ToString("N0", Czech)} z {Limit.ToString("N0", Czech)} Kč";
            }
        }

        private void UpdateOverview()
        {
            prijmy = 0;
            vydaje = 0;
            pravidelnePlatby = 0;

            foreach (Transaction transaction in TransactionStore.Transactions)
            {
                if (transaction.IsIncome)
                    prijmy += transaction.Amount;
                else
                {
                    vydaje += transaction.Amount;
                    if (transaction.IsRecurring)
                        pravidelnePlatby += transaction.Amount;
                }
            }
            usetreno = prijmy - vydaje;
            zustatek = PocatecniZustatek + usetreno;

            double progress = (double)vydaje / Limit;
            int procenta = (int)(progress * 100);
            if (progress > 1)
                progress = 1;

            BudgetProgressBar.Progress = progress;
            BudgetPercentLabel.Text = $"{procenta} %";

            NacistPenize();
        }

        private void ApplyFilter()
        {
            if (filtr == "vse" && !pravidelne)
            {
                BindableLayout.SetItemsSource(TransactionsList, TransactionStore.Transactions);
                TransactionCountLabel.Text = $"{TransactionStore.Transactions.Count} transakcí";
                return;
            }

            List<Transaction> transakce = new List<Transaction>();
            foreach (Transaction transaction in TransactionStore.Transactions)
            {
                bool sediTyp;
                if (filtr == "vse")
                    sediTyp = true;
                else if (filtr == "prijmy")
                    sediTyp = transaction.IsIncome;
                else
                    sediTyp = !transaction.IsIncome;

                bool sediPravidelnost = !pravidelne || transaction.IsRecurring;

                if (sediTyp && sediPravidelnost)
                    transakce.Add(transaction);
            }
            BindableLayout.SetItemsSource(TransactionsList, transakce);
            TransactionCountLabel.Text = $"{transakce.Count} transakcí";
        }

        private void OznacButton(Button vybrany)
        {
            FilterAllButton.Style = (Style)Application.Current!.Resources["ChipButton"];
            FilterIncomeButton.Style = (Style)Application.Current!.Resources["ChipButton"];
            FilterExpenseButton.Style = (Style)Application.Current!.Resources["ChipButton"];

            vybrany.Style = (Style)Application.Current!.Resources["ChipButtonSelected"];
        }

        private void OnFilterAllClicked(object? sender, EventArgs e)
        {
            filtr = "vse";
            OznacButton(FilterAllButton);
            ApplyFilter();
        }

        private void OnFilterIncomeClicked(object? sender, EventArgs e)
        {
            filtr = "prijmy";
            OznacButton(FilterIncomeButton);
            ApplyFilter();
        }

        private void OnFilterExpenseClicked(object? sender, EventArgs e)
        {
            filtr = "vydaje";
            OznacButton(FilterExpenseButton);
            ApplyFilter();
        }

        private void OnRecurringOnlyCheckedChanged(object? sender, CheckedChangedEventArgs e)
        {
            pravidelne = RecurringOnlyCheckBox.IsChecked;
            ApplyFilter();
        }

        private void OnHideAmountsToggled(object? sender, ToggledEventArgs e)
        {
            schovat = HideAmountsSwitch.IsToggled;
            NacistPenize();

            Transaction.SkrytCastky = schovat;
            BindableLayout.SetItemsSource(TransactionsList, null);
            ApplyFilter();
        }
    }
}
