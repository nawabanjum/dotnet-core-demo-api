namespace DemoWebAPI.Middlewares
{
    public class BasicResponse
    {
        public BasicResponse()
        {
            Success = true;
        }

        public bool Success { get; set; }
        public dynamic Data { get; set; }
        public string ErrorMessasge { get; set; }
    }
}
