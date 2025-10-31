namespace Spidran.Command
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        public static Result Ok(string message = "", object? data = null) => new()
        {
            Success = true,
            Message = message,
            Data = data
        };

        public static Result Fail(string message) => new()
        {
            Success = false,
            Message = message
        };
     }

   
}
