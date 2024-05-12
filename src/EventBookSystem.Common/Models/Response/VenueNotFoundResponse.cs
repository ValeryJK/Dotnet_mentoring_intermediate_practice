namespace EventBookSystem.Common.Models.Response
{
    public sealed class VenueNotFoundResponse : ApiNotFoundResponse
    {
        public VenueNotFoundResponse(Guid id) : base($"Venue with id: {id} is not found in db.") { }
    }
}
