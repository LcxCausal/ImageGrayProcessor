using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

        public static async Task<List<Image<Rgba32>>> ConvertImageToGrayscaleAsync(string imagePath, GrayscaleAlgorithmType grayscaleAlgorithmType)
        {
            if (string.IsNullOrEmpty(imagePath) || File.Exists(imagePath))
            {
                throw new ArgumentException();
            }

            Image<Rgba32> image = Image.Load(imagePath);
            return await ConvertImageToGrayscaleAsync(image, grayscaleAlgorithmType);
        }

        public static List<Image<Rgba32>> ConvertImageToGrayscale(string imagePath, GrayscaleAlgorithmType grayscaleAlgorithmType)
        {
            if (string.IsNullOrEmpty(imagePath) || File.Exists(imagePath))
            {
                return new List<Image<Rgba32>>();
            }

            Image<Rgba32> image = Image.Load(imagePath);
            return ConvertImageToGrayscale(image, grayscaleAlgorithmType);
        }

        public static async Task<List<Image<Rgba32>>> ConvertImageToGrayscaleAsync(Image<Rgba32> image, GrayscaleAlgorithmType grayscaleAlgorithmType)
        {
            if (image == null) { return null; }

            List<Image<Rgba32>> grayscaleImages = new List<Image<Rgba32>>();

            switch (grayscaleAlgorithmType)
            {
                case GrayscaleAlgorithmType.Component:
                    grayscaleImages = await Task.FromResult(ConvertImageFromComponentAlgorithm(image));
                    break;
                case GrayscaleAlgorithmType.Maximum:
                    grayscaleImages = await Task.FromResult(ConvertImageFromMaximunAlogorithm(image));
                    break;
                case GrayscaleAlgorithmType.Minimum:
                    grayscaleImages = await Task.FromResult(ConvertImageFromMinimunAlogorithm(image));
                    break;
                case GrayscaleAlgorithmType.Average:
                    grayscaleImages = await Task.FromResult(ConvertImageFromAverageAlogorithm(image));
                    break;
                case GrayscaleAlgorithmType.WeightedAverage:
                    grayscaleImages = await Task.FromResult(ConvertImageFromWeightedAverageAlogorithm(image));
                    break;
            }

            return grayscaleImages;
        }

        public static List<Image<Rgba32>> ConvertImageToGrayscale(Image<Rgba32> image, GrayscaleAlgorithmType grayscaleAlgorithmType)
        {
            if (image == null) { return null; }

            List<Image<Rgba32>> grayscaleImages = null;

            switch (grayscaleAlgorithmType)
            {
                case GrayscaleAlgorithmType.Component:
                    grayscaleImages = ConvertImageFromComponentAlgorithm(image);
                    break;
                case GrayscaleAlgorithmType.Maximum:
                    grayscaleImages = ConvertImageFromMaximunAlogorithm(image);
                    break;
                case GrayscaleAlgorithmType.Minimum:
                    grayscaleImages = ConvertImageFromMinimunAlogorithm(image);
                    break;
                case GrayscaleAlgorithmType.Average:
                    grayscaleImages = ConvertImageFromAverageAlogorithm(image);
                    break;
                case GrayscaleAlgorithmType.WeightedAverage:
                    grayscaleImages = ConvertImageFromWeightedAverageAlogorithm(image);
                    break;
            }

            return grayscaleImages;
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

        private static List<Image<Rgba32>> ConvertImageFromWeightedAverageAlogorithm(Image<Rgba32> image)
        {
            List<Image<Rgba32>> grayscaleImages = new List<Image<Rgba32>>();
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            Image<Rgba32> tmpImage = new Image<Rgba32>(imageWidth, imageHeight);

            for (int h = 0; h < imageHeight; h++)
            {
                for (int w = 0; w < imageWidth; w++)
                {
                    Rgba32 pixel = image[w, h];
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;
                    byte weightAvg = (byte)(r * 0.3 + g * 0.59 + b * 0.11);
                    tmpImage[w, h] = new Rgba32(weightAvg, weightAvg, weightAvg);
                }
            }

            grayscaleImages.Add(tmpImage);
            return grayscaleImages;
        }

        private static List<Image<Rgba32>> ConvertImageFromAverageAlogorithm(Image<Rgba32> image)
        {
            List<Image<Rgba32>> grayscaleImages = new List<Image<Rgba32>>();
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            Image<Rgba32> tmpImage = new Image<Rgba32>(imageWidth, imageHeight);

            for (int h = 0; h < imageHeight; h++)
            {
                for (int w = 0; w < imageWidth; w++)
                {
                    Rgba32 pixel = image[w, h];
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;
                    byte avg = (byte)((r + g + b) / 3);
                    tmpImage[w, h] = new Rgba32(avg, avg, avg);
                }
            }

            grayscaleImages.Add(tmpImage);
            return grayscaleImages;
        }

        private static List<Image<Rgba32>> ConvertImageFromMinimunAlogorithm(Image<Rgba32> image)
        {
            List<Image<Rgba32>> grayscaleImages = new List<Image<Rgba32>>();
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            Image<Rgba32> tmpImage = new Image<Rgba32>(imageWidth, imageHeight);

            for (int h = 0; h < imageHeight; h++)
            {
                for (int w = 0; w < imageWidth; w++)
                {
                    Rgba32 pixel = image[w, h];
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;
                    byte min = Math.Min(Math.Min(r, g), b);
                    tmpImage[w, h] = new Rgba32(min, min, min);
                }
            }

            grayscaleImages.Add(tmpImage);
            return grayscaleImages;
        }

        private static List<Image<Rgba32>> ConvertImageFromMaximunAlogorithm(Image<Rgba32> image)
        {
            List<Image<Rgba32>> grayscaleImages = new List<Image<Rgba32>>();
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            Image<Rgba32> tmpImage = new Image<Rgba32>(imageWidth, imageHeight);

            for (int h = 0; h < imageHeight; h++)
            {
                for (int w = 0; w < imageWidth; w++)
                {
                    Rgba32 pixel = image[w, h];
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;
                    byte max = Math.Max(Math.Max(r, g), b);
                    tmpImage[w, h] = new Rgba32(max, max, max);
                }
            }

            grayscaleImages.Add(tmpImage);
            return grayscaleImages;
        }

        private static List<Image<Rgba32>> ConvertImageFromComponentAlgorithm(Image<Rgba32> image)
        {
            List<Image<Rgba32>> grayscaleImages = new List<Image<Rgba32>>();
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            Image<Rgba32> rComponentImage = new Image<Rgba32>(imageWidth, imageHeight);
            Image<Rgba32> gComponentImage = new Image<Rgba32>(imageWidth, imageHeight);
            Image<Rgba32> bComponentImage = new Image<Rgba32>(imageWidth, imageHeight);

            for (int h = 0; h < imageHeight; h++)
            {
                for (int w = 0; w < imageWidth; w++)
                {
                    Rgba32 pixel = image[w, h];
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;
                    rComponentImage[w, h] = new Rgba32(r, r, r);
                    gComponentImage[w, h] = new Rgba32(g, g, g);
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
