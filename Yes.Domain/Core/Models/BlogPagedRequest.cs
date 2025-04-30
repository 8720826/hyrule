namespace Yes.Domain.Core.Models
{
    public class BlogPagedRequest : IPagedRequest
    {
        public const int MaxPageSize = 100;
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }


    }
}
