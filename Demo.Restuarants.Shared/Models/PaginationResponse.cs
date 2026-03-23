namespace Demo.Restuarants.Shared.Models;

public record PaginationResponse<T>
(
    IEnumerable<T> Data,
    PaginationMetaData MetaData
) where T : class
{
    // adding parameterless default constructor for deserialization
    public PaginationResponse() : this([], new PaginationMetaData())
    {

    }
}
