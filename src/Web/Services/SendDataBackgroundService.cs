using Domain.DomainServices;
using Domain.Entities;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Web.Services;

public class SendDataBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync(s => s.LoginName == "ydfd"))
        {
            var sender = scope.ServiceProvider.GetRequiredService<ISender>();

            await sender.Send(new Application.Features.Users.Cmds.CreateCmd("ydfd", "yd123456", "默认用户"));
            
            await db.SaveChangesAsync();
        }

    }
}
