namespace EventBookSystem.Common.Models.Response
{
    public sealed class ApiOkResponse<TResult> : ApiBaseResponse
    {
        public TResult Result { get; set; }
        public ApiOkResponse(TResult result) : base(true)
        {
            Result = result;
        }
    }
}
