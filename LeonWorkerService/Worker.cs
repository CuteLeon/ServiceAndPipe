using System;
using System.IO;
using System.IO.Pipes;
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
        private const string PipeName = "DateTimePipe";
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
            using var serverStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut);

            this._logger.LogInformation($"�ܵ���������ڵȴ����� ...");
            serverStream.WaitForConnection();
            this._logger.LogInformation($"���ӳɹ�");
            using var reader = new StreamReader(serverStream);
            using var writer = new StreamWriter(serverStream) { AutoFlush = true };

            this._logger.LogInformation($"���ڵȴ����� ...");
            var request = string.Empty;
            try
            {
                while (!string.IsNullOrEmpty(request = reader.ReadLine()))
                {
                    this._logger.LogInformation($"����{request}");
                    writer.WriteLine(Guid.NewGuid().ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"��Ӧʧ�ܣ�{ex.Message}");
                serverStream.Dispose();
            }
        }
    }
}
