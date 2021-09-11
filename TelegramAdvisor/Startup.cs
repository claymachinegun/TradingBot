using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Runtime.Caching;
using TradingBot.Core;
using TradingBot.Binance;
using TradingBot.TechIndicators;
using TradingBot.TechIndexes;
using TelegramAdvisor.Services;
using Telegram.Bot;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
namespace TelegramAdvisor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ITradingApiProvider, BinanceApi>(factory => {
                BinanceApi api = new BinanceApi(
                    Configuration.GetSection("Binance").GetValue<string>("ApiKey"),
                    Configuration.GetSection("Binance").GetValue<string>("Secret"),
                    "https://api.binance.com","1d"
                );
                return api;
            });
            services.AddScoped<Advisor>(factory => {
                Advisor advisor = new Advisor();
                advisor.Threshold = 0.7;
                var rsi = new RelativeStrengthIndex(14);
                advisor.AddIndex(rsi);
                advisor.AddIndicator(new MACross(6,12,12),0.25);
                advisor.AddIndicator(new RSIAndPriceGain(rsi), 0.25);
                advisor.AddIndicator(new RSIOverSold(rsi), 0.25);
                advisor.AddIndicator(new MACD(12,26,9), 0.25);
                return advisor;
            });
            services.AddScoped<AdvisorService>();
            services.AddHostedService<TelegramService>();
            
            services.AddHttpClient("tgwebhook")
                    .AddTypedClient<ITelegramBotClient>(httpClient
                        => new TelegramBotClient(Configuration.GetSection("BotConfiguration").GetValue<string>("BotToken"), httpClient));

            services.AddScoped<TelegramReplyService>();
            services.AddControllers()
                    .AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TelegramAdvisor", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TelegramAdvisor v1"));
            }

            
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

                string token = Configuration.GetSection("BotConfiguration").GetValue<string>("BotToken");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("telegram", "bot/"+token, new {controller = "Hook", action = "Post"});
                endpoints.MapControllers();
                
            });
        }
    }
}
