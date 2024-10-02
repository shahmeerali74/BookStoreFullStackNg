namespace BookStoreFullStackNg.Data.DTOs.Common;

public class BookQueryParameter:QueryParameters
{
    public int? PublishedFrom { get; set; }=null;
    public int? PublishedTo { get; set; }=null;

    public override string ToString()
    {
        return $"pageNumber:{PageNumber},pageSize:{PageSize},searchTerm:{SearchTerm},sortBy:{SortBy},publishedFrom:{PublishedFrom},publishedTo:{PublishedTo}";
    }

}

