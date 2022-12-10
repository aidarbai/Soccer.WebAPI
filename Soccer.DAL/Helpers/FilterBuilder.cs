using MongoDB.Driver;
using Soccer.COMMON.Helpers;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.DAL.Helpers
{
    public static class FilterBuilder
    {
        private static FilterDefinition<Player>? filter;

        private static void BuildDateOfBirthFilter(DateTime? from, DateTime? to)
        {
            if (from != null)
            {
                filter &= Builders<Player>.Filter.Gte(p => p.Birth.Date, from);
            }

            if (to != null)
            {
                filter &= Builders<Player>.Filter.Lte(p => p.Birth.Date, to);
            }
        }

        private static void ProcessAgeAndDates(PlayerSearchByParametersModel searchModel)
        {
            DateTime? dateFrom = null;
            DateTime? dateTo = null;


            if (searchModel.AgeFrom > 0)
            {
                dateTo = DateTime.Today.AddYears(-(searchModel.AgeFrom));

            }

            if (searchModel.AgeTo > 0)
            {
                var dateToYears = DateTime.Today.AddYears(-(searchModel.AgeTo + 1));

                dateFrom = dateToYears.AddDays(1);

            }

            if (searchModel.DateOfBirthFrom.HasValue)
            {
                dateFrom = searchModel.DateOfBirthFrom.Value;
            }

            if (searchModel.DateOfBirthTo.HasValue)
            {
                dateTo = searchModel.DateOfBirthTo.Value;
            }

            //dateFrom = dateFrom.HasValue ? DateHelper.GetDateFromWithHours(dateFrom.Value) : dateFrom;
            //dateTo = dateTo.HasValue ? DateHelper.GetDateToWithHours(dateTo.Value) : dateTo;

            //BuildDateOfBirthFilter(dateFrom, dateTo);

        }

        public static FilterDefinition<Player> Build(PlayerSearchByParametersModel searchModel)
        {
            filter = Builders<Player>.Filter.Empty;

            //ProcessAgeAndDates(searchModel);

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
    }
}
