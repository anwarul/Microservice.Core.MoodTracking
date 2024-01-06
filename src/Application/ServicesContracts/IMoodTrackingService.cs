namespace RS.MF.MoodTracking.Application.ServicesContracts;

public interface IMoodTrackingService
{
    Task Processor(MoodTrackingEvent @event, CancellationToken cancellationToken);

    Task Processor(MoodTrackingEditEvent @event, CancellationToken cancellationToken);
}