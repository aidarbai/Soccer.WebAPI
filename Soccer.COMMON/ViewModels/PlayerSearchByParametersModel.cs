using System.Text;
using static Soccer.COMMON.Constants.AppConstants;
using static Soccer.COMMON.Models.PlayerInnerModels;

namespace Soccer.COMMON.ViewModels
{
    public class PlayerSearchByParametersModel
    {
        public int AgeFrom { get; set; }

        public int AgeTo { get; set; }

        public DateTime? DateOfBirthFrom { get; set; }

        public DateTime? DateOfBirthTo { get; set; }

        public int CardsFrom { get; set; }

        public int CardsTo { get; set; }
        public PlayerSortBy SortBy { get; set; }
        public Order Order { get; set; }
        public int PageNumber { get; set; }
        
        public int PageSize { get; set; } = 10;

        //public Pagesize PageSize { get; set; } = Pagesize.P10;

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
