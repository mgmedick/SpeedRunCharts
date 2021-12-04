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
            For<SpeedRunView>().TableName("dbo.vw_SpeedRun");
            For<SpeedRunGridView>().TableName("dbo.vw_SpeedRunGrid");
            For<SpeedRunGridTabView>().TableName("dbo.vw_SpeedRunGridTab");
            For<SpeedRunSummaryView>().TableName("dbo.vw_SpeedRunSummary");
            For<GameView>().TableName("dbo.vw_Game");
            For<UserView>().TableName("dbo.vw_User");
            For<User>().PrimaryKey("ID").TableName("dbo.tbl_User");
            For<UserAccount>().PrimaryKey("ID").TableName("dbo.tbl_UserAccount");
            For<UserAccountSetting>().PrimaryKey("UserAccountID", false).TableName("dbo.tbl_UserAccount_Setting");
            For<UserAccountView>().TableName("dbo.vw_UserAccount");
            For<UserAccountSpeedRunListCategory>().PrimaryKey("ID").TableName("dbo.tbl_UserAccount_SpeedRunListCategory");
            For<SpeedRunListCategory>().PrimaryKey("ID").TableName("dbo.tbl_SpeedRunListCategory");
        }
    }
}



