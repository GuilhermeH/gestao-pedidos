using Gestao.Pedidos.Recepcao.Eventos;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Gestao.Pedidos.Recepcao.Handlers
{
    public class EnviarEmailAlteracaoEstadoPedidoHandler(PedidoRepository pedidoRepository, IConfiguration configuration) : INotificationHandler<AvisarClienteAlteracaoEstadoPedidoEvent>
    {
        public async Task Handle(AvisarClienteAlteracaoEstadoPedidoEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(EnviarEmailAlteracaoEstadoPedidoHandler)} - {notification.IdPedido} - {notification.EstadoPedido}");
            var corpoDoEmail = $"Pedido {notification.IdPedido} está no status {notification.EstadoPedido}";
            var pedido = await pedidoRepository.ObterPedido(notification.IdPedido);

            await EnviarEmail(notification.EmailCliente, "Status Pedido", corpoDoEmail);
        }

        public async Task EnviarEmail(string destinatario, string assunto, string corpo)
        {
            var networkCredentials = configuration.GetSection("NetworkEmailCredential").Get<NetworkEmailCredential>();
           
            try
            {
                // Cria a mensagem de e-mail
                MailMessage mensagem = new MailMessage();
                mensagem.From = new MailAddress("gestaopedidos@teste.com");
                mensagem.To.Add(destinatario);
                mensagem.Subject = assunto;
                mensagem.Body = corpo;
                mensagem.IsBodyHtml = true; // Se o corpo for HTML, defina como true

                // Configura o cliente SMTP
                SmtpClient smtpClient = new SmtpClient("sandbox.smtp.mailtrap.io"); // Use o servidor SMTP correto
                smtpClient.Port = 587; // Verifique a porta correta para o seu provedor de email
                smtpClient.Credentials = new NetworkCredential(networkCredentials.Username, networkCredentials.Password);
                smtpClient.EnableSsl = true; // Use SSL se necessário pelo provedor

                // Envia o e-mail
                await smtpClient.SendMailAsync(mensagem);
                Console.WriteLine("Email enviado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }

        public record NetworkEmailCredential(string Username, string Password);
    }
}
