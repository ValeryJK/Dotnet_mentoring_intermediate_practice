namespace EventBookSystem.Common.Models.Response
{
    public abstract class ApiNotFoundResponse : ApiBaseResponse
    {
        public string Message { get; set; }

        protected ApiNotFoundResponse(string message) : base(false)
        {
            Message = message;
        }
    }
}