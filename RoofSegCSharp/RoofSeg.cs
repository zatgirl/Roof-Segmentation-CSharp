using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using MPI;

namespace RoofSegCSharp
{
    class RoofSeg
    {
        private SessionOptions sessionOptions;
        private InferenceSession session;
        private const int inputHeight = 256;
        private const int inputWidth = 256;
        private const int inputChannels = 3;
        private float[] inputData;
        private float[] outputData;

        /// <summary>
        /// 
        /// </summary>
        private void ReadModel(string modelPath)
        {
            sessionOptions = new SessionOptions();
            session = new InferenceSession(modelPath, sessionOptions);
        }

        private void InputUnet()
        {
            int[] tensorShape = new int[] { 1, inputChannels, inputHeight, inputWidth };
            inputData = new float[tensorShape.Aggregate((a, b) => a * b)];
            DenseTensor<float> tensor = new DenseTensor<float>(inputData, tensorShape);
            Inference(tensor);
        }

        private void Inference(DenseTensor<float> tensor)
        {
            var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input", tensor)
        };

            var results = session.Run(inputs);
            var outputTensor = results.First().AsTensor<float>();
            outputData = outputTensor.ToArray();
        }

        private void ReadImage()
        {

        }

        public void Main()
        {
            ReadModel("C:\\Users\\maure\\Downloads\\drive-download-20230612T185242Z-001\\unet-256-model-150epochs\\modelo.onnx");
            InputUnet();
        }
    }
}