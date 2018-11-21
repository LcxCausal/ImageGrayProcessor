using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.IO;

namespace ImageGrayProcessor
{
    public class ImageGrayProcessor
    {
        public ImageGrayProcessor(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                ImagePath = imagePath;
            }
            else
            {
                ImagePath = string.Empty;
            }
        }

        public string ImagePath { get; protected set; }

        public static List<Image<Rgba32>> ConvertImageToGrayscale(string imagePath, GrayscaleAlgorithmType grayscaleAlgorithmType)
        {
            if (string.IsNullOrEmpty(imagePath) || File.Exists(imagePath))
            {
                return new List<Image<Rgba32>>();
            }

            Image<Rgba32> image = Image.Load(imagePath);
            return ConvertImageToGrayscale(image, grayscaleAlgorithmType);
        }

        public static List<Image<Rgba32>> ConvertImageToGrayscale(Image<Rgba32> image, GrayscaleAlgorithmType grayscaleAlgorithmType)
        {
            if (image == null) { return null; }

            List<Image<Rgba32>> grayscaleImage = null;

            switch (grayscaleAlgorithmType)
            {
                case GrayscaleAlgorithmType.Component:
                    grayscaleImage = ConvertImageFromComponentAlgorithm(image);
                    break;
                case GrayscaleAlgorithmType.Maximum:
                    break;
                case GrayscaleAlgorithmType.Average:
                    break;
                case GrayscaleAlgorithmType.WeightedAverage:
                    break;
            }

            return grayscaleImage;
        }

        public List<Image<Rgba32>> ToGrayscale(GrayscaleAlgorithmType grayscaleAlgorithmType)
        {
            if (!File.Exists(ImagePath))
            {
                return new List<Image<Rgba32>>();
            }

            Image<Rgba32> image = Image.Load(ImagePath);
            return ConvertImageToGrayscale(image, grayscaleAlgorithmType);
        }

        private static List<Image<Rgba32>> ConvertImageFromComponentAlgorithm(Image<Rgba32> image)
        {
            List<Image<Rgba32>> grayscaleImages = new List<Image<Rgba32>>();
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            Image<Rgba32> rComponentImage = new Image<Rgba32>(imageWidth, imageWidth);
            Image<Rgba32> gComponentImage = new Image<Rgba32>(imageWidth, imageWidth);
            Image<Rgba32> bComponentImage = new Image<Rgba32>(imageWidth, imageWidth);

            for (int h = 0; h < imageHeight; h++)
            {
                for (int w = 0; w < imageWidth; w++)
                {
                    Rgba32 pixel = image[w, h];
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;

                    // R Component
                    rComponentImage[w, h] = new Rgba32(r, r, r);

                    // G Component
                    gComponentImage[w, h] = new Rgba32(g, g, g);

                    // B Component
                    bComponentImage[w, h] = new Rgba32(b, b, b);
                }
            }

            grayscaleImages.Add(rComponentImage);
            grayscaleImages.Add(gComponentImage);
            grayscaleImages.Add(bComponentImage);

            return grayscaleImages;
        }
    }
}
