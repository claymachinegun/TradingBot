using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TelegramAdvisor.Services{
    public class TelegramService : IHostedService
    {
        private IConfiguration _configuration;

        private IServiceProvider _services;
        private ILogger<TelegramService> _logger;
        public TelegramService(IConfiguration configuraiton, IServiceProvider services, ILogger<TelegramService> logger){
            _configuration = configuraiton.GetSection("BotConfiguration");
            _services = services;
            _logger = logger;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var client = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            var hookUrl = String.Format("{0}/bot/{1}",_configuration.GetValue<string>("Host"), _configuration.GetValue<string>("BotToken")); 
            _logger.LogInformation($"WebHook registered:{hookUrl}");
            await client
                .SetWebhookAsync(
                    url:hookUrl,
                    allowedUpdates: Array.Empty<UpdateType>(),
                    cancellationToken: cancellationToken
                );
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var client  = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            _logger.LogInformation($"WebHook removed");
            await client.DeleteWebhookAsync();
        }
    }
}