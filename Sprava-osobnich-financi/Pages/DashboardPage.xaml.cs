using Sprava_osobnich_financi.Services;

namespace Sprava_osobnich_financi.Pages
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateOverview();
        }

        private async void OnAddTransactionClicked(object? sender, EventArgs e) =>
            await NavigationService.GoToAsync(nameof(AddTransactionPage));

        private async void OnAddIncomeClicked(object? sender, EventArgs e) =>
            await NavigationService.GoToAsync($"{nameof(AddTransactionPage)}?type=income");

        private async void OnAddExpenseClicked(object? sender, EventArgs e) =>
            await NavigationService.GoToAsync($"{nameof(AddTransactionPage)}?type=expense");

        private async void OnOpenRatesClicked(object? sender, EventArgs e) =>
            await NavigationService.GoToAsync("///ExchangeRatesTab");

        private async void OnProfileTapped(object? sender, TappedEventArgs e) =>
            await NavigationService.GoToAsync("///SettingsTab");

        private void UpdateOverview()
        {
        }

        private void OnFilterAllClicked(object? sender, EventArgs e)
        {
        }

        private void OnFilterIncomeClicked(object? sender, EventArgs e)
        {
        }

        private void OnFilterExpenseClicked(object? sender, EventArgs e)
        {
        }

        private void OnRecurringOnlyCheckedChanged(object? sender, CheckedChangedEventArgs e)
        {
        }

        private void OnHideAmountsToggled(object? sender, ToggledEventArgs e)
        {
        }
    }
}
