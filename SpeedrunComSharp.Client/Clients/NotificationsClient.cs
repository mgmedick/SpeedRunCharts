using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedrunComSharp.Client
{
    public class NotificationsClient : BaseClient
    {

        public const string Name = "notifications";

        public NotificationsClient(ClientContainer client) : base(client)
        {
        }

        public static Uri GetNotificationsUri(string subUri)
        {
            return GetAPIUri(string.Format("{0}{1}", Name, subUri));
        }

        public IEnumerable<Notification> GetNotifications(
            int? elementsPerPage = null,
            NotificationsOrdering ordering = default(NotificationsOrdering))
        {
            var parameters = new List<string>();

            if (elementsPerPage.HasValue)
                parameters.Add(string.Format("max={0}", elementsPerPage.Value));

            parameters.AddRange(ordering.ToParameters());

            var uri = GetNotificationsUri(string.Format("{0}", 
                parameters.ToParameters()));

            return DoPaginatedRequest<Notification>(uri,
                x => Parse(x));
        }

        public Notification Parse(dynamic notificationElement)
        {
            var notification = new Notification();

            //Parse Attributes

            notification.ID = notificationElement.id as string;
            notification.TimeCreated = DateTime.Parse(notificationElement.created as string, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            notification.Status = (notificationElement.status as string).ToNotificationStatus();
            notification.Text = notificationElement.text as string;
            notification.Type = (notificationElement.item.rel as string).ToNotificationType();
            notification.WebLink = new Uri(notificationElement.item.uri as string);

            //Parse Links

            var links = notificationElement.links as IList<dynamic>;

            if (links != null)
            {
                var run = links.FirstOrDefault(x => x.rel == "run");

                if (run != null)
                {
                    var runUri = run.uri as string;
                    notification.RunID = runUri.Substring(runUri.LastIndexOf("/") + 1);
                }

                var game = links.FirstOrDefault(x => x.rel == "game");

                if (game != null)
                {
                    var gameUri = game.uri as string;
                    notification.GameID = gameUri.Substring(gameUri.LastIndexOf("/") + 1);
                }
            }

            if (!string.IsNullOrEmpty(notification.RunID))
            {
                notification.run = new Lazy<Run>(() => Client.Runs.GetRun(notification.RunID));
            }
            else
            {
                notification.run = new Lazy<Run>(() => null);
            }

            if (!string.IsNullOrEmpty(notification.GameID))
            {
                notification.game = new Lazy<Game>(() => Client.Games.GetGame(notification.GameID));
            }
            else
            {
                notification.game = new Lazy<Game>(() => null);
            }

            return notification;
        }
    }
}
