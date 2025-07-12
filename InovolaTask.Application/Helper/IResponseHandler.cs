namespace InovolaTask.Application.Helper;

public interface IResponseHandler
{
    public GeneralResponse Success(object entity = null, object Meta = null, string Message = null);
    public GeneralResponse SuccessMessage(string Message = null);
    public GeneralResponse ShowMessage(string Message = null);
    GeneralResponse ErrorMessage(string Message = null);
}
