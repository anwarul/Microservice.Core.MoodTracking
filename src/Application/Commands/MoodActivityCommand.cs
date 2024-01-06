namespace RS.MF.MoodTracking.Application.Commands
{
    public class MoodActivityCommand : MoodActivityDto, ICommand<RequestResponse<CommandResult>>
    {
        public string CorrelationId { get; set; }
        public string SpanId { get; set; }
    }
}