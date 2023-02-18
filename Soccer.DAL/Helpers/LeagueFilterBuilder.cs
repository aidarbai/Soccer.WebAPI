using MongoDB.Bson;
using MongoDB.Driver;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using System.Text.RegularExpressions;

namespace Soccer.DAL.Helpers
{
    public static class LeagueFilterBuilder
    {
        public static FilterDefinition<League> Build(LeagueSearchModel searchModel)
        {
            var filter = Builders<League>.Filter.Empty;

            var name = searchModel.Name;
            if (!string.IsNullOrEmpty(name))
            {
                if (name.First() == '*' || name.Last() == '*')
                {
                    name = name.Replace("*", string.Empty);
                    var builder = Builders<League>.Filter;
                    var queryExpr = new BsonRegularExpression(new Regex(name, RegexOptions.IgnoreCase));
                    return builder.Regex("Name", queryExpr) | builder.Regex("Country", queryExpr);
                }

                else
                {
                    return Builders<League>.Filter.Text(name);
                }
            }

            return filter;
        }
    }
}
