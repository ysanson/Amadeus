using System;
using System.Collections.Generic;
using System.Text;
using Amadeus.Nyaapi.gRPC;
using Amadeus.Nyaapi.gRPC.Models;

namespace Amadeus.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly Nyaapi.gRPC.Nyaapi nyaapi = new Nyaapi.gRPC.Nyaapi("0.0.0.0:5031");

        public List<Episode> episodes = new List<Episode>();

        public async void GetNewEpisodes()
        {
            episodes.Clear();
            await foreach(var episode in nyaapi.GetEpisodesAsync("", "HorribleSubs"))
            {
                episodes.Add(episode);
            }
        }
    }
}
