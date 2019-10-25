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
    /// 启动服务：构造函数> StartAsync> ExecuteAsync
    /// 停止服务：StopAsync> Dispose
    /// </remarks>
    public class Worker : BackgroundService
    {
        private const string PipeName = "DateTimePipe";
        private readonly ILogger<Worker> _logger;

        #region 创建/释放 Worker

        /// <summary>
        /// 创建 Worker
        /// </summary>
        /// <param name="logger"></param>
        public Worker(ILogger<Worker> logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// 释放 Worker
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region 启动/停止服务

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"启动服务 ...");
            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($"停止服务");
            await base.StopAsync(cancellationToken);
        }
        #endregion

        /// <summary>
        /// 执行服务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var serverStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut);

            this._logger.LogInformation($"管道服务端正在等待连接 ...");
            serverStream.WaitForConnection();
            this._logger.LogInformation($"连接成功");
            using var reader = new StreamReader(serverStream);
            using var writer = new StreamWriter(serverStream) { AutoFlush = true };

            this._logger.LogInformation($"正在等待请求 ...");
            var request = string.Empty;
            try
            {
                while (!string.IsNullOrEmpty(request = reader.ReadLine()))
                {
                    this._logger.LogInformation($"请求：{request}");
                    writer.WriteLine(Guid.NewGuid().ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"响应失败：{ex.Message}");
                serverStream.Dispose();
            }
        }
    }
}
