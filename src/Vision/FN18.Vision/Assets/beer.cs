using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// 00182f0f-2a9a-4618-813c-aa3a8b72251d_139e98f4-4434-4918-a121-accd0a88287b

namespace FN18.Vision
{
    public sealed class _x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModelInput
    {
        public VideoFrame data { get; set; }
    }

    public sealed class _x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModelOutput
    {
        public IList<string> classLabel { get; set; }
        public IDictionary<string, float> loss { get; set; }
        public _x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModelOutput()
        {
            this.classLabel = new List<string>();
            this.loss = new Dictionary<string, float>()
            {
                { "beer", float.NaN },
                { "notbeer", float.NaN },
            };
        }
    }

    public sealed class _x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModel
    {
        private LearningModelPreview learningModel;
        public static async Task<_x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModel> Create_x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModel(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            _x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModel model = new _x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModel();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<_x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModelOutput> EvaluateAsync(_x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModelInput input) {
            _x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModelOutput output = new _x0030_0182f0f_x002D_2a9a_x002D_4618_x002D_813c_x002D_aa3a8b72251d_139e98f4_x002D_4434_x002D_4918_x002D_a121_x002D_accd0a88287bModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data", input.data);
            binding.Bind("classLabel", output.classLabel);
            binding.Bind("loss", output.loss);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
