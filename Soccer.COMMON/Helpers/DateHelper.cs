using Soccer.COMMON.ViewModels;

namespace Soccer.COMMON.Helpers
{
    public static class DateHelperForSearchModel
    {
        public static void ProcessAgeAndDates(PlayerSearchByParametersModel searchModel)
        {
            searchModel.DateOfBirthFrom = searchModel.DateOfBirthFrom.HasValue ?
                                          GetDateFromWithHours(searchModel.DateOfBirthFrom.Value) :
                                          searchModel.DateOfBirthFrom;

            searchModel.DateOfBirthTo = searchModel.DateOfBirthTo.HasValue ?
                                        GetDateToWithHours(searchModel.DateOfBirthTo.Value) :
                                        searchModel.DateOfBirthTo;

            if (searchModel.AgeFrom > 0)
            {
                var tempDate = DateTime.Today.AddYears(-searchModel.AgeFrom);
                searchModel.DateOfBirthTo = GetDateToWithHours(tempDate);
            }

            if (searchModel.AgeTo > 0)
            {
                var dateToYears = DateTime.Today.AddYears(-(searchModel.AgeTo + 1));
                searchModel.DateOfBirthFrom = GetDateFromWithHours(dateToYears.AddDays(1));
            }

        }

        private static DateTime GetDateFromWithHours(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        private static DateTime GetDateToWithHours(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            return date.AddHours(23).AddMinutes(59);

        }
    }
}
