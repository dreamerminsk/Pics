﻿using LinqToDB;

namespace Rater.Models
{
    public class NnmContext : LinqToDB.Data.DataConnection
    {
        public NnmContext() : base("NnmClubDb")
        { }

        public ITable<TorrentInfo> Torrents => GetTable<TorrentInfo>();

        public ITable<CategoryInfo> Categories => GetTable<CategoryInfo>();

        public ITable<UserInfo> Users => GetTable<UserInfo>();

    }

}
