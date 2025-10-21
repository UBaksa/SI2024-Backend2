using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using carGooBackend.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace carGooBackend.Services
{
    public class PreduzeceCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1); // pokreće se svakih 1h

        public PreduzeceCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanUpPreduzecaAsync();
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task CleanUpPreduzecaAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CarGooDataContext>();

                var preduzecaBezKorisnika = await dbContext.Preduzeca
                    .Include(p => p.Korisnici)
                    .Where(p => !p.Korisnici.Any())
                    .ToListAsync();

                if (preduzecaBezKorisnika.Any())
                {
                    dbContext.Preduzeca.RemoveRange(preduzecaBezKorisnika);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
