namespace SpeedRunApp.Model.Data
{
    public class ElementDescription
    {
        public string ID { get; set; }
        public ElementType Type { get; set; }

        public ElementDescription(string id, ElementType type)
        {
            ID = id;
            Type = type;
        }

        /*
                private static ElementType parseUriType(string type)
                {
                    switch (type)
                    {
                        case CategoriesService.Name:
                            return ElementType.Category;
                        case GamesService.Name:
                            return ElementType.Game;
                        case GuestsService.Name:
                            return ElementType.Guest;
                        case LevelsService.Name:
                            return ElementType.Level;
                        case NotificationsService.Name:
                            return ElementType.Notification;
                        case PlatformsService.Name:
                            return ElementType.Platform;
                        case RegionsService.Name:
                            return ElementType.Region;
                        case RunsService.Name:
                            return ElementType.Run;
                        case SeriesService.Name:
                            return ElementType.Series;
                        case UsersService.Name:
                            return ElementType.User;
                        case VariablesService.Name:
                            return ElementType.Variable;
                    }
                    throw new ArgumentException("type");
                }

                public static ElementDescription ParseUri(string uri)
                {
                    var splits = uri.Split('/');

                    if (splits.Length < 2)
                        return null;

                    var id = splits[splits.Length - 1];
                    var uriTypeString = splits[splits.Length - 2];

                    try
                    {
                        var uriType = parseUriType(uriTypeString);
                        return new ElementDescription(id, uriType);
                    }
                    catch
                    {
                        return null;
                    }
                }
                */
    }
}
