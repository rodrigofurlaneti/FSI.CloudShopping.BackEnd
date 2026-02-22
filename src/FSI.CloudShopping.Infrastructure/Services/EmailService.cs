using FSI.CloudShopping.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FSI.CloudShopping.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private static readonly HttpClient _httpClient = new HttpClient();
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendResetPasswordEmailAsync(string email, string newPassword)
        {
            var settings = _config.GetSection("EmailSettings");
            var request = new HttpRequestMessage(HttpMethod.Post, settings["PrimaryDomain"]);
            request.Headers.Add("api-key", settings["UsernamePassword"]);
            var mailBody = new
            {
                sender = new { name = "FSI CloudShopping", email = settings["FromEmail"] },
                to = new[] { new { email = email } },
                subject = "Sua Nova Senha - FSI CloudShopping",
                htmlContent = $@"
                    <html>
                        <body>
                            <h2>Olá!</h2>
                            <p>Sua senha foi resetada com sucesso.</p>
                            <p>Sua nova senha temporária é: <strong>{newPassword}</strong></p>
                            <br>
                            <p><i>Recomendamos trocar esta senha no seu próximo acesso.</i></p>
                        </body>
                    </html>"
            };
            request.Content = new StringContent(
                JsonSerializer.Serialize(mailBody),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                throw new Exception($"Falha na API Brevo: {errorDetail}");
            }
        }
    }
}