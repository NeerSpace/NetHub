using NetHub.Shared.Models;

namespace NetHub.Admin.Models.Articles;

public record ArticleFilterRequest : FilterRequest
{
    public override string? Filters { get; set; }
    public override string Sorts { get; set; } = "-created";
}