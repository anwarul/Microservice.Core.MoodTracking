namespace RS.MF.MoodTracking.Application.Commands
{
    public class MoodTrackingCommand : MoodTrackingDto, ICommand<RequestResponse<CommandResult>>
    {
        public string CorrelationId { get; set; }
        public string SpanId { get; set; }
    }
}