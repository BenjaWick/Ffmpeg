using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        
        string inputFolder = "/home/benja/Documentos/Mp3";

        
        string outputFolder = "/home/benja/Documentos/Resultantes/";

        
        string processedFile = "processed.txt";

        string ffmpegPath = "/usr/bin/ffmpeg";

        
        Directory.CreateDirectory(inputFolder);
        Directory.CreateDirectory(outputFolder);

        
        if (!File.Exists(processedFile))
        {
            File.Create(processedFile).Close();
        }

        
        string[] mp3Files = Directory.GetFiles(inputFolder, "*.mp3");

        foreach (string mp3File in mp3Files)
        {
            
            string processedFiles = File.ReadAllText(processedFile);
            if (processedFiles.Contains(mp3File))
            {
                Console.WriteLine($"El archivo {mp3File} ya se ha procesado previamente.");
                continue;
            }

            
            string outputFileName = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(mp3File) + ".mp4");

           
            string ffmpegCommand = $"\"{ffmpegPath}\" -i \"{mp3File}\" -filter_complex \"[0:a]showwaves=s=640x360:mode=line,format=yuv420p[v]\" -map \"[v]\" -map 0:a -c:v libx265 -crf 28 -preset veryfast -c:a copy \"{outputFileName}\"";

            
            var processStartInfo = new ProcessStartInfo("bash", $"-c \"{ffmpegCommand}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit();

            Console.WriteLine($"Proceso completo para {mp3File}. Resultado guardado en {outputFileName}");
       
            File.AppendAllText(processedFile, mp3File + Environment.NewLine);
        }

        Console.WriteLine("Todos los archivos han sido convertidos y guardados en la carpeta de resultados.");
    }
}


