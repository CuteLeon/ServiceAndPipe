using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LeonWorkerService
{
    /// <summary>
    /// Worker
    /// </summary>
    /// <remarks>
    /// �������񣺹��캯��> StartAsync> ExecuteAsync
    /// ֹͣ����StopAsync> Dispose
    /// </remarks>
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        #region ����/�ͷ� Worker

        /// <summary>
        /// ���� Worker
        /// </summary>
        /// <param name="logger"></param>
        public Worker(ILogger<Worker> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// �ͷ� Worker
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region ����/ֹͣ����

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"�������� ...");
            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// ֹͣ����
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"ֹͣ����");
            await base.StopAsync(cancellationToken);
        }
        #endregion

        /// <summary>
        /// ִ�з���
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
