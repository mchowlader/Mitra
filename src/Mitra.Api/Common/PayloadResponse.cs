namespace Mitra.Api.Common;

public class PayloadResponse<TEntity> where TEntity : class
{
    private readonly IHttpContextAccessor _contextAccessor;
    public PayloadResponse()
    {
        _contextAccessor = new HttpContextAccessor();
        this.RequestUrl = _contextAccessor.HttpContext != null ? $"{_contextAccessor.HttpContext.Request.Scheme} : //{_contextAccessor.HttpContext.Request.Host}{_contextAccessor.HttpContext.Request.PathBase}{_contextAccessor.HttpContext.Request.Path}" : "";
        this.ResponseTime = Utilities.GetRequestResponseTime(); 
    }

    public bool Success { get; set; }
    public string RequestTime { get; set; }
    public string ResponseTime { get; set; }
    public string RequestUrl { get; set; }
    public List<string> Message { get; set;}
    public TEntity Payload { get;set; }
    public string PayloadType { get; set; }
}