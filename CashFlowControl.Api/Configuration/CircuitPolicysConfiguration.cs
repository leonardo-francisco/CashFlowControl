using Polly.Extensions.Http;
using Polly;

namespace CashFlowControl.Api.Configuration
{
    public static class CircuitPolicysConfiguration
    {
        public static IServiceCollection AddPoliticasDeResiliencia(this IServiceCollection services)
        {
            // Política de Retry
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            // Política de Circuit Breaker
            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

            // Adiciona as políticas no container de DI
            services.AddSingleton(retryPolicy);
            services.AddSingleton(circuitBreakerPolicy);

            return services;
        }
    }
}
