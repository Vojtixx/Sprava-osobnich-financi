using Sprava_osobnich_financi.Services;

namespace Sprava_osobnich_financi.Pages
{
    public partial class AddTransactionPage : ContentPage, IQueryAttributable
    {
        public AddTransactionPage()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("type", out var value) && value is string type)
            {
                IncomeRadio.IsChecked = type == "income";
                ExpenseRadio.IsChecked = type != "income";
            }
        }

        private async void OnCancelClicked(object? sender, EventArgs e) =>
            await NavigationService.GoBackAsync();

        private void OnNameTextChanged(object? sender, TextChangedEventArgs e)
        {
        }

        private void OnAmountTextChanged(object? sender, TextChangedEventArgs e)
        {
        }

        private void OnCategoryChanged(object? sender, EventArgs e)
        {
        }

        private void OnTypeCheckedChanged(object? sender, CheckedChangedEventArgs e)
        {
        }

        private void OnPrioritySliderValueChanged(object? sender, ValueChangedEventArgs e)
        {
        }

        private void OnRecurringToggled(object? sender, ToggledEventArgs e)
        {
        }

        private void OnSimulateErrorCheckedChanged(object? sender, CheckedChangedEventArgs e)
        {
        }

        private void OnSaveClicked(object? sender, EventArgs e)
        {
        }
    }
}
