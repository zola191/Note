namespace Notebook.Server.Dto
{
    public class LogFileByPeriodRequest
    {
        public string Email { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
