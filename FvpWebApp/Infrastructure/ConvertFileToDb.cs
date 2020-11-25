using FvpWebAppModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Infrastructure
{
    public class ConvertFileToDb
    {
        public static List<Document> Documents(List<string[]> fileData, int sourceId)
        {
            var documents = new List<Document>();
            foreach (var row in fileData)
            {
                var documentVats = GetTocumentVats(row);
                var date = DateTime.Now;
                documents.Add(new Document
                {
                    ExternalId = row[10].GetHashCode(),
                    SourceId = sourceId,
                    ContractorId = null,
                    TaskTicketId = -1,//todo
                    DocumentNumber = row[10],
                    DocumentSymbol = row[2],
                    SaleDate = string.IsNullOrEmpty(row[12]) ? Convert.ToDateTime(row[11]) : Convert.ToDateTime(row[12]),
                    DocumentDate = Convert.ToDateTime(row[11]),
                    Net = documentVats.Sum(n => n.NetAmount),
                    Gross = documentVats.Sum(g => g.GrossAmount),
                    Vat = documentVats.Sum(v => v.VatAmount),
                    JpkV7 = GetJpkDocumentTags(row),
                    DocumentStatus = DocumentStatus.Added,
                    DocContractorId = string.IsNullOrEmpty(row[8]) ? row[9].GetHashCode().ToString() : row[8].GetHashCode().ToString(),
                    DocContractorName = row[9],
                    DocContractorVatId = row[8].Replace("-", "").Replace(" ", ""),
                    DocContractorCity = null,
                    DocContractorPostCode = null,
                    DocContractorCountryCode = string.IsNullOrEmpty(row[7]) ? "-" : row[7],
                    DocContractorStreetAndNumber = row[78],
                    DocContractorFirm = -1,
                    CreatedAt = date,
                    UpdatedAt = date,
                    UpdatedBy = null,
                    DocumentVats = documentVats
                });
            }
            return documents;
        }

        public static List<DocumentVat> GetTocumentVats(string[] row)
        {
            var documentVats = new List<DocumentVat>();
            if (decimal.Parse(row[50].ToString()) != 0)
            {
                var vatAmount = decimal.Parse(row[51].Replace(',','.'));
                var netAmount = decimal.Parse(row[50].Replace(',', '.'));
                documentVats.Add(new DocumentVat
                {
                    VatCode = "A",
                    VatAmount = vatAmount,
                    NetAmount = netAmount,
                    GrossAmount = vatAmount + netAmount,
                    VatValue = 23,
                    VatTags = GetJpkVatTags(row),
                });
            }
            if (decimal.Parse(row[48].ToString()) != 0)
            {
                var vatAmount = decimal.Parse(row[49].Replace(',', '.'));
                var netAmount = decimal.Parse(row[48].Replace(',', '.'));
                documentVats.Add(new DocumentVat
                {
                    VatCode = "B",
                    VatAmount = vatAmount,
                    NetAmount = netAmount,
                    GrossAmount = vatAmount + netAmount,
                    VatValue = 8,
                    VatTags = GetJpkVatTags(row),
                });
            }
            if (decimal.Parse(row[46].ToString()) != 0)
            {
                var vatAmount = decimal.Parse(row[47].Replace(',', '.'));
                var netAmount = decimal.Parse(row[46].Replace(',', '.'));
                documentVats.Add(new DocumentVat
                {
                    VatCode = "C",
                    VatAmount = vatAmount,
                    NetAmount = netAmount,
                    GrossAmount = vatAmount + netAmount,
                    VatValue = 5,
                    VatTags = GetJpkVatTags(row),
                });
            }
            if (decimal.Parse(row[44].ToString()) != 0)
            {
                var netAmount = decimal.Parse(row[44].Replace(',', '.'));
                documentVats.Add(new DocumentVat
                {
                    VatCode = "D",
                    VatAmount = 0,
                    NetAmount = netAmount,
                    GrossAmount = netAmount,
                    VatValue = 0,
                    VatTags = GetJpkVatTags(row),
                });
            }
            if (decimal.Parse(row[41].ToString()) != 0)
            {
                var netAmount = decimal.Parse(row[44].Replace(',', '.'));
                documentVats.Add(new DocumentVat
                {
                    VatCode = "E",
                    VatAmount = 0,
                    NetAmount = netAmount,
                    GrossAmount = netAmount,
                    VatValue = 0,
                    VatTags = GetJpkVatTags(row),
                });
            }
            if (decimal.Parse(row[42].ToString()) != 0)
            {
                var netAmount = decimal.Parse(row[42].Replace(',', '.'));
                documentVats.Add(new DocumentVat
                {
                    VatCode = "F",
                    VatAmount = 0,
                    NetAmount = netAmount,
                    GrossAmount = netAmount,
                    VatValue = 0,
                    VatTags = GetJpkVatTags(row),
                });
            }
            return documentVats;
        }

        public static string GetJpkDocumentTags(string[] row)
        {
            string tags = string.IsNullOrEmpty(row[13]) ? "" : $"{row[13]},";
            tags += string.IsNullOrEmpty(row[27]) ? "" : "SW,";
            tags += string.IsNullOrEmpty(row[28]) ? "" : "EE,";
            tags += string.IsNullOrEmpty(row[29]) ? "" : "TP,";
            tags += string.IsNullOrEmpty(row[30]) ? "" : "TT_WNT,";
            tags += string.IsNullOrEmpty(row[31]) ? "" : "TT_D,";
            tags += string.IsNullOrEmpty(row[32]) ? "" : "MR_T,";
            tags += string.IsNullOrEmpty(row[33]) ? "" : "MR_UZ,";
            tags += string.IsNullOrEmpty(row[34]) ? "" : "I_42,";
            tags += string.IsNullOrEmpty(row[35]) ? "" : "I_63,";
            tags += string.IsNullOrEmpty(row[36]) ? "" : "B_SPV,";
            tags += string.IsNullOrEmpty(row[37]) ? "" : "B_SPV_DOSTAWA,";
            tags += string.IsNullOrEmpty(row[38]) ? "" : "B_MPV_PROWIZJA,";
            tags += string.IsNullOrEmpty(row[39]) ? "" : "MPP,";
            return tags.TrimEnd(',');
        }

        public static string GetJpkVatTags(string[] row)
        {
            string tags = string.IsNullOrEmpty(row[14]) ? "" : "GTU_01,";
            tags += string.IsNullOrEmpty(row[15]) ? "" : "GTU_02,";
            tags += string.IsNullOrEmpty(row[16]) ? "" : "GTU_03,";
            tags += string.IsNullOrEmpty(row[17]) ? "" : "GTU_04,";
            tags += string.IsNullOrEmpty(row[18]) ? "" : "GTU_05,";
            tags += string.IsNullOrEmpty(row[19]) ? "" : "GTU_06,";
            tags += string.IsNullOrEmpty(row[20]) ? "" : "GTU_07,";
            tags += string.IsNullOrEmpty(row[21]) ? "" : "GTU_08,";
            tags += string.IsNullOrEmpty(row[22]) ? "" : "GTU_09,";
            tags += string.IsNullOrEmpty(row[23]) ? "" : "GTU_10,";
            tags += string.IsNullOrEmpty(row[24]) ? "" : "GTU_11,";
            tags += string.IsNullOrEmpty(row[25]) ? "" : "GTU_12,";
            tags += string.IsNullOrEmpty(row[26]) ? "" : "GTU_13,";
            return tags.TrimEnd(',');
        }
    }
}
