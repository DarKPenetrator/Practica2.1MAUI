using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;
using practica21.Helpers;
using practica21.Models;

namespace practica21.ViewModels;

public class MainViewModel
{
    private readonly HttpClient _httpClient;

    public ObservableCollection<Usuario> Usuarios { get; set; }

    public ICommand ComandoCargarUsuarios { get; }
    public ICommand ComandoAñadir { get; }
    public ICommand ComandoActualizar { get; }
    public ICommand ComandoEliminar { get; }
    public ICommand ComandoAbrirFormulario { get; }
    public ICommand ComandoActualizarUsuario { get; }
    public ICommand ComandoEliminarUsuario { get; }
    public ICommand ComandoSeleccionarUsuario { get; }




    private Usuario _usuarioSeleccionado;

    public MainViewModel()
    {
        // Inicializar HttpClient new Uri("https://677d3f4b4496848554c9b73c.mockapi.io/") };
        _httpClient = new HttpClient { BaseAddress = new Uri("https://677d3f4b4496848554c9b73c.mockapi.io/") };

        Usuarios = new ObservableCollection<Usuario>();

        // Comandos
        ComandoCargarUsuarios = new Command(async () => await CargarUsuarios());
        ComandoAñadir = new Command(async () => await MostrarFormularioAñadirUsuario());
        //ComandoActualizar = new Command(async () => await ActualizarUsuario());
        //ComandoEliminar = new Command(async () => await EliminarUsuario());
        ComandoAbrirFormulario = new Command(async () => await Shell.Current.GoToAsync("///FormularioUsuario"));
        ComandoActualizarUsuario = new Command(async () => await Shell.Current.GoToAsync("///ActualizarUsuario"));
        ComandoEliminarUsuario = new Command<Usuario>(async (usuario) => await EliminarUsuario(usuario));
        ComandoSeleccionarUsuario = new Command<Usuario>((usuario) => UsuarioSeleccionado = usuario);


        _ = CargarUsuarios();
    }

    public Usuario UsuarioSeleccionado
    {
        get => _usuarioSeleccionado;
        set
        {
            _usuarioSeleccionado = value;
            if (_usuarioSeleccionado != null)
            {
                AbrirFormularioActualizar();
            }
        }
    }

    private async void AbrirFormularioActualizar()
    {
        if (UsuarioSeleccionado != null)
        {
            await Shell.Current.GoToAsync($"///ActualizarUsuario?id={UsuarioSeleccionado.Id}&nombre={UsuarioSeleccionado.Nombre}&correo={UsuarioSeleccionado.Correo}");
        }
        else
        {
            Console.WriteLine("No se seleccionó ningún usuario.");
        }
    }





    public async Task CargarUsuarios()
    {
        try
        {
            var usuarios = await _httpClient.GetFromJsonAsync<List<Usuario>>("usuarios");
            if (usuarios == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "No se pudo cargar la lista de usuarios. Intente más tarde.", "OK");
                return;
            }

            Usuarios.Clear();
            foreach (var usuario in usuarios)
            {
                Usuarios.Add(usuario);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error de Conexión", "No se pudo conectar al servidor. Verifica tu conexión a internet.", "OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error desconocido: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error", "Ocurrió un error inesperado. Intente más tarde.", "OK");
        }
    }



    public async Task AñadirUsuario(string nombre, string correo)
    {
        try
        {
            // Generar avatar aleatorio con Gravatar
            string avatarUrl = GravatarHelper.GetGravatarUrl(correo);

            var nuevoUsuario = new Usuario { Nombre = nombre, Correo = correo, AvatarUrl = avatarUrl };
            var respuesta = await _httpClient.PostAsJsonAsync("usuarios", nuevoUsuario);

            if (respuesta.IsSuccessStatusCode)
            {
                await CargarUsuarios(); // Recargar la lista después de añadir
                await App.Current.MainPage.DisplayAlert("Éxito", "Usuario añadido correctamente.", "OK");
            }
            else
            {
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert("Error", $"No se pudo agregar el usuario: {mensajeError}", "OK");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error de Conexión", "No se pudo conectar al servidor. Verifica tu conexión a internet.", "OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error desconocido: {ex.Message}");
            await App.Current.MainPage.DisplayAlert("Error", "Ocurrió un error inesperado. Intente más tarde.", "OK");
        }
    }





    private async Task MostrarFormularioAñadirUsuario()
    {
        string nombre = await App.Current.MainPage.DisplayPromptAsync("Nuevo Usuario", "Introduce el nombre:");
        if (string.IsNullOrWhiteSpace(nombre)) return;

        string correo = await App.Current.MainPage.DisplayPromptAsync("Nuevo Usuario", "Introduce el correo:");
        if (string.IsNullOrWhiteSpace(correo)) return;

        await AñadirUsuario(nombre, correo);
    }



    public async Task ActualizarUsuario(string id, string nombre, string correo)
    {
        try
        {
            // Buscar el usuario en la lista local para mantener el avatar
            var usuarioActual = Usuarios.FirstOrDefault(u => u.Id == id);

            // Si no se encuentra, generamos un nuevo avatar
            var avatarUrl = usuarioActual?.AvatarUrl ?? GravatarHelper.GetGravatarUrl(correo);

            // Crear el objeto actualizado con el avatar existente
            var usuarioActualizado = new Usuario { Id = id, Nombre = nombre, Correo = correo, AvatarUrl = avatarUrl };

            var respuesta = await _httpClient.PutAsJsonAsync($"usuarios/{id}", usuarioActualizado);

            if (respuesta.IsSuccessStatusCode)
            {
                await CargarUsuarios(); // Recargar la lista después de actualizar
            }
            else
            {
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error desconocido: {ex.Message}");
        }
    }







    private async Task EliminarUsuario(Usuario usuario)
    {
        if (usuario == null) return;

        try
        {
            var respuesta = await _httpClient.DeleteAsync($"usuarios/{usuario.Id}");
            if (respuesta.IsSuccessStatusCode)
            {
                Usuarios.Remove(usuario); // Eliminar localmente
            }
            else
            {
                var mensajeError = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al eliminar usuario: {mensajeError}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error desconocido al eliminar usuario: {ex.Message}");
        }
    }


}
