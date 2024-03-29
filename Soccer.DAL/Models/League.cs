﻿using Soccer.DAL.Attributes;

namespace Soccer.DAL.Models
{
    [BsonCollection("leagues")]
    public class League : Document
    {
        public string? Logo { get; set; }
        public string Country { get; set; } = null!;
        public string? Flag { get; set; }
    }
}
