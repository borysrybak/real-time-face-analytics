using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Models;
using RealTimeFaceAnalytics.Core.Properties;
using RealTimeFaceAnalytics.Core.Utils;
using VideoFrameAnalyzer;

namespace RealTimeFaceAnalytics.Core.Services
{
    public class ComputerVisionService : IComputerVisionService
    {
        #region Fields

        private readonly VisionServiceClient _visionServiceClient;
        private readonly string _visionServiceClientApiRoot = Settings.Default.VisionAPIHost;
        private readonly string _visionServiceClientSubscriptionKey = Settings.Default.VisionAPIKey.Trim();

        private int _visionApiCallCount;

        #endregion Fields

        public ComputerVisionService()
        {
            _visionServiceClient =
                new VisionServiceClient(_visionServiceClientSubscriptionKey, _visionServiceClientApiRoot);
        }

        #region Methods

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

        public int GetVisionServiceClientApiCallCount()
        {
            return _visionApiCallCount;
        }

        public async Task<LiveCameraResult> VisionAnalysisFunction(VideoFrame frame)
        {
            return await SubmitVisionAnalysisFunction(frame);
        }

        #endregion Methods

        #region Private Methods

        private async Task<LiveCameraResult> SubmitVisionAnalysisFunction(VideoFrame frame)
        {
            var result = new LiveCameraResult();

            var frameImage = frame.Image.ToMemoryStream(".jpg", ImageEncodingParameter.JpegParams);
            var tags = await AnalyzeImageBySpecificVisualFeatures(frameImage, VisualFeature.Tags);
            result.Tags = tags;

            return result;
        }

        private async Task<Tag[]> AnalyzeImageBySpecificVisualFeatures(dynamic image,
            params VisualFeature[] visualFeatures)
        {
            var features = visualFeatures.Select(feature => Enum.GetName(typeof(VisualFeature), feature)).ToArray();

            AnalysisResult analysisResult = await _visionServiceClient.AnalyzeImageAsync(image, features);
            var result = analysisResult.Tags;

            _visionApiCallCount++;

            return result;
        }

        #endregion Private Methods
    }
}