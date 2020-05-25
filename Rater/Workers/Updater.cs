using LinqToDB;
using Rater.Clients;
using Rater.Models;
using Rater.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Rater.Workers
{
    public class Updater
    {

        public List<TorrentInfo> Torrents { get; } = new List<TorrentInfo>();

        public ObservableConcurrentDictionary<string, Stats> UserInfos { get; } = new ObservableConcurrentDictionary<string, Stats>();
        public ObservableConcurrentDictionary<string, Stats> CatInfos { get; } = new ObservableConcurrentDictionary<string, Stats>();
        public ObservableConcurrentDictionary<MonthYear, Stats> MonthInfos { get; } = new ObservableConcurrentDictionary<MonthYear, Stats>();


        public Updater()
        {

        }

        public int Page { get; private set; } = 0;

        public void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(16))
                .Subscribe(async t => await ProcessNextPageAsync().ConfigureAwait(true));
        }

        private async Task ProcessNextPageAsync()
        {
            var torrents = await NnmClub.GetTorrents(Page++).ConfigureAwait(true);
            torrents.ForEach(t => UpdateStats(t));

        }

        private void UpdateStats(TorrentInfo t)
        {
            using (var db = new NnmContext())
            {
                var uri = new Uri("http://nnmclub.to/" + t.Ref);
                var query = HttpUtility.ParseQueryString(uri.Query);
                var tt = query.Get("t");
                t.ID = int.Parse(tt);
                db.InsertOrReplace(t);
            }
            if (!UserInfos.ContainsKey(t.User))
            {
                UserInfos.Add(t.User, new Stats { Count = 1, Likes = t.Likes });
                using (var db = new NnmContext())
                {
                    db.Insert(new UserInfo { Name = t.User });
                }
            }
            else
            {
                UserInfos[t.User].Count += 1;
                UserInfos[t.User].Likes += t.Likes;
            }

            if (!CatInfos.ContainsKey(t.Category))
            {
                CatInfos.Add(t.Category, new Stats { Count = 1, Likes = t.Likes });
                using (var db = new NnmContext())
                {
                    db.Insert(new CategoryInfo { Name = t.Category });
                }
            }
            else
            {
                CatInfos[t.Category].Count += 1;
                CatInfos[t.Category].Likes += t.Likes;
            }
            if (!MonthInfos.ContainsKey(new MonthYear(t.Published)))
            {
                MonthInfos.Add(new MonthYear(t.Published), new Stats { Count = 1, Likes = t.Likes });
            }
            else
            {
                MonthInfos[new MonthYear(t.Published)].Count += 1;
                MonthInfos[new MonthYear(t.Published)].Likes += t.Likes;

            }
        }
    }
}
