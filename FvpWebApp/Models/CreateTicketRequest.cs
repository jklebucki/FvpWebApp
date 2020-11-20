namespace FvpWebApp.Models
{
    public class CreateTicketRequest
    {
        public string TicketsGroup { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int SourceId { get; set; }
    }
}
