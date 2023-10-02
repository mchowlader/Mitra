using Microsoft.AspNetCore.Mvc;

namespace Mitra.Api.Common;

public class ServiceResponse<TEntity> where TEntity : class
{
    public TEntity data { get; set; }
    public List<string> message { get; set; }
    public bool success { get; set; }
    public int code { get; set; }
    public string token {  get; set; }
    public static ServiceResponse<TEntity> Error(string message = null)
    {
        if (message.IsNotNullOrEmpty())
        {
            if (message.Contains("Return the transaction"))
            {
                message = "Something went wrong please try again.";
            }
        }

        return new ServiceResponse<TEntity>()
        {
            data = default,
            message = new List<string> { message ?? "There was a problem handing the request." },
            success = false
        };
    }

    public static ServiceResponse<TEntity> Success(string message = null, TEntity data = null)
    {
        return new ServiceResponse<TEntity>()
        {
            data = data,
            message = new List<string> { message ?? "Request successfully." },
            success = true
        };
    }

    public static ServiceResponse<TEntity> Unauthorizes(int code, string messge = null)
    {
        return new ServiceResponse<TEntity>()
        {
            code = code, 
            message = new List<string> { messge ?? "Invalid" },
            success = false
        };
    }
}