using System;
using System.Collections.Generic;
using System.Text;
using NPoco.FluentMappings;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Repository
{
    public class DataMappings : Mappings
    {
        public DataMappings()
        {
            For<SpeedRunView>().PrimaryKey("ID", true).TableName("dbo.vw_SpeedRun");
            For<SpeedRunGridView>().PrimaryKey("ID", true).TableName("dbo.vw_SpeedRunGrid");
            For<SpeedRunSummaryView>().PrimaryKey("ID", true).TableName("dbo.vw_SpeedRunSummary");
            For<GameView>().PrimaryKey("ID", true).TableName("dbo.vw_Game");
            For<UserView>().PrimaryKey("ID", true).TableName("dbo.vw_User");
        }
    }
}



