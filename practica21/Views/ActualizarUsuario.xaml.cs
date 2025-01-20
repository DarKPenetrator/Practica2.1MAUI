using practica21.ViewModels;

namespace practica21.Views;

[QueryProperty(nameof(UserId), "id")]
[QueryProperty(nameof(UserName), "nombre")]
[QueryProperty(nameof(UserEmail), "correo")]
public partial class ActualizarUsuario : ContentPage
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }

    public ActualizarUsuario()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Esperar un breve momento para que se configuren las propiedades
        await Task.Delay(100);

        // Verificar si las propiedades se han configurado
        if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(UserEmail))
        {
            await DisplayAlert("Error", "No se pudo cargar el usuario. Intenta nuevamente.", "OK");
            return;
        }
        // Asignar los valores a los campos de entrada
        NombreEntry.Text = UserName;
        CorreoEntry.Text = UserEmail;
    }

    private async void OnActualizarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NombreEntry.Text) || string.IsNullOrWhiteSpace(CorreoEntry.Text))
        {
            await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
            return;
        }

        // Verificar y convertir el ID antes de llamar al método
        if (int.TryParse(UserId, out int id))
        {
            if (BindingContext is MainViewModel mainViewModel)
            {
                // Llamar al método ActualizarUsuario en el ViewModel
                await mainViewModel.ActualizarUsuario(UserId, NombreEntry.Text, CorreoEntry.Text);

                // Regresar a la página principal después de actualizar
                await Shell.Current.GoToAsync("///MainPage");
            }
        }
        else
        {
            await DisplayAlert("Error", "ID de usuario no válido.", "OK");
        }
    }


    private async void OnCancelarClicked(object sender, EventArgs e)
    {

        NombreEntry.Text = string.Empty;
        CorreoEntry.Text = string.Empty;
        await Shell.Current.GoToAsync("///MainPage");
    }
}
