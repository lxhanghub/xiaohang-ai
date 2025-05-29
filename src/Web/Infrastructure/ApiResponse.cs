namespace Web.Infrastructure;

public class ApiResponse
{
    public int Code { get; set; }
    public object? Data { get; set; }
    public string? Message { get; set; }

    public ApiResponse(int code = 0, object? data = default, string? message = null)
    {
        Code = code;
        Data = data;
        Message = message;
    }
}
