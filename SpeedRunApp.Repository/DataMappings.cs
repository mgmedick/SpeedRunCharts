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
            /*
            For<Setting>().PrimaryKey("ID", false).TableName("dbo.tbl_Setting");
            For<Game>().PrimaryKey("ID", false).TableName("dbo.tbl_Game").Columns(i => {
                i.Column(g => g.OrderValue).Ignore();
                i.Column(g => g.ImportedDate).Ignore();
            });
            For<Level>().PrimaryKey("ID", false).TableName("dbo.tbl_Level").Columns(i => {
                i.Column(g => g.OrderValue).Ignore();
            });
            For<Category>().PrimaryKey("ID", false).TableName("dbo.tbl_Category").Columns(i => {
                i.Column(g => g.OrderValue).Ignore();
            });
            For<Variable>().PrimaryKey("ID", false).TableName("dbo.tbl_Variable").Columns(i => {
                i.Column(g => g.OrderValue).Ignore();
            });
            For<VariableValue>().PrimaryKey("ID", false).TableName("dbo.tbl_VariableValue").Columns(i => {
                i.Column(g => g.OrderValue).Ignore();
            });
            For<GamePlatform>().PrimaryKey("ID", true).TableName("dbo.tbl_Game_Platform");
            For<GameRegion>().PrimaryKey("ID", true).TableName("dbo.tbl_Game_Region");
            For<GameModerator>().PrimaryKey("ID", true).TableName("dbo.tbl_Game_Moderator");
            For<GameRuleset>().PrimaryKey("ID", true).TableName("dbo.tbl_Game_Ruleset");
            For<GameTimingMethod>().PrimaryKey("ID", true).TableName("dbo.tbl_Game_TimingMethod");
            For<User>().PrimaryKey("ID", false).TableName("dbo.tbl_User").Columns(i => {
                i.Column(g => g.OrderValue).Ignore();
                i.Column(g => g.ImportedDate).Ignore();
            });
            For<SpeedRun>().PrimaryKey("ID", false).TableName("dbo.tbl_SpeedRun").Columns(i => {
                i.Column(g => g.OrderValue).Ignore();
                i.Column(g => g.ImportedDate).Ignore();
            });
            For<SpeedRunVariableValue>().PrimaryKey("ID", true).TableName("dbo.tbl_SpeedRun_VariableValue");
            For<SpeedRunPlayer>().PrimaryKey("ID", true).TableName("dbo.tbl_SpeedRun_Player");
            For<SpeedRunVideo>().PrimaryKey("ID", true).TableName("dbo.tbl_SpeedRun_Video");
            For<Leaderboard>().PrimaryKey("ID", true).TableName("dbo.tbl_Leaderboard").Columns(i => {
                i.Column(g => g.ImportedDate).Ignore();
            });
            For<Platform>().PrimaryKey("ID", false).TableName("dbo.tbl_Platform").Columns(i =>
            {
                i.Column(g => g.OrderValue).Ignore();
                i.Column(g => g.ImportedDate).Ignore();
            });
            */
            For<SpeedRunView>().PrimaryKey("ID", true).TableName("dbo.vw_SpeedRun");
            For<GameView>().PrimaryKey("ID", true).TableName("dbo.vw_Game");
        }
    }
}



