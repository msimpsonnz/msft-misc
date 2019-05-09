using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace MediaServices.Demo.Function
{
    public class FFMpeg
    {
        public static string RunFFMpeg(string workingDir, string cmdPath, string args, string correlationId, ILogger log)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.WorkingDirectory = workingDir;
                process.StartInfo.FileName = cmdPath;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string err = process.StandardError.ReadToEnd();

                process.WaitForExit();

                log.LogInformation($"Status: FFMpeg success, CorrelationId: {correlationId}");

                return output;
            }
            catch (Exception ex)
            {
                log.LogError($"Status: Error running FFMpeg {correlationId}, Exception: {ex.Message}, CorrelationId: {correlationId}");
                throw;
            }
        }

    }
}
