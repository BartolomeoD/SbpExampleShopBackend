using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SbpExampleShop.Backend.Abstractions;
using SbpExampleShop.Backend.Abstractions.Repositories;

namespace SbpExampleShop.Backend.Logic
{
    public class PaymentStatusService
    {
        private readonly IAkbarsSbpIntegration _akbarsSbpIntegration;

        private readonly Dictionary<string, Task<PaymentStatus>> _checkingTasks =
            new Dictionary<string, Task<PaymentStatus>>();

        public PaymentStatusService(IAkbarsSbpIntegration akbarsSbpIntegration)
        {
            _akbarsSbpIntegration = akbarsSbpIntegration;
        }

        public void AddToChecking(string qrId)
        {
            var tcs = new TaskCompletionSource<PaymentStatus>();
            _checkingTasks.Add(qrId, tcs.Task);

            Task.Factory.StartNew(async () => { tcs.SetResult(await CheckStatus(qrId)); });
        }

        public async Task<PaymentStatus> GetStatus(string qrId)
        {
            var checkingTask = _checkingTasks[qrId];
            if (await Task.WhenAny(checkingTask, Task.Delay(TimeSpan.FromSeconds(5))) == checkingTask)
                return await checkingTask;
            return PaymentStatus.Waiting;
        }

        private async Task<PaymentStatus> CheckStatus(string qrId)
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                var status = await _akbarsSbpIntegration.GetStatus(qrId);
                if (status != PaymentStatus.Waiting)
                    return status;
            }
        }
    }
}