using System.Net;

namespace InovolaTask.Application.Helper;

public class GeneralResponse
{
    public GeneralResponse()
    {

    }
    public GeneralResponse(object data, string message = null)
    {
        Succeeded = true;
        Message = message;
        Data = data;
    }
    public GeneralResponse(string message)
    {
        Succeeded = false;
        Message = message;
    }
    public GeneralResponse(string message, bool succeeded)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public HttpStatusCode StatusCode { get; set; }
    public object Meta { get; set; }

    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public object Data { get; set; }
}