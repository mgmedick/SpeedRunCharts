using System;

namespace SpeedrunComSharp.Model
{
    public class GameHeader : IElementWithID
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string Abbreviation { get; set; }
        public Uri WebLink { get; set; }

        public GameHeader() { }

        /*
        public static GameHeader Parse(SpeedrunComClient client, dynamic gameHeaderElement)
        {
            var gameHeader = new GameHeader();

            gameHeader.ID = gameHeaderElement.id as string;
            gameHeader.Name = gameHeaderElement.names.international as string;
            gameHeader.JapaneseName = gameHeaderElement.names.japanese as string;
            gameHeader.WebLink = new Uri(gameHeaderElement.weblink as string);
            gameHeader.Abbreviation = gameHeaderElement.abbreviation as string;

            return gameHeader;
        }
        */
        public override int GetHashCode()
        {
            return (ID ?? string.Empty).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as GameHeader;

            if (other == null)
                return false;

            return ID == other.ID;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
