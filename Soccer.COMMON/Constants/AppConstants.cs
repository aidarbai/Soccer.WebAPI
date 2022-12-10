using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Soccer.COMMON.Constants
{
    public static class AppConstants
    {
        public static class FootballAPI
        {
            public const string HEADERKEY = "X-RapidAPI-Key";
            public const string HEADERHOST = "X-RapidAPI-Host";
        }
        public enum Order
        {
            ASC,
            DESC
        }

        public enum TeamSortBy
        {
            NAME,
            FOUNDED
        }

        public static class Attributes
        {
            public const string NAME = "Name";
            public const string FOUNDED = "Founded";
        }

        public enum PlayerSortBy
        {
            FIRSTNAME,
            LASTNAME,
            AGE,
            GAMESPLAYED
        }

        //public enum Pagesize
        //{
        //    P10 = 10,
        //    P25 = 25,
        //    P50 = 50,
        //    P100 = 100
        //}

        public static readonly IReadOnlyCollection<int> pageSize = new List<int> { 10, 25, 50, 100 };
    }
}
