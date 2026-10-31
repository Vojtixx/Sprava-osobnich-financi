using Sprava_osobnich_financi.Pages;

namespace Sprava_osobnich_financi
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrace tras pro stránky, které nejsou v TabBaru.
            // Otevírají se relativně: await Shell.Current.GoToAsync(nameof(AddTransactionPage));
            Routing.RegisterRoute(nameof(AddTransactionPage), typeof(AddTransactionPage));
        }
    }
}
