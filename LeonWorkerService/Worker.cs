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
    /// 启动服务：构造函数> StartAsync> ExecuteAsync
    /// 停止服务：StopAsync> Dispose
    /// </remarks>
    public class Worker : BackgroundService
    {
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
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
