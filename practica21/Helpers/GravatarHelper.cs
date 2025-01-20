using System.Security.Cryptography;
using System.Text;

namespace practica21.Helpers
{
    public static class GravatarHelper
    {
        //Diferentes estilos de avatares
        private static readonly string[] DefaultOptions = { "identicon", "monsterid", "wavatar", "retro", "robohash" };

        public static string GetGravatarUrl(string email)
        {
            email = email.Trim().ToLower(); // Normalizar el correo
            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
            var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            /* Seleccionar aleatoriamente una opción de avatar
            var random = new Random();
            string randomOption = DefaultOptions[random.Next(DefaultOptions.Length)];*/

            // Generar URL para enviar solicitud GET 
            return $"https://www.gravatar.com/avatar/{hash}?d={"robohash"}";
        }
    }
}
