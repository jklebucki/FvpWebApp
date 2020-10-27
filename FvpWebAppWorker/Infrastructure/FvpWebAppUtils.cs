using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Infrastructure
{
    public class FvpWebAppUtils
    {
        public static List<List<T>> DividedDataList<T>(List<T> data, int maxSalesRows)
        {
            List<List<T>> dividedList = new List<List<T>>();
            int divideCount = (int)Math.Ceiling((double)data.Count / (double)maxSalesRows);
            int index = 0;
            for (int i = 1; i <= divideCount; i++)
            {
                List<T> rows;
                if (i < divideCount)
                    rows = data.GetRange(index, maxSalesRows);
                else
                    rows = data.GetRange(index, (data.Count - (maxSalesRows * (i - 1))));
                dividedList.Add(rows);
                index = (maxSalesRows * i);
            }
            return dividedList;
        }

        public static short CheckUeCountry(List<Country> countries, string countryCode)
        {
            return (countries != null && countryCode != "PL") ? (countries.Select(c => c.Symbol).Contains(countryCode) ? (short)1 : (short)0) : (short)0;
        }

        public static string GetDigitsFromString(string stringWithDigist)
        {
            return new string(stringWithDigist.Where(Char.IsDigit).ToArray());
        }

        public static string TruncateToLength(string stringToCut, int length)
        {
            if (stringToCut.Length > length)
                return stringToCut.Substring(0, length);
            else
                return stringToCut;
        }
        public static async Task ChangeTicketStatus(WorkerAppDbContext dbContext, int ticketId, TicketStatus ticketStatus)
        {
            dbContext.DetachAllEntities();
            var ticket = await dbContext.TaskTickets.FirstOrDefaultAsync(f => f.TaskTicketId == ticketId);
            ticket.TicketStatus = ticketStatus;
            ticket.StatusChangedAt = DateTime.Now;
            dbContext.Update(ticket);
            await dbContext.SaveChangesAsync();
        }
    }
}
