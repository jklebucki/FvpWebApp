using FvpWebApp.Models;
using System;

namespace FvpWebApp.Infrastructure
{
    public class DatesFromMonth
    {
        public static DateTime DateFrom(int year, int month)
        {
            return new DateTime(year, month, 1, 0, 0, 0);
        }
        public static DateTime DateTo(int year, int month)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);
        }
        public static DateTime DateFrom(CreateTicketRequest createTicketRequest)
        {
            return new DateTime(createTicketRequest.Year, createTicketRequest.Month, 1, 0, 0, 0);
        }
        public static DateTime DateTo(CreateTicketRequest createTicketRequest)
        {
            return new DateTime(createTicketRequest.Year, createTicketRequest.Month, DateTime.DaysInMonth(createTicketRequest.Year, createTicketRequest.Month), 23, 59, 59);
        }

    }
}
