namespace RS.MF.ServiceName.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly ICommandBus _commandBus;

        private readonly ILogger<CommandController> _logger;

        public CommandController(
            ILogger<CommandController> logger,
            ICommandBus commandBus
            )
        {
            _logger = logger;
            _commandBus = commandBus;
        }

        [HttpPost("create")]
        [AnyonomusEndPoint]
        public async Task<RequestResponse<CommandResult>> Create([FromBody] MoodTrackingDto value)
        {
            var data = Mapping.Map<MoodTrackingDto, MoodTrackingCommand>(value);

            return await _commandBus.Send(data);
        }

        [HttpPatch("update/{ItemId:Guid}")]
        public async Task<RequestResponse<CommandResult>> Update([FromRoute] MoodTrackingEditCommandDto value, CancellationToken cancellationToken)
        {
            var command = Mapping.Map<MoodTrackingEditCommandDto, MoodTrackingEditCommand>(value);
            var result = await _commandBus.Send(command, cancellationToken);
            return result;
        }
    }
}