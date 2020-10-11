using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Services.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace FvpWebAppWorker.Services
{
    public class SBenDataService : ISourceDataService
    {
        public async Task<List<Document>> GetDocuments(Source source)
        {
            List<Document> documents = new List<Document>();
            string sqlCommandText = "";
            try
            {
                sqlCommandText = (await FileUtils.GetQueryFile("sben.sql")).Replace("dataOd", "2020-08-01").Replace("dataDo", "2020-09-30");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            using (OracleConnection conn = new OracleConnection("Data Source=" + source.Address + "/XE;User ID=" + source.Username + ";Password=" + source.Password + ";Connection Timeout=9999;"))
            {
                using (OracleCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlCommandText;
                    try
                    {
                        conn.Open();
                        Console.WriteLine($"Query: {DateTime.Now}");
                        using (OracleDataReader rdr = cmd.ExecuteReader())
                        {
                            Console.WriteLine($"End query: {DateTime.Now}");
                            var cratedAt = DateTime.Now;
                            var dt = new DataTable();
                            dt.Load(rdr);
                            Console.WriteLine($"Dt loaded: {DateTime.Now}");
                            rdr.Close();
                            foreach (DataRow row in dt.Rows)
                            {
                                //var obj = rdr;
                                documents.Add(new Document
                                {
                                    SourceId = source.SourceId,
                                    ExternalId = int.Parse(row["DOKID"].ToString()),
                                    DocumentNumber = row["NRDOK"].ToString(),
                                    DocumentSymbol = row["TYPDOK"].ToString(),
                                    SaleDate = DateTime.Parse(row["DATASPRZEDAZY"].ToString()),
                                    DocumentDate = DateTime.Parse(row["DATA"].ToString()),
                                    Net = decimal.Parse(row["NETTO"].ToString()),
                                    Gross = decimal.Parse(row["BRUTTO"].ToString()),
                                    Vat = decimal.Parse(row["VAT"].ToString()),
                                    DocumentValid = false,
                                    DocContractorId = row["KONTRAHID"].ToString(),
                                    DocContractorName = row["KONTRAHNAZWA"].ToString(),
                                    DocContractorVatCode = row["NIP"].ToString(),
                                    DocContractorCity = row["MIEJSCOWOSC"].ToString(),
                                    DocContractorPostCode = row["KODPOCZTOWY"].ToString(),
                                    DocContractorCountryCode = row["KODKRAJU"].ToString(),
                                    DocContractorStreetAndNumber = row["ULICANR"].ToString(),
                                    CreatedAt = cratedAt,
                                    DocumentVats = ParseDocumentVat(row),
                                });
                            }
                        }
                        conn.Close();
                        Console.WriteLine($"Documents done: {DateTime.Now}");
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw ex;
                    }

                };
            }
            //var test = documents.Where(c => c.DocumentVats.Select(s => s.VatCode).ToList().Contains(new List<string> { "C", "D" }) && c.DocumentVats.Count > 1).ToList();

            return await Task.FromResult<List<Document>>(documents).ConfigureAwait(false);
        }
        private List<DocumentVat> ParseDocumentVat(DataRow row)
        {
            List<DocumentVat> documentVats = new List<DocumentVat>();
            if (decimal.Parse(row["NETTO_A"].ToString()) > 0)
            {
                documentVats.Add(new DocumentVat
                {
                    VatCode = "A",
                    VatAmount = decimal.Parse(row["VAT_A"].ToString()),
                    NetAmount = decimal.Parse(row["NETTO_A"].ToString()),
                    GrossAmount = decimal.Parse(row["BRUTTO_A"].ToString()),
                });
            }
            if (decimal.Parse(row["NETTO_B"].ToString()) > 0)
            {
                documentVats.Add(new DocumentVat
                {
                    VatCode = "B",
                    VatAmount = decimal.Parse(row["VAT_B"].ToString()),
                    NetAmount = decimal.Parse(row["NETTO_B"].ToString()),
                    GrossAmount = decimal.Parse(row["BRUTTO_B"].ToString()),
                });
            }
            if (decimal.Parse(row["NETTO_C"].ToString()) > 0)
            {
                documentVats.Add(new DocumentVat
                {
                    VatCode = "C",
                    VatAmount = decimal.Parse(row["VAT_C"].ToString()),
                    NetAmount = decimal.Parse(row["NETTO_C"].ToString()),
                    GrossAmount = decimal.Parse(row["BRUTTO_C"].ToString()),
                });
            }
            if (decimal.Parse(row["NETTO_D"].ToString()) > 0)
            {
                documentVats.Add(new DocumentVat
                {
                    VatCode = "D",
                    VatAmount = decimal.Parse(row["VAT_D"].ToString()),
                    NetAmount = decimal.Parse(row["NETTO_D"].ToString()),
                    GrossAmount = decimal.Parse(row["BRUTTO_D"].ToString()),
                });
            }
            if (decimal.Parse(row["NETTO_E"].ToString()) > 0)
            {
                documentVats.Add(new DocumentVat
                {
                    VatCode = "E",
                    VatAmount = decimal.Parse(row["VAT_E"].ToString()),
                    NetAmount = decimal.Parse(row["NETTO_E"].ToString()),
                    GrossAmount = decimal.Parse(row["BRUTTO_E"].ToString()),
                });
            }
            if (decimal.Parse(row["NETTO_F"].ToString()) > 0)
            {
                documentVats.Add(new DocumentVat
                {
                    VatCode = "F",
                    VatAmount = decimal.Parse(row["VAT_F"].ToString()),
                    NetAmount = decimal.Parse(row["NETTO_F"].ToString()),
                    GrossAmount = decimal.Parse(row["BRUTTO_F"].ToString()),
                });
            }
            if (decimal.Parse(row["NETTO_Z"].ToString()) > 0)
            {
                documentVats.Add(new DocumentVat
                {
                    VatCode = "Z",
                    VatAmount = decimal.Parse(row["VAT_Z"].ToString()),
                    NetAmount = decimal.Parse(row["NETTO_Z"].ToString()),
                    GrossAmount = decimal.Parse(row["BRUTTO_Z"].ToString()),
                });
            }
            if (decimal.Parse(row["NETTO_NP"].ToString()) > 0)
            {
                documentVats.Add(new DocumentVat
                {
                    VatCode = "NP",
                    VatAmount = decimal.Parse(row["VAT_NP"].ToString()),
                    NetAmount = decimal.Parse(row["NETTO_NP"].ToString()),
                    GrossAmount = decimal.Parse(row["BRUTTO_NP"].ToString()),
                });
            }
            return documentVats;
        }
    }
}