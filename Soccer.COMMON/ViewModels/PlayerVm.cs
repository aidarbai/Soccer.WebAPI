﻿using static Soccer.COMMON.Models.PlayerInnerModels;

namespace Soccer.COMMON.ViewModels
{
    public class PlayerVM
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public int Age { get; set; }

        public Birth Birth { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public string Height { get; set; } = null!;

        public string Weight { get; set; } = null!;

        public bool Injured { get; set; }

        public string Photo { get; set; } = null!;

        public List<StatisticVM> Statistics { get; set; } = new();
    }
}