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
            For<SpeedRun>().TableName("tbl_SpeedRun");
            For<SpeedRunGridView>().TableName("vw_SpeedRunGrid").Columns(i =>
            {
                i.Column(g => g.IsPersonalBest).Ignore();
                i.Column(g => g.SubCategoryVariableValues).Ignore();
                i.Column(g => g.Players).Ignore();
                i.Column(g => g.VariableValues).Ignore();
                i.Column(g => g.Videos).Ignore();
            }); 
            For<SpeedRunGridPlayerView>().TableName("vw_SpeedRunGridPlayer").Columns(i =>
            {
                i.Column(g => g.IsPersonalBest).Ignore();
                i.Column(g => g.SubCategoryVariableValues).Ignore();
                i.Column(g => g.Players).Ignore();
                i.Column(g => g.VariableValues).Ignore();
                i.Column(g => g.Videos).Ignore();
            }); 
            For<SpeedRunSummaryView>().TableName("vw_SpeedRunSummary").Columns(i =>
            {
                i.Column(g => g.Players).Ignore();
                i.Column(g => g.SubCategoryVariableValueNames).Ignore();
                i.Column(g => g.Videos).Ignore();
            }); 
            For<SpeedRunDetailView>().TableName("vw_SpeedRunDetail").Columns(i =>
            {
                i.Column(g => g.Players).Ignore();
                i.Column(g => g.VariableValues).Ignore();
                i.Column(g => g.Videos).Ignore();
            }); 

            For<GameView>().TableName("vw_Game").Columns(i =>
            {
                i.Column(g => g.GameCategoryTypes).Ignore();
                i.Column(g => g.Categories).Ignore();
                i.Column(g => g.Levels).Ignore();
                i.Column(g => g.Variables).Ignore();
                i.Column(g => g.VariableValues).Ignore();
                i.Column(g => g.GamePlatforms).Ignore();
                i.Column(g => g.SantizedName).Ignore();
                i.Column(g => g.SantizedNameNoSpace).Ignore();
            }); 
            For<Game>().PrimaryKey("ID").TableName("tbl_Game");
            For<GameCategoryType>().PrimaryKey("ID").TableName("tbl_Game_CategoryType");
            For<GamePlatform>().PrimaryKey("ID").TableName("tbl_Game_Platform");
            For<Category>().PrimaryKey("ID").TableName("tbl_Category").Columns(i =>
            {
                i.Column(g => g.HasData).Ignore();
            });           
            For<Level>().PrimaryKey("ID").TableName("tbl_Level").Columns(i =>
            {
                i.Column(g => g.HasData).Ignore();
            });                   
            For<Variable>().PrimaryKey("ID").TableName("tbl_Variable").Columns(i =>
            {
                i.Column(g => g.HasData).Ignore();  
                i.Column(g => g.IsSingleCategory).Ignore();
                i.Column(g => g.VariableValues).Ignore();
            });                 
            For<VariableValue>().PrimaryKey("ID").TableName("tbl_VariableValue").Columns(i =>
            {
                i.Column(g => g.HasData).Ignore();
                i.Column(g => g.SubVariables).Ignore();
            });            
            For<PlayerView>().PrimaryKey("ID").TableName("vw_Player").Columns(i =>
            {
                i.Column(g => g.SantizedName).Ignore();
                i.Column(g => g.SantizedNameNoSpace).Ignore();
            }); 

            For<User>().PrimaryKey("ID").TableName("tbl_User");
            For<UserSetting>().PrimaryKey("ID").TableName("tbl_User_Setting");
            For<UserSummaryList>().PrimaryKey("ID").TableName("tbl_User_SummaryList");
            For<UserView>().TableName("vw_User");   
            For<SummaryList>().PrimaryKey("ID").TableName("tbl_SummaryList");
            For<Setting>().PrimaryKey("ID").TableName("tbl_Setting");
        }
    }
}



