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
            For<SpeedRunGridTabView>().TableName("vw_SpeedRunGridTab2");
            For<SpeedRunGridTabUserView>().TableName("vw_SpeedRunGridTabUser");
            For<SpeedRunSummaryView>().TableName("vw_SpeedRunSummary");
            For<GameView>().TableName("vw_Game");
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



