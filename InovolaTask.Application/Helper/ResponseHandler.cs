namespace InovolaTask.Application.Helper;

public class ResponseHandler : IResponseHandler
{
    public GeneralResponse ErrorMessage(string Message = null)
    {
        return new GeneralResponse()
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = Message
        };
    }

    public GeneralResponse ShowMessage(string Message = null)
    {
        return new GeneralResponse()
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest,
            Message = Message
        };
    }

    public GeneralResponse Success(object entity = null, object Meta = null, string Message = null)
    {
        return new GeneralResponse()
        {
            Data = entity,
            StatusCode = System.Net.HttpStatusCode.OK,
            Succeeded = true,
            Message = Message
        };
    }

    public GeneralResponse SuccessMessage(string Message = null)
    {
        return new GeneralResponse()
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Succeeded = true,
            Message = Message
        };
    }
}
