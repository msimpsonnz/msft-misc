using Microsoft.ML.Core.Data;
using Microsoft.ML.Transforms;
using Microsoft.ML.Runtime;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Runtime.ImageAnalytics;
using Microsoft.ML.Runtime.Model;
using Microsoft.ML.Runtime.Tools;
using Microsoft.ML.Legacy.Trainers;
using Microsoft.ML.Legacy.Transforms;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Transforms.TensorFlow;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FN18.Onnx
{
    class Program
    {
        static void Main(string[] args)
        {
            //MainAsync().GetAwaiter().GetResult();
            LoadModel();
        }

        static async Task MainAsync()
        {
            //await LoadModelAsync();
        }

        public static void TensorLegacy()
        {
            var modelFile = "./Assets/model.pb";

            var imageHeight = 227;
            var imageWidth = 227;
            var dataFile = "./Assets/images.tsv";
            var imageFolder = Path.GetDirectoryName(dataFile);

            var pipeline = new Microsoft.ML.Legacy.LearningPipeline();
            pipeline.Add(new Microsoft.ML.Legacy.Data.TextLoader(dataFile).CreateFrom<ModelData>(useHeader: false));
            pipeline.Add(new ImageLoader(("ImagePath", "ImageReal"))
            {
                ImageFolder = imageFolder
            });

            pipeline.Add(new ImageResizer(("ImageReal", "ImageCropped"))
            {
                ImageHeight = imageHeight,
                ImageWidth = imageWidth,
                Resizing = ImageResizerTransformResizingKind.IsoCrop
            });

            pipeline.Add(new ImagePixelExtractor(("ImageCropped", "Input"))
            {
                UseAlpha = false,
                InterleaveArgb = true
            });

            pipeline.Add(new TensorFlowScorer()
            {
                Model = modelFile,
                InputColumns = new[] { "Placeholder" },
                OutputColumns = new[] { "loss" }
            });

            pipeline.Add(new ColumnConcatenator(outputColumn: "Features", "Output"));
            pipeline.Add(new TextToKeyConverter("Label"));
            pipeline.Add(new StochasticDualCoordinateAscentClassifier());

            var model = pipeline.Train<ModelData, Prediction>();
            string[] scoreLabels;
            model.TryGetScoreLabelNames(out scoreLabels);

        }


        public static void Tensor()
        {
            var modelFile = "./Assets/model.pb";

            using (var env = new ConsoleEnvironment(null, true, 0, 1, null, null))
            {
                var imageHeight = 227;
                var imageWidth = 227;
                var dataFile = "./Assets/images.tsv";
                var imageFolder = Path.GetDirectoryName(dataFile);

                var data = TextLoader.CreateReader(env, ctx => (
                    imagePath: ctx.LoadText(0),
                    name: ctx.LoadText(1)))
                    .Read(new MultiFileSource(dataFile));

                var pipe = data.MakeNewEstimator()
                    .Append
                    (row => (
                        row,
                        Placeholder: row.imagePath.LoadAsImage(imageFolder).Resize(imageHeight, imageWidth).ExtractPixels(interleaveArgb: true)))
                    .Append(row => (row, loss: row.Placeholder.ApplyTensorFlowGraph(modelFile)));

                var result = pipe.Fit(data).Transform(data).AsDynamic;

            }

        }

        private static void LoadModel()
        {
            // Load and create the model
            var modelFile = "./Assets/beer.onnx";

            using (var env = new ConsoleEnvironment(null, true, 0, 1, null, null))
            {
                var imageHeight = 227;
                var imageWidth = 227;
                var dataFile = "./Assets/images.tsv";
                var imageFolder = Path.GetDirectoryName(dataFile);

                var data = TextLoader.CreateReader(env, ctx => (
                    imagePath: ctx.LoadText(0),
                    name: ctx.LoadText(1)))
                    .Read(new MultiFileSource(dataFile));

                var pipe = data.MakeNewEstimator()
                    .Append
                    (row => (
                        row,
                        Placeholder: row.imagePath.LoadAsImage(imageFolder).Resize(imageHeight, imageWidth).ExtractPixels(interleaveArgb: true)))
                    .Append(row => (row, loss: row.Placeholder.ApplyOnnxModel(modelFile)));

                var result = pipe.Fit(data).Transform(data).AsDynamic;
                result.Schema.TryGetColumnIndex("loss", out int output);
                System.Console.WriteLine(output);
                System.Console.WriteLine();

            }
        }


        public class ModelData
        {
            [Column("0")]
            public string ImagePath;

            [Column("1")]
            public string Label;
        }

        public class Prediction
        {
            [ColumnName("Score")]
            public float[] PredictedLabels;
        }
    }
}
