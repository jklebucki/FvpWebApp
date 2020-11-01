using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FvpWebApp.Data
{
    public class AccountingRecordConfiguration : IEntityTypeConfiguration<AccountingRecord>
    {
        public void Configure(EntityTypeBuilder<AccountingRecord> builder)
        {
            builder.HasData(
                new AccountingRecord
                {
                    AccountingRecordId = 1,
                    SourceId = 1,
                    RecordOrder = 1,
                    Account = "199-9",
                    Debit = "brutto",
                    Credit = string.Empty,
                    Sign = '+'
                },
                new AccountingRecord
                {
                    AccountingRecordId = 2,
                    SourceId = 1,
                    RecordOrder = 2,
                    Account = "199-9",
                    Debit = string.Empty,
                    Credit = "netto",
                    Sign = '+'
                },
                new AccountingRecord
                {
                    AccountingRecordId = 3,
                    SourceId = 1,
                    RecordOrder = 3,
                    Account = "199-9",
                    Debit = string.Empty,
                    Credit = "vat",
                    Sign = '+'
                },
                new AccountingRecord
                {
                    AccountingRecordId = 4,
                    SourceId = 2,
                    RecordOrder = 1,
                    Account = "199-23",
                    Debit = "brutto",
                    Credit = string.Empty,
                    Sign = '+'
                },
                new AccountingRecord
                {
                    AccountingRecordId = 5,
                    SourceId = 2,
                    RecordOrder = 2,
                    Account = "199-23",
                    Debit = string.Empty,
                    Credit = "netto",
                    Sign = '+'
                },
                new AccountingRecord
                {
                    AccountingRecordId = 6,
                    SourceId = 2,
                    RecordOrder = 3,
                    Account = "199-23",
                    Debit = string.Empty,
                    Credit = "vat",
                    Sign = '+'
                }
            );
        }
    }
}
