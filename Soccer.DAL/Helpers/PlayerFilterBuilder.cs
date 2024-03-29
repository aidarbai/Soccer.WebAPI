﻿using MongoDB.Bson;
using MongoDB.Driver;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using System.Text.RegularExpressions;

namespace Soccer.DAL.Helpers
{
    public static class PlayerFilterBuilder
    {
        public static FilterDefinition<Player> Build(PlayerSearchByParametersModel searchModel)
        {
            var filter = Builders<Player>.Filter.Empty;

            if (!string.IsNullOrEmpty(searchModel.TeamId))
            {
                filter &= Builders<Player>.Filter.ElemMatch(x => x.Statistics, y => y.Team == searchModel.TeamId);
            }

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                filter &= GetFilterByName(searchModel.Name);
            }

            if (searchModel.DateOfBirthFrom.HasValue)
            {
                filter &= Builders<Player>.Filter.Gte(p => p.Birth.Date, searchModel.DateOfBirthFrom.Value);
            }

            if (searchModel.DateOfBirthTo.HasValue)
            {
                filter &= Builders<Player>.Filter.Lte(p => p.Birth.Date, searchModel.DateOfBirthTo.Value);
            }

            if (searchModel.CardsFrom > 0 && searchModel.CardsTo > 0)
            {
                filter &= Builders<Player>.Filter.ElemMatch(p => p.Statistics, Builders<Statistic>.Filter.Gte(s => s.Cards.Yellow, searchModel.CardsFrom)) &
                          Builders<Player>.Filter.ElemMatch(p => p.Statistics, Builders<Statistic>.Filter.Lte(s => s.Cards.Yellow, searchModel.CardsTo));

                filter &= Builders<Player>.Filter.ElemMatch(p => p.Statistics, Builders<Statistic>.Filter.Gte(s => s.Cards.Yellowred, searchModel.CardsFrom)) &
                          Builders<Player>.Filter.ElemMatch(p => p.Statistics, Builders<Statistic>.Filter.Lte(s => s.Cards.Yellowred, searchModel.CardsTo));

                filter &= Builders<Player>.Filter.ElemMatch(p => p.Statistics, Builders<Statistic>.Filter.Gte(s => s.Cards.Red, searchModel.CardsFrom)) &
                                          Builders<Player>.Filter.ElemMatch(p => p.Statistics, Builders<Statistic>.Filter.Lte(s => s.Cards.Red, searchModel.CardsTo));

                return filter;
            }

            if (searchModel.CardsFrom > 0)
            {
                filter &= Builders<Player>.Filter.ElemMatch(p => p.Statistics,
                    Builders<Statistic>.Filter.Gte(s => s.Cards.Yellow, searchModel.CardsFrom));

                filter &= Builders<Player>.Filter.ElemMatch(p => p.Statistics,
                   Builders<Statistic>.Filter.Gte(s => s.Cards.Yellowred, searchModel.CardsFrom));

                filter &= Builders<Player>.Filter.ElemMatch(p => p.Statistics,
                 Builders<Statistic>.Filter.Gte(s => s.Cards.Red, searchModel.CardsFrom));
            }

            if (searchModel.CardsTo > 0)
            {
                filter &= Builders<Player>.Filter.ElemMatch(p => p.Statistics,
                    Builders<Statistic>.Filter.Lte(s => s.Cards.Yellow, searchModel.CardsTo));

                filter &= Builders<Player>.Filter.ElemMatch(p => p.Statistics,
                   Builders<Statistic>.Filter.Lte(s => s.Cards.Yellowred, searchModel.CardsTo));

                filter &= Builders<Player>.Filter.ElemMatch(p => p.Statistics,
                 Builders<Statistic>.Filter.Lte(s => s.Cards.Red, searchModel.CardsTo));
            }

            return filter;
        }

        private static FilterDefinition<Player> GetFilterByName(string name)
        {

            if (name.First() == '*' || name.Last() == '*')
            {
                name = name.Replace("*", string.Empty);
                var builder = Builders<Player>.Filter;
                var queryExpr = new BsonRegularExpression(new Regex(name, RegexOptions.IgnoreCase));
                return builder.Regex("Firstname", queryExpr) | builder.Regex("Lastname", queryExpr);
            }

            else
            {
                return Builders<Player>.Filter.Text(name);
            }

        }
    }
}
