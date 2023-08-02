using Microsoft.AspNetCore.Razor.Language.Extensions;
using System;
using System.Globalization;

namespace HomeBanking.Models
{
    public class Card
    {
        public long Id { get; set; }
        public string CardHolder { get; set; }
        public string Tyoe { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public int Cvv { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ThruDate { get; set; }
        public long ClientId { get; set; }
        public Client Client { get; set; }

    }
}
