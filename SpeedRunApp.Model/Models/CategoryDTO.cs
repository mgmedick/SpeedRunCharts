using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class CategoryDTO
    {
        public CategoryDTO(Category category)
        {
            ID = category.ID;
            Name = category.Name;
            Type = category.Type;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
    }
}
