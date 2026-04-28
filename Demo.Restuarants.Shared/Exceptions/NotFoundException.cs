using System.Net;

namespace Demo.Restuarants.Shared.Exceptions;

public class NotFoundException : Exception, ICustomException
{
    public int StatusCode => (int)HttpStatusCode.NotFound;
    public string Title => "Requested Data Not Found";
    public string DataKey => "RequestId";

    public string? Id { get; }

    public NotFoundException(string msg)
        : base(msg) { }

    public NotFoundException(string msg, Exception innerException)
        : base(msg, innerException) { }

    public NotFoundException(string msg, string id)
        : this(msg)
    {
        Id = id;
        Data.Add(DataKey, Id);
    }

    public NotFoundException(string msg, Exception innerException, string id)
        : this(msg, innerException)
    {
        Id = id;
        Data.Add(DataKey, Id);
    }
}
