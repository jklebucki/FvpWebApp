using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FvpWebAppWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _provider;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _provider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = _provider.CreateScope())
                {
                    using (var _dbContext = scope.ServiceProvider.GetRequiredService<WorkerAppDbContext>())
                    {
                        var taskTicket = await _dbContext.TaskTickets.FirstOrDefaultAsync(s => s.TicketStatus == TicketStatus.Added).ConfigureAwait(false);
                        if (taskTicket != null)
                        {
                            SystemDataService systemDataService = new SystemDataService(_logger);
                            var source = await _dbContext.Sources.FirstOrDefaultAsync(i => i.SourceId == taskTicket.SourceId).ConfigureAwait(false);
                            switch (taskTicket.TicketType)
                            {
                                case TicketType.ImportDocuments:
                                    List<Document> documents = new List<Document>();
                                    if (source != null)
                                        try
                                        {
                                            switch (source.Type)
                                            {
                                                case "oracle_sben_dp":
                                                    documents = await ProceedSbenOracleDpDocuments(_dbContext, source, taskTicket, systemDataService);
                                                    break;
                                                default:
                                                    await FvpWebAppUtils.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Failed).ConfigureAwait(false);
                                                    break;
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            _logger.LogError(ex.Message);
                                        }
                                    else
                                        await FvpWebAppUtils.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Failed).ConfigureAwait(false);
                                    _logger.LogInformation($"Documents: {documents.Count}");

                                    try
                                    {
                                        await ProceedContractors(_dbContext, documents, systemDataService).ConfigureAwait(false);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex.Message);
                                    }
                                    break;
                                case TicketType.ImportContractors:
                                    break;
                                case TicketType.CheckContractors:
                                    try
                                    {
                                        await systemDataService.CheckContractors(_dbContext, taskTicket.TaskTicketId);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex.Message);
                                    }
                                    break;
                                case TicketType.MatchContractors:
                                    try
                                    {
                                        var target = await _dbContext.Targets.FirstOrDefaultAsync(t => t.TargetId == source.TargetId);
                                        await systemDataService.MatchContractors(_dbContext, taskTicket, target);
                                        await FvpWebAppUtils.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Done).ConfigureAwait(false);
                                    }
                                    catch (Exception)
                                    {
                                        await FvpWebAppUtils.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Failed).ConfigureAwait(false);
                                    }
                                    break;
                                case TicketType.ExportContractorsToErp:
                                    try
                                    {
                                        var target = await _dbContext.Targets.FirstOrDefaultAsync(t => t.TargetId == source.TargetId);
                                        TargetDataService targetDataService = new TargetDataService(_logger);
                                        await targetDataService.ExportContractorsToErp(_dbContext, taskTicket, target);
                                        await FvpWebAppUtils.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Done).ConfigureAwait(false);
                                    }
                                    catch (Exception)
                                    {
                                        await FvpWebAppUtils.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Failed).ConfigureAwait(false);
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                    };
                }
                await Task.Delay(5000).ConfigureAwait(false);
            }
        }

        private async Task<List<Document>> ProceedSbenOracleDpDocuments(WorkerAppDbContext _dbContext, Source source, TaskTicket taskTicket, SystemDataService targetDataService)
        {
            SBenDataService sBenDataService = new SBenDataService();
            var documentsResponse = await sBenDataService.GetDocuments(source, taskTicket).ConfigureAwait(false);
            if (documentsResponse != null && documentsResponse.Count > 0)
            {
                await targetDataService.TransferDocuments(documentsResponse, taskTicket, _dbContext);
            }
            return documentsResponse;
        }

        private async Task ProceedContractors(WorkerAppDbContext _dbContext, List<Document> documents, SystemDataService targetDataService)
        {
            await targetDataService.TransferContractors(documents, _dbContext);
        }


    }
}
