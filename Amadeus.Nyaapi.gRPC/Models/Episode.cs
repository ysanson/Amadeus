using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Amadeus.Nyaapi.gRPC.Models
{
    public class Episode
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Hash { get; set; }
        public DateTime Date { get; set; }
        public string Sub_category { get; set; }
        public string Category { get; set; }
        public string Magnet { get; set; }
        public string Torrent { get; set; }
        // public string FormattedDescription { get { return Windows.Data.Html.HtmlUtilities.ConvertToText(description); } }
        public string FormattedTitle { get => Regex.Replace(Name, @"\[.*?\]|(\.[a-z]{3})", ""); }

        public string Team
        {
            get
            {
                var extractedTeam = Regex.Match(Name, @"^\[.*?\]").Value;
                if (extractedTeam.Length == 0)
                    return "Team inconnue";
                return "Team : " + extractedTeam.Substring(1, extractedTeam.Length - 2);
            }
        }

        public override string ToString() => Name + "\nId: " + Id;
    }
}
