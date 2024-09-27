using Polly;
using Polly.Retry;

namespace Gestao.Pedidos.Pagamentos
{
    public class PagamentoService
    {
        private readonly Random _random = new Random();

        public Task<bool> PostApiPagamento(Pagamento pagamento)
        {
            bool resultado = _random.Next(0, 2) == 1; // Gera aleatório true ou false
            return Task.FromResult(resultado);
        }

        public async Task<bool> ProcessarPagamento(Pagamento pagamento)
        {
            AsyncRetryPolicy<bool> retryPolicy = Policy
            .HandleResult<bool>(resultado => !resultado)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                (resultado, tempoEspera, tentativa, contexto) =>
                {
                    Console.WriteLine($"Tentativa {tentativa}: pagamento falhou. Tentando novamente em {tempoEspera.TotalSeconds} segundos.");
                });

            bool sucesso = await retryPolicy.ExecuteAsync(() => PostApiPagamento(pagamento));

            if (sucesso)
            {
                Console.WriteLine("Pagamento processado com sucesso.");
            }
            else
            {
                Console.WriteLine("Falha no processamento do pagamento após múltiplas tentativas.");
            }

            return sucesso;
        }
    }
}
