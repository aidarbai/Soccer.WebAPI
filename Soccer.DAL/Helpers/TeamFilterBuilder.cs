using MongoDB.Bson;
using MongoDB.Driver;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Soccer.DAL.Helpers
{
    public static class TeamFilterBuilder
    {
        public static FilterDefinition<Team> Build(TeamSearchModel searchModel)
        {
            var filter = Builders<Team>.Filter.Empty;

            if (!string.IsNullOrEmpty(searchModel.LeagueId))
            {
                filter &= Builders<Team>.Filter.Eq("League", searchModel.LeagueId);
            }

            var name = searchModel.Name;
            if (!string.IsNullOrEmpty(name))
            {
                if (name.First() == '*' || name.Last() == '*')
                {
                    name = name.Replace("*", string.Empty);
                    var builder = Builders<Team>.Filter;
                    var queryExpr = new BsonRegularExpression(new Regex(name, RegexOptions.IgnoreCase));
                    return builder.Regex("Name", queryExpr) | builder.Regex("City", queryExpr);
                }

                else
                {
                    return Builders<Team>.Filter.Text(name);
                }
            }
            

            return filter;
        }
    }
}
