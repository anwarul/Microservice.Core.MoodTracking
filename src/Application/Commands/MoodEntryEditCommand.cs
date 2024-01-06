namespace RS.MF.MoodTracking.Application.Commands
{
    public class MoodEntryEditCommand : MoodEntryDto, ICommand<RequestResponse<CommandResult>>
    {
        public string CorrelationId { get; set; }
        public string SpanId { get; set; }
    }
}