using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace MediaServices.Demo.Function
{
    public class FFProcess
    {
        private static readonly string ffmpegLocation = Environment.GetEnvironmentVariable("ffmpegLocation");
        public static string CreateSprite(string workingDir, string blobPath, string outputName, string correlationId, ILogger log)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.WorkingDirectory = workingDir;
                process.StartInfo.FileName = ffmpegLocation;
                process.StartInfo.Arguments = $"-v quiet -r 1/5 -i {blobPath}%06d.png -c:v libx264 -vf fps=25 -pix_fmt yuv420p {outputName}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string err = process.StandardError.ReadToEnd();

                process.WaitForExit();

                log.LogInformation($"Status: Extracted metadata, CorrelationId: {correlationId}");

                return outputName;
            }
            catch (Exception ex)
            {
                log.LogError($"Status: Error creating sprite {correlationId}, Exception: {ex.Message}, CorrelationId: {correlationId}");
                throw;
            }
        }

    }
}
