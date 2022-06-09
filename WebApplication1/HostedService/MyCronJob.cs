using Microsoft.AspNetCore.SignalR;
using WebApplication1.HubConnection;

namespace WebApplication1.HostedService
{
    public class MyCronJob : CronJobService
    {
        IHubContext<MessageHub> _hubContext;
        Random _random;
        public MyCronJob(IScheduleConfig<MyCronJob> config, IHubContext<MessageHub> hubContext)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _hubContext = hubContext;
            _random = new Random();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            int number = _random.Next(3);
            string status=null;

            switch (number)
            {
                case 0:
                    status = "status 200";
                    break;
                case 1:
                    status = "status 404";
                    break;
                case 2:
                    status = "status 500";
                    break;
                case 3:
                    status = "status 500";
                    break;
            }

            await _hubContext.Clients.All.SendAsync("ReceiveMessageHandler", status);
            await base.DoWork(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
