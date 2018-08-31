using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediaServices.Demo.Function
{
    public class VideoInfo
    {
        static string FfProbeLocation = @".\util\ffprobe.exe";
        static string _workingFolder = @".\util";



        public Dictionary<string, string> BlobVideoInfo(string blobUri, ILogger log)
        {
            Dictionary<string, string> blobVideoInfo = new Dictionary<string, string>();

            var meta = GetMetadata(blobUri, log);
            //log.LogInformation(meta);

            return blobVideoInfo;
        }

        public static async Task<string> GetMetadata(string blobUri, ILogger log)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = @".\util\ffprobe.exe";
                process.StartInfo.Arguments = $"-v quiet -show_format -print_format json \"{blobUri}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string err = process.StandardError.ReadToEnd();
                ///
                //process.OutputDataReceived += (sender, line) =>
                //{
                //    if (line.Data != null)
                //    {
                //        output.Append(line.Data);
                //        Console.WriteLine(line.Data);
                //    }
                //};

                //process.BeginOutputReadLine();

                process.WaitForExit();

                log.LogInformation(output);
                //log.LogInformation(err);

                return output;
            }
            catch (Exception ex)
            {
                log.LogCritical($"Error extracting metadata from Video for {blobUri}, Exception: {ex.Message}");
                throw;
            }
        }

        static void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)

        {

            Console.WriteLine("Output data: {0}", e.Data);

        }

    }
}
