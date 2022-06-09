using Cronos;

namespace WebApplication1.HostedService
{
    public abstract class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        float sumMinutes = 0;

        public CronJobService(string expression, TimeZoneInfo timeZoneInfo)
        {
            _expression = CronExpression.Parse(expression, CronFormat.IncludeSeconds);
            _timeZoneInfo = timeZoneInfo;
        }


        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {

            await ScheduleJob(cancellationToken);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTime.UtcNow);      
            var stopAfter2Minutes = false;


            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;

                sumMinutes += (float) delay.Milliseconds/1000;

                if (sumMinutes > 120)
                {
                    await Task.CompletedTask;
                }
                else
                {

                    if (delay.TotalMilliseconds <= 0)
                    {
                        await ScheduleJob(cancellationToken);
                    }
                    _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                    _timer.Elapsed += async (sender, args) =>
                     {
                         _timer.Dispose();
                         _timer = null;
                         if (!cancellationToken.IsCancellationRequested)
                         {
                             await DoWork(cancellationToken);
                         }

                         if (!cancellationToken.IsCancellationRequested)
                         {
                             await ScheduleJob(cancellationToken);
                         }
                     };
                    _timer.Start();
                }
            }
            await Task.CompletedTask;
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {

        }
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
