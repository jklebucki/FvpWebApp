using FvpWebApp.Data;
using FvpWebApp.Models;
using FvpWebApp.Services.Interfaces;
using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Services
{
    public class SourceService : ISourceService
    {
        private readonly ApplicationDbContext _context;
        public SourceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WebAppMessage> UpdateSource(SourceAggregate sourceAggregate)
        {
            var message = new WebAppMessage { IsError = false, MessageText = "" };
            try
            {
                using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
                {
                    var source = await _context.Sources.FirstOrDefaultAsync(s => s.SourceId == sourceAggregate.Source.SourceId);
                    if (source == null)
                        throw new Exception("Nie znaleziono danych do aktualizacji");
                    var accountingRecords = await _context.AccountingRecords.Where(a => a.SourceId == sourceAggregate.Source.SourceId).ToListAsync();

                    if (accountingRecords != null)
                    {
                        accountingRecords.Clear();
                        await _context.SaveChangesAsync();
                    }

                    source.TargetId = sourceAggregate.Source.TargetId;
                    source.Description = sourceAggregate.Source.Description;
                    source.Code = sourceAggregate.Source.Code;
                    source.Type = sourceAggregate.Source.Type;
                    source.Address = sourceAggregate.Source.Address;
                    source.DbName = sourceAggregate.Source.DbName;
                    source.Username = sourceAggregate.Source.Username;
                    source.Password = sourceAggregate.Source.Password;
                    source.AccountingRecords = sourceAggregate.Source.AccountingRecords;
                    await _context.SaveChangesAsync();

                    var targetDocumentSettings = await _context.TargetDocumentsSettings.FirstOrDefaultAsync(t => t.SourceId == source.SourceId);
                    if (targetDocumentSettings == null)
                    {
                        targetDocumentSettings = new TargetDocumentSettings
                        {
                            SourceId = source.SourceId,
                            DocumentShortcut = sourceAggregate.TargetDocumentSettings.DocumentShortcut,
                            VatRegisterId = sourceAggregate.TargetDocumentSettings.VatRegisterId,
                            VatRegisters = (sourceAggregate.TargetDocumentSettings.VatRegisters != null && sourceAggregate.TargetDocumentSettings.VatRegisters.Count > 0) ? sourceAggregate.TargetDocumentSettings.VatRegisters : null
                        };
                        await _context.AddAsync(targetDocumentSettings);
                        await _context.SaveChangesAsync();
                    }

                    var vatRegisters = await _context.VatRegisters.Where(v => v.TargetDocumentSettingsId == targetDocumentSettings.TargetDocumentSettingsId).ToListAsync();
                    if (vatRegisters != null
                        && sourceAggregate.TargetDocumentSettings.VatRegisters != null
                        && sourceAggregate.TargetDocumentSettings.VatRegisters.Count > 0)
                    {
                        vatRegisters.Clear();
                        await _context.SaveChangesAsync();
                    }

                    targetDocumentSettings.DocumentShortcut = sourceAggregate.TargetDocumentSettings.DocumentShortcut;
                    targetDocumentSettings.VatRegisterId = sourceAggregate.TargetDocumentSettings.VatRegisterId;
                    if (sourceAggregate.TargetDocumentSettings.VatRegisters != null && sourceAggregate.TargetDocumentSettings.VatRegisters.Count > 0)
                        targetDocumentSettings.VatRegisters = sourceAggregate.TargetDocumentSettings.VatRegisters;
                    await _context.SaveChangesAsync();

                    await dbContextTransaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.MessageText = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return message;
        }


        public async Task<WebAppMessage> ClearContractors(int sourceId)
        {
            var message = new WebAppMessage { IsError = false, MessageText = "" };
            try
            {
                var contractors = await _context.Contractors.Where(c => c.SourceId == sourceId).ToListAsync();
                if (contractors != null && contractors.Count > 0)
                {
                    contractors.ForEach(c => { c.ContractorErpId = null; c.ContractorErpPosition = null; });
                    await _context.SaveChangesAsync();
                    message.MessageText = $"Zaktualizowano {contractors.Count} kontrahentów";
                }
            }
            catch (Exception ex)
            {
                message.IsError = true;
                message.MessageText = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return message;
        }
    }
}
