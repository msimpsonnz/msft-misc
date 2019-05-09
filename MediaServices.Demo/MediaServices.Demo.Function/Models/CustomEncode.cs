using Microsoft.Azure.Management.Media.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaServices.Demo.Function
{
    class CustomEncode
    {
        public static TransformOutput[] CustomEncode_HD_SD_Thumb()
        {
            TransformOutput[] outputs = new TransformOutput[]
                {
                    new TransformOutput(
                        new StandardEncoderPreset(
                            codecs: new Codec[]
                            {
                                // Add an AAC Audio layer for the audio encoding
                                //new AacAudio(
                                //    channels: 2,
                                //    samplingRate: 48000,
                                //    bitrate: 128000,
                                //    profile: AacAudioProfile.AacLc
                                //),
                                // Next, add a H264Video for the video encoding
                               new H264Video (
                                    // Set the GOP interval to 2 seconds for both H264Layers
                                    keyFrameInterval:TimeSpan.FromSeconds(2),
                                     // Add H264Layers, one at HD and the other at SD. Assign a label that you can use for the output filename
                                    layers:  new H264Layer[]
                                    {
                                        new H264Layer (
                                            bitrate: 1500000, // Note that the units is in bits per second
                                            width: "1920",
                                            height: "1080",
                                            label: "HD-1080" // This label is used to modify the file name in the output formats
                                        ),
                                        new H264Layer (
                                            bitrate: 1000000, // Note that the units is in bits per second
                                            width: "1280",
                                            height: "720",
                                            label: "HD-720" // This label is used to modify the file name in the output formats
                                        ),
                                        new H264Layer (
                                            bitrate: 600000,
                                            width: "960",
                                            height: "540",
                                            label: "SD-540"
                                        )
                                    }
                                ),
                                // Also generate a set of PNG thumbnails
                                new JpgImage(
                                    start: "25%",
                                    step: "25%",
                                    range: "80%",
                                    layers: new JpgLayer[]{
                                        new JpgLayer(
                                            width: "50%",
                                            height: "50%"
                                        )
                                    }
                                )
                            },
                            // Specify the format for the output files - one for video+audio, and another for the thumbnails
                            formats: new Format[]
                            {
                                // Mux the H.264 video and AAC audio into MP4 files, using basename, label, bitrate and extension macros
                                // Note that since you have multiple H264Layers defined above, you have to use a macro that produces unique names per H264Layer
                                // Either {Label} or {Bitrate} should suffice
                                 
                                new Mp4Format(
                                    filenamePattern:"Video-{Basename}-{Label}-{Bitrate}{Extension}"
                                ),
                                new JpgFormat(
                                    filenamePattern:"Thumbnail-{Basename}-{Index}{Extension}"
                                )
                            }
                        ),
                        onError: OnErrorType.StopProcessingJob,
                        relativePriority: Priority.Normal
                    )
                };
        
            return outputs;
        }
    }
}
