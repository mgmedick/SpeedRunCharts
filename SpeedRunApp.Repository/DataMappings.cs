using System;
using System.Collections.Generic;
using System.Text;
using NPoco.FluentMappings;
using SpeedRunApp.Model.Entity;

namespace SpeedRunApp.Repository
{
    public class DataMappings : Mappings
    {
        public DataMappings()
        {
            For<SettingEntity>().PrimaryKey("ID", false).TableName("dbo.tbl_Setting");
        }
    }
}



