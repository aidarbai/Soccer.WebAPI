﻿using System.Text;
using static Soccer.COMMON.Constants.AppConstants;

namespace Soccer.COMMON.ViewModels
{
    public class TeamSearchModel
    {
        public string? LeagueId { get; set; }

        public string? Name { get; set; }
        public TeamSortBy SortBy { get; set; }
        public Order Order { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 25;

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
