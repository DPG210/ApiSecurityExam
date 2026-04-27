using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MvcApiSecurityExam.Models;

namespace MvcApiSecurityExam.Services
{
    public class ApiClientService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiClientService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            this.client = client;
            this.httpContextAccessor = httpContextAccessor;
        }

        private void ApplyAuthorization(HttpRequestMessage request)
        {
            string? token = this.httpContextAccessor.HttpContext?.Session.GetString("token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<T?> ReadAsync<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>(this.jsonOptions);
        }

        private async Task<List<T>> ReadListAsync<T>(HttpResponseMessage response)
        {
            return await ReadAsync<List<T>>(response) ?? new List<T>();
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, object? body = null, bool authorized = false)
        {
            using HttpRequestMessage request = new(method, url);
            if (authorized)
            {
                this.ApplyAuthorization(request);
            }

            if (body is not null)
            {
                request.Content = JsonContent.Create(body, options: this.jsonOptions);
            }

            return await this.client.SendAsync(request);
        }

        public async Task<List<Libros>> GetLibrosAsync() => await ReadListAsync<Libros>(await SendAsync(HttpMethod.Get, "api/Libros/Libros"));

        public async Task<Libros?> GetLibroAsync(int id) => await ReadAsync<Libros>(await SendAsync(HttpMethod.Get, $"api/Libros/Libro/{id}"));

        public async Task<List<Libros>> GetLibrosGeneroAsync(int idGenero) => await ReadListAsync<Libros>(await SendAsync(HttpMethod.Get, $"api/Libros/LibrosGenero/{idGenero}"));

        public async Task<bool> CreateLibroAsync(Libros model) => (await SendAsync(HttpMethod.Post, "api/Libros/CreateLibro", model)).IsSuccessStatusCode;

        public async Task<bool> UpdateLibroAsync(int id, Libros model) => (await SendAsync(HttpMethod.Put, $"api/Libros/UpdateLibro/{id}", model)).IsSuccessStatusCode;

        public async Task<bool> DeleteLibroAsync(int id) => (await SendAsync(HttpMethod.Delete, $"api/Libros/DeleteLibro/{id}")).IsSuccessStatusCode;

        public async Task<List<Generos>> GetGenerosAsync() => await ReadListAsync<Generos>(await SendAsync(HttpMethod.Get, "api/Libros/Generos"));

        public async Task<Generos?> GetGeneroAsync(int idGenero) => await ReadAsync<Generos>(await SendAsync(HttpMethod.Get, $"api/Libros/Genero/{idGenero}"));

        public async Task<List<Usuarios>> GetUsuariosAsync() => await ReadListAsync<Usuarios>(await SendAsync(HttpMethod.Get, "api/Libros/Usuarios"));

        public async Task<bool> CreateUsuarioAsync(Usuarios model) => (await SendAsync(HttpMethod.Post, "api/Libros/CreateUsuario", model)).IsSuccessStatusCode;

        public async Task<string?> LoginAsync(LoginModel model)
        {
            HttpResponseMessage response = await SendAsync(HttpMethod.Post, "api/Auth", model);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            ApiTokenResponse? token = await response.Content.ReadFromJsonAsync<ApiTokenResponse>(this.jsonOptions);
            return token?.Response;
        }

        public async Task<bool> CreatePedidoAsync(int idUsuario, List<int> idLibros)
        {
            string query = $"idUsuario={idUsuario}";
            foreach (int idLibro in idLibros)
            {
                query += $"&idLibros={idLibro}";
            }

            return (await SendAsync(HttpMethod.Post, $"api/Libros/CreatePedido?{query}", authorized: true)).IsSuccessStatusCode;
        }

        public async Task<UserModel?> GetPerfilUsuarioAsync() => await ReadAsync<UserModel>(await SendAsync(HttpMethod.Get, "api/Libros/PerfilUsuario", authorized: true));

        public async Task<UserModel?> GetPerfilUsuarioBlobAsync() => await ReadAsync<UserModel>(await SendAsync(HttpMethod.Get, "api/Libros/PerfilBlob", authorized: true));

        public async Task<List<Libros>> GetLibrosBlobAsync() => await ReadListAsync<Libros>(await SendAsync(HttpMethod.Get, "api/Libros/LibrosBlob", authorized: true));
    }
}
