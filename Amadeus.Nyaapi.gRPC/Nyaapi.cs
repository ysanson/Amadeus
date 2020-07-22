using Amadeus.Nyaapi.gRPC.Models;
using Grpc.Core;
using Grpc.Net.Client;
using NyaapiService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Amadeus.Nyaapi.gRPC
{
    public class Nyaapi
    {
        private string hostAddress;

        public Nyaapi(string hostAddress)
        {
            this.hostAddress = hostAddress;
        }

        private GetLatestEpisodesRequest.Types.Feed ConvertFeed(Feed inputFeed)
        {
            return inputFeed switch
            {
                Feed.SI => GetLatestEpisodesRequest.Types.Feed.Si,
                Feed.PANTSU => GetLatestEpisodesRequest.Types.Feed.Pantsu,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private GetLatestEpisodesRequest.Types.Quality ConvertQuality(Quality inputQuality)
        {
            return inputQuality switch
            {
                Quality.FHD => GetLatestEpisodesRequest.Types.Quality.Fhd,
                Quality.HD => GetLatestEpisodesRequest.Types.Quality.Hd,
                Quality.SD => GetLatestEpisodesRequest.Types.Quality.Sd,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public async IAsyncEnumerable<Episode> GetEpisodesAsync(string searchTerms, string fansub, Feed feed = Feed.SI, Quality quality = Quality.FHD)
        {
            var requestFeed = ConvertFeed(feed);
            var requestQuality = ConvertQuality(quality);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel = GrpcChannel.ForAddress("http://172.26.179.58:50051");
            var client = new NyaapiService.Nyaapi.NyaapiClient(channel);
            var request = new GetLatestEpisodesRequest
            {
                Feed = requestFeed,
                Quality = requestQuality,
                SearchTerms = searchTerms,
                Fansub = fansub
            };
            using var call = client.GetLatestEpisodes(request);
            var responseStream = call.ResponseStream;
            while (await responseStream.MoveNext())
            {
                var current = responseStream.Current;
                yield return new Episode
                {
                    Id = current.Id,
                    Name = current.Name,
                    Status = current.Status,
                    Hash = current.Hash,
                    Date = DateTime.Parse(current.DateISO),
                    Sub_category = current.SubCategory,
                    Category = current.Category,
                    Magnet = current.Magnet,
                    Torrent = current.Torrent
                };
            }
        }
    }
}
