using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using carGooBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace carGooBackend.Services
{
    public class OfferCleanupService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); 

        public OfferCleanupService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<CarGooDataContext>();

                    // Brise ponudu gde je datum utovara prosaooo
                    var expiredOffers = await dbContext.Ponude
                        .Where(p => p.Utovar.Date < DateTime.Now.Date)
                        .ToListAsync(stoppingToken);

                    if (expiredOffers.Any())
                    {
                        dbContext.Ponude.RemoveRange(expiredOffers);
                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
