namespace BookStoreFullStackNg.Data.DTOs.Common;

public class BookQueryParameter:QueryParameters
{
    public PublishFilterParameter? PublishFilterParameter { get; set; }
}

public class PublishFilterParameter
{
    public DateTime? PublishedFrom;
    public DateTime? PublishedTo;
}
