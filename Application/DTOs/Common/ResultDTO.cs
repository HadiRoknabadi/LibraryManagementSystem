namespace Application.DTOs.Common
{
    public class ResultDTO<TStatus> where TStatus:struct
    {
        public string Message { get; set; }
        public TStatus Status { get; set; }
    }

    public class ResultDTO<TStatus, TData> where TStatus : struct where TData : class
    {
        public string Message { get; set; }
        public TStatus Status { get; set; }
        public TData Data { get; set; } = default;
    }

}
