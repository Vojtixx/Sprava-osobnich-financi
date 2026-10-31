using Sprava_osobnich_financi.Services;

namespace Sprava_osobnich_financi.Pages
{
    public partial class AddTransactionPage : ContentPage, IQueryAttributable
    {
        bool jeNazev, jeCastka;

        public AddTransactionPage()
        {
            InitializeComponent();
        }

        // Přijme parametry z URI, např. GoToAsync("AddTransactionPage?type=income"),
        // a podle parametru "type" předvybere Příjem / Výdaj.
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
            jeNazev = false;
            string nazev = NameEntry.Text;
            if (nazev.Trim().Length < 2)
            {
                NameErrorLabel.IsVisible = true;
                jeNazev = false;
            }
            else
            {
                NameErrorLabel.IsVisible = false;
                jeNazev = true;
            }

            NameCounterLabel.Text = $"{nazev.Length} / 40";
            NameField.Style = (Style)Application.Current!.Resources[jeNazev ? "InputField" : "InputFieldError"];
        }

        private void OnAmountTextChanged(object? sender, TextChangedEventArgs e)
        {
            jeCastka = false;
            if (int.TryParse(AmountEntry.Text, out int castka) && castka > 0)
            {
                AmountErrorLabel.IsVisible = false;
                jeCastka = true;
            }
            else
            {
                AmountErrorLabel.IsVisible = true;
                jeCastka = false;
            }

            AmountField.Style = (Style)Application.Current!.Resources[jeCastka ? "InputField" : "InputFieldError"];
        }

        private void OnPrioritySliderValueChanged(object? sender, ValueChangedEventArgs e)
        {
            if (PrioritySlider == null || PriorityValueLabel == null || PriorityDescriptionLabel == null)
                return;

            int priorita = (int)Math.Round(e.NewValue);

            if (PrioritySlider.Value != priorita)
            {
                PrioritySlider.Value = priorita;
                return;
            }

            PriorityValueLabel.Text = $"{priorita} / 5";
            switch(priorita)
            {
                case 1:
                    PriorityDescriptionLabel.Text = "Zbytný výdaj";
                    break;
                case 2:
                    PriorityDescriptionLabel.Text = "Spíš zbytný";
                    break;
                case 3:
                    PriorityDescriptionLabel.Text = "Běžný výdaj";
                    break;
                case 4:
                    PriorityDescriptionLabel.Text = "Důležitý";
                    break;
                case 5:
                    PriorityDescriptionLabel.Text = "Nezbytný";
                    break;
            }
        }

        private void OnRecurringToggled(object? sender, ToggledEventArgs e)
        {
            if (RecurringSwitch.IsToggled)
                RecurringHintLabel.Text = "Opakuje se každý měsíc";
            else
                RecurringHintLabel.Text = "Jednorázová platba";
        }

        private async void OnSaveClicked(object? sender, EventArgs e)
        {
            SaveButton.IsEnabled = false;

            if (!jeNazev || !jeCastka)
            {
                await DisplayAlert("Chyba", "Záznam nelze uložit. Zkontrolujte název a částku.", "OK");
                SaveButton.IsEnabled = true;
                return;
            }

            SavingOverlay.IsVisible = true;
            SavingIndicator.IsRunning = true;
            try
            {
                await Task.Delay(2000);

                if (SimulateErrorCheckBox.IsChecked)
                    throw new Exception("Nepodařilo se spojit se serverem.");

                TransactionStore.Transactions.Insert(0, new Models.Transaction
                {
                    Title = NameEntry.Text.Trim(),
                    Category = CategoryPicker.SelectedItem?.ToString() ?? "Ostatní",
                    Amount = int.TryParse(AmountEntry.Text, out int castka) ? castka : 0,
                    IsIncome = IncomeRadio.IsChecked,
                    IsRecurring = RecurringSwitch.IsToggled,
                    Priority = (int)Math.Round(PrioritySlider.Value),
                    Date = DateTime.Now
                });

                await DisplayAlert("Úspěch", "Záznam byl uložen.", "OK");
                await NavigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Chyba", $"Došlo k chybě při ukládání záznamu: {ex.Message}", "OK");
            }
            finally
            {
                SavingIndicator.IsRunning = false;
                SavingOverlay.IsVisible = false;
                SaveButton.IsEnabled = true;
            }
        }
    }
}
