using System;
using System.Collections.Generic;
using System.Text;
using NPoco;
using NPoco.FluentMappings;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace SpeedRunApp.Repository.Configuration
{
    public static class NPocoBootstrapper
    {
        public static void Configure(string connectionString, bool isMySQL)
        {
            var fluentConfig = FluentMappingConfiguration.Configure(new Repository.DataMappings());

            BaseRepository.DBFactory = DatabaseFactory.Config(i =>
            {
                i.UsingDatabase(() => isMySQL ? new Database(connectionString, DatabaseType.MySQL, MySqlClientFactory.Instance, System.Data.IsolationLevel.ReadUncommitted) :
                                                new Database(connectionString, DatabaseType.SqlServer2012, SqlClientFactory.Instance, System.Data.IsolationLevel.ReadUncommitted));
                i.WithFluentConfig(fluentConfig);
            });

            BaseRepository.IsMySQL = isMySQL;
        }
    }
}
