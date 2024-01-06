namespace RS.MF.MoodTracking.Application.Commands
{
    public class MoodEntryCommand : MoodEntryDto, ICommand<RequestResponse<CommandResult>>
    {
        public string CorrelationId { get; set; }
        public string SpanId { get; set; }
    }
}