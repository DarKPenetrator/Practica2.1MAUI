using System.Windows.Input;
using practica21.ViewModels;

namespace practica21
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainViewModel mainViewModel)
            {
                await mainViewModel.CargarUsuarios();
            }
        }

    }

}
