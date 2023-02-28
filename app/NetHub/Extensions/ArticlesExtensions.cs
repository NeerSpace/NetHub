using Microsoft.EntityFrameworkCore;
using NetHub.Data.SqlServer.Context;
using NetHub.Data.SqlServer.Entities;
using NetHub.Data.SqlServer.Entities.Articles;
using NetHub.Shared.Models.Localizations;

namespace NetHub.Extensions;

public static class ArticlesExtensions
{
    public static IQueryable<ArticleLocalizationModel> GetExtendedArticles(
        this ISqlServerDatabase database, long? userId = null, bool loadBody = false, bool loadContributors = false)
    {
        IQueryable<ArticleLocalization> localizationsDbSet = database
            .Set<ArticleLocalization>();

        if (loadContributors)
            localizationsDbSet = localizationsDbSet
                .Include(l => l.Contributors)
                .ThenInclude(c => c.User);

        if (userId == null)
            return localizationsDbSet.Select(l => new ArticleLocalizationModel
            {
                Id = l.Id,
                ArticleId = l.ArticleId,
                LanguageCode = l.LanguageCode,
                Title = l.Title,
                Contributors = loadContributors
                    ? l.Contributors.Select(c => new ArticleContributorModel
                    {
                        Role = c.Role,
                        UserName = c.User!.UserName,
                        ProfilePhotoUrl = c.User!.ProfilePhotoUrl
                    }).ToArray()
                    : new ArticleContributorModel[] { },
                Description = l.Description,
                Html = loadBody ? l.Html : string.Empty,
                Views = l.Views,
                Status = l.Status,
                Created = l.Created,
                Updated = l.Updated,
                Published = l.Published,
                Banned = l.Banned,
                Rate = l.Article!.Rate,
            });

        var votesDbSet = database.Set<ArticleVote>();
        var savedDbSet = database.Set<SavedArticle>();

        var result = from l in localizationsDbSet
                         //Join votes
                     join _v in votesDbSet on
                         new { l.ArticleId, UserId = (long)userId }
                         equals
                         new { _v.ArticleId, _v.UserId }
                         into votes
                     from v in votes.DefaultIfEmpty()
                         //Join savings
                     join _s in savedDbSet on
                         new { LocalizationId = l.Id, UserId = (long)userId }
                         equals
                         new { _s.LocalizationId, _s.UserId }
                         into saved
                     from s in saved.DefaultIfEmpty()
                     select new ArticleLocalizationModel
                     {
                         Id = l.Id,
                         ArticleId = l.ArticleId,
                         LanguageCode = l.LanguageCode,
                         Title = l.Title,
                         Contributors = loadContributors
                             ? l.Contributors.Select(c => new ArticleContributorModel
                             {
                                 Role = c.Role,
                                 UserName = c.User!.UserName,
                                 ProfilePhotoUrl = c.User!.ProfilePhotoUrl
                             }).ToArray()
                             : new ArticleContributorModel[] { },
                         Description = l.Description,
                         Html = loadBody ? l.Html : string.Empty,
                         Views = l.Views,
                         Status = l.Status,
                         Created = l.Created,
                         Updated = l.Updated,
                         Published = l.Published,
                         Banned = l.Banned,
                         Vote = v.Vote,
                         Rate = l.Article!.Rate,
                         IsSaved = s != null,
                         SavedDate = s != null ? s.SavedDate : null
                     };

        return result;
    }
}