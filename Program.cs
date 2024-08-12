using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;

class Program
{
    static void Main(string[] args)
    {
        while(true)
        {
            Console.WriteLine("Enter the paths and parameters in one line (separated by spaces):");
            Console.WriteLine("Format: <sourceFolder> <destinationFolder> <formatOption> <width> <height>");
            string inputLine = Console.ReadLine();

            string[] inputArgs = inputLine.Split(' ');

            if (inputArgs.Length != 5)
            {
                Console.WriteLine("Invalid number of arguments. Please provide exactly 5 arguments.");
                return;
            }

            string sourceFolder = inputArgs[0];
            string destinationFolder = inputArgs[1];
            int formatOption = int.Parse(inputArgs[2]);
            int width = int.Parse(inputArgs[3]);
            int height = int.Parse(inputArgs[4]);

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            string[] imageFiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);

            foreach (string imageFile in imageFiles)
            {
                using (Image image = Image.Load(imageFile))
                {
                    int cropX = (image.Width - width) / 2;
                    int cropY = (image.Height - height) / 2;

                    image.Mutate(x => x.Crop(new Rectangle(cropX, cropY, width, height)));

                    string outputFilePath = Path.Combine(destinationFolder, Path.GetFileName(imageFile));

                    switch (formatOption)
                    {
                        case 0:
                            outputFilePath = Path.ChangeExtension(outputFilePath, Path.GetExtension(imageFile));
                            image.Save(outputFilePath);
                            break;
                        case 1:
                            outputFilePath = Path.ChangeExtension(outputFilePath, "png");
                            image.Save(outputFilePath, new PngEncoder());
                            break;
                        case 2:
                            outputFilePath = Path.ChangeExtension(outputFilePath, "jpg");
                            image.Save(outputFilePath, new JpegEncoder());
                            break;
                        case 3:
                            outputFilePath = Path.ChangeExtension(outputFilePath, "webp");
                            image.Save(outputFilePath, new WebpEncoder());
                            break;
                        default:
                            Console.WriteLine("Invalid format option. Keeping original format.");
                            outputFilePath = Path.ChangeExtension(outputFilePath, Path.GetExtension(imageFile));
                            image.Save(outputFilePath);
                            break;
                    }

                    Console.WriteLine($"Processed and saved: {outputFilePath}");
                }
            }

            Console.WriteLine("All images have been processed.");
        }  
    }
}