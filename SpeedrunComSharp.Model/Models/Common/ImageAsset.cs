using System;

namespace SpeedrunComSharp.Model
{
    public class ImageAsset
    {
        public Uri Uri { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ImageAsset() { }

        /*
        public static ImageAsset Parse(SpeedrunComClient client, dynamic imageElement)
        {
            if (imageElement == null)
                return null;

            var image = new ImageAsset();

            var uri = imageElement.uri as string;
            image.Uri = new Uri(uri);
            image.Width = (int)imageElement.width;
            image.Height = (int)imageElement.height;

            return image;
        }
        */
    }
}
