using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Models;
using RealTimeFaceAnalytics.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VideoFrameAnalyzer;

namespace RealTimeFaceAnalytics.Core.Services
{
    public class ComputerVisionService : IComputerVisionService
    {
        private readonly string _visionServiceClientSubscriptionKey = Properties.Settings.Default.VisionAPIKey.Trim();
        private readonly string _visionServiceClientApiRoot = Properties.Settings.Default.VisionAPIHost;
        private readonly VisionServiceClient _visionServiceClient;

        private int _visionAPICallCount = 0;

        public ComputerVisionService()
        {
            _visionServiceClient = new VisionServiceClient(_visionServiceClientSubscriptionKey, _visionServiceClientApiRoot);
        }

        public VisionServiceClient GetVisionServiceClient()
        {
            return _visionServiceClient;
        }

        public Tag[] GetTags(MemoryStream imageStream)
        {
            return AnalyzeImageBySpecificVisualFeatures(imageStream, VisualFeature.Tags).Result;
        }

        public Tag[] GetTags(string imagePath)
        {
            return AnalyzeImageBySpecificVisualFeatures(imagePath, VisualFeature.Tags).Result;
        }

        public int GetVisionServiceClientAPICallCount()
        {
            return _visionAPICallCount;
        }

        public async Task<LiveCameraResult> VisionAnalysisFunction(VideoFrame frame)
        {
            return await SubmitVisionAnalysisFunction(frame);
        }

        private async Task<LiveCameraResult> SubmitVisionAnalysisFunction(VideoFrame frame)
        {
            var result = new LiveCameraResult();

            var frameImage = frame.Image.ToMemoryStream(".jpg", ImageEncodingParameter.JpegParams); ;
            var tags = await AnalyzeImageBySpecificVisualFeatures(frameImage, VisualFeature.Tags);
            result.Tags = tags;

            return result;
        }
        private async Task<Tag[]> AnalyzeImageBySpecificVisualFeatures(dynamic image, params VisualFeature[] visualFeatures)
        {
            var result = new Tag[0];
            var analysisResult = new AnalysisResult();
            var visualFeaturesLength = visualFeatures.Length;
            var featuresList = new List<string>();
            foreach (var feature in visualFeatures)
            {
                var featureAsString = Enum.GetName(typeof(VisualFeature), feature);
                featuresList.Add(featureAsString);
            }
            var features = featuresList.ToArray();

            analysisResult = await _visionServiceClient.AnalyzeImageAsync(image, features);
            result = analysisResult.Tags;

            _visionAPICallCount++;

            return result;
        }
    }
}
