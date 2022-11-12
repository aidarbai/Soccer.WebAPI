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
            DESC,
            ASC
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
            LASTNAME
        }
    }
}
