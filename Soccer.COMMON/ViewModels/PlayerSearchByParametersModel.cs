using System.Text;
using static Soccer.COMMON.Constants.AppConstants;

namespace Soccer.COMMON.ViewModels
{
    public class PlayerSearchByParametersModel
    {
        public uint AgeFrom { get; set; }

        public uint AgeTo { get; set; }

        public DateTime? DateOfBirthFrom { get; set; }

        public DateTime? DateOfBirthTo { get; set; }

        public uint CardsFrom { get; set; }

        public uint CardsTo { get; set; }
        public PlayerSortBy SortBy { get; set; }
        public Order Order { get; set; }
        public uint PageNumber { get; set; } = 1;
        public uint PageSize { get; set; } = 4;

        public override string ToString()
        {
            var props = this.GetType().GetProperties();
            var sb = new StringBuilder();
            foreach (var p in props)
            {
                sb.AppendLine(p.Name + ": " + p.GetValue(this, null));
            }
            return sb.ToString();
        }
    }
}
