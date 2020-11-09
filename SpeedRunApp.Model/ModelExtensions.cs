using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using System.Linq;

namespace SpeedRunApp.Model
{
    public static class ModelExtensions
    {
        public static IEnumerable<User> OrderByEnum(this IEnumerable<User> users, UsersOrdering orderBy)
        {
            IEnumerable<User> results = null;
            switch (orderBy)
            {
                case UsersOrdering.JapaneseName:
                    results = users.OrderBy(i => i.JapaneseName);
                    break;
                case UsersOrdering.JapaneseNameDescending:
                    results = users.OrderByDescending(i => i.JapaneseName);
                    break;
                case UsersOrdering.Name:
                    results = users.OrderBy(i => i.Name);
                    break;
                case UsersOrdering.NameDescending:
                    results = users.OrderByDescending(i => i.Name);
                    break;
                case UsersOrdering.Role:
                    results = users.OrderBy(i => i.Role);
                    break;
                case UsersOrdering.RoleDescending:
                    results = users.OrderByDescending(i => i.Role);
                    break;
                case UsersOrdering.SignUpDate:
                    results = users.OrderBy(i => i.SignUpDate);
                    break;
                case UsersOrdering.SignUpDateDescending:
                    results = users.OrderByDescending(i => i.SignUpDate);
                    break;
            }

            return results;
        }
    }
}
