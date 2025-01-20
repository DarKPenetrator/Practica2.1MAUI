using practica21.ViewModels;

namespace practica21.Views;

public partial class FormularioUsuario : ContentPage
{
	public FormularioUsuario()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }


    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        // Navegar de forma absoluta a la p�gina principal
        await Shell.Current.GoToAsync("///MainPage");
        // Limpiar los campos
        NombreEntry.Text = string.Empty;
        CorreoEntry.Text = string.Empty;

    }


    private async void OnAgregarClicked(object sender, EventArgs e)
    {
        // Validar los campos
        if (string.IsNullOrWhiteSpace(NombreEntry.Text) || string.IsNullOrWhiteSpace(CorreoEntry.Text))
        {
            await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
            return;
        }

        // Obtener el ViewModel desde el BindingContext
        if (BindingContext is MainViewModel mainViewModel)
        {
            // Agregar el usuario usando el m�todo p�blico
            await mainViewModel.A�adirUsuario(NombreEntry.Text, CorreoEntry.Text);

            // Limpiar los campos
            NombreEntry.Text = string.Empty;
            CorreoEntry.Text = string.Empty;


            // Navegar de vuelta a la p�gina principal
            await Shell.Current.GoToAsync("///MainPage");
        }
        else
        {
            await DisplayAlert("Error", "No se pudo acceder al ViewModel.", "OK");
        }
    }


  


}