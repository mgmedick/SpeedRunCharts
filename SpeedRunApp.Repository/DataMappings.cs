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
            For<SpeedRunView>().TableName("vw_SpeedRun");
            For<SpeedRunGridView>().TableName("vw_SpeedRunGrid");
            For<SpeedRunGridUserView>().TableName("vw_SpeedRunGridUser");
            For<WorldRecordGridView>().TableName("vw_WorldRecordGrid");        
            For<SpeedRunGridTabView>().TableName("vw_SpeedRunGridTab");
            For<SpeedRunSummaryView>().TableName("vw_SpeedRunSummary");
            For<SpeedRunChartView>().TableName("vw_SpeedRunChart");
            For<SpeedRunChartUserView>().TableName("vw_SpeedRunChartUser");
            For<GameView>().TableName("vw_Game");
            For<Game>().PrimaryKey("ID").TableName("tbl_Game");
            For<UserView>().TableName("vw_User");
            For<User>().PrimaryKey("ID").TableName("tbl_User");
            For<UserAccount>().PrimaryKey("ID").TableName("tbl_UserAccount");
            For<UserAccountSetting>().PrimaryKey("UserAccountID", false).TableName("tbl_UserAccount_Setting");
            For<UserAccountView>().TableName("vw_UserAccount");
            For<UserAccountSpeedRunListCategory>().PrimaryKey("ID").TableName("tbl_UserAccount_SpeedRunListCategory");
            For<SpeedRunListCategory>().PrimaryKey("ID").TableName("tbl_SpeedRunListCategory");
            For<Setting>().PrimaryKey("ID").TableName("tbl_Setting");
        }
    }
}



