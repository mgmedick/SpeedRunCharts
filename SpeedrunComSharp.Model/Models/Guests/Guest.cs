using System;
using System.Collections.Generic;

namespace SpeedrunComSharp.Model
{
    public class Guest
    {
        public string Name { get; set; }

        #region Links

        public Lazy<IEnumerable<Run>> runs { get; set; }
        public IEnumerable<Run> Runs { get { return runs.Value; } }
        #endregion

        public Guest() { }

        /*
        public static Guest Parse(SpeedrunComClient client, dynamic guestElement)
        {
            var guest = new Guest();

            guest.Name = guestElement.name;
            //guest.Runs = client.Runs.GetRuns(guestName: guest.Name);

            return guest;
        }
        */
        public override string ToString()
        {
            return Name;
        }
    }
}
