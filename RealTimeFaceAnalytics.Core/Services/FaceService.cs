using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Models;
using RealTimeFaceAnalytics.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoFrameAnalyzer;

namespace RealTimeFaceAnalytics.Core.Services
{
    public class FaceService : IFaceService
    {
        private readonly List<FaceAttributeType> _faceAttributes;
        private FaceServiceClient _faceServiceClient;
        private int _faceAPICallCount = 0;
        private List<double> _ageArray = new List<double>();

        public FaceService()
        {
            InitializeFaceServiceClient();
            _faceAttributes = new List<FaceAttributeType>();
            InitializeAllFaceAttributes();//InitializeDefaultFaceAttributes();
        }

        public FaceServiceClient GetFaceServiceClient()
        {
            return _faceServiceClient;
        }

        public void InitializeFaceServiceClient()
        {
            InitializeFaceAPIClient();
        }

        public Face[] DetectFaces(MemoryStream imageStream, IEnumerable<FaceAttributeType> faceAttributeTypes = null)
        {
            return DetectFacesFromImage(imageStream).Result;
        }

        public Face[] DetectFaces(string imagePath, IEnumerable<FaceAttributeType> faceAttriubuteTypes = null)
        {
            return DetectFacesFromImage(imagePath).Result;
        }

        public Face[] DetectFacesWithDefaultAttributes(MemoryStream imageStream)
        {
            return DetectFacesFromImage(imageStream, _faceAttributes).Result;
        }

        public Face[] DetectFacesWithDefaultAttributes(string imagePath)
        {
            return DetectFacesFromImage(imagePath, _faceAttributes).Result;
        }

        public int GetFaceServiceClientAPICallCount()
        {
            return _faceAPICallCount;
        }

        public async Task<LiveCameraResult> FacesAnalysisFunction(VideoFrame frame)
        {
            return await SubmitFacesAnalysisFunction(frame);
        }

        public string SummarizeFaceAttributes(FaceAttributes faceAttributes)
        {
            return SummarizeDefaultFaceAttributes(faceAttributes);
        }

        public void AddAgeToStatistics(double age)
        {
            AddAndCalculateAgeStatistics(age);
        }

        public double CalculateAverageAge()
        {
            return GetAgeStatistics();
        }

        private void InitializeFaceAPIClient()
        {
            var faceServiceClientSubscriptionKey = Properties.Settings.Default.FaceAPIKey.Trim();
            var faceServiceClientApiRoot = Properties.Settings.Default.FaceAPIHost;
            _faceServiceClient = new FaceServiceClient(faceServiceClientSubscriptionKey, faceServiceClientApiRoot);
        }
        private async Task<LiveCameraResult> SubmitFacesAnalysisFunction(VideoFrame frame)
        {
            var result = new LiveCameraResult();

            var frameImage = frame.Image.ToMemoryStream(".jpg", ImageEncodingParameter.JpegParams); ;
            var faces = await DetectFacesFromImage(frameImage, _faceAttributes);
            result.Faces = faces;

            return result;
        }
        private async Task<Face[]> DetectFacesFromImage(dynamic image, IEnumerable<FaceAttributeType> faceAttributeTypes = null)
        {
            var result = await _faceServiceClient.DetectAsync(image, true, false, faceAttributeTypes);

            _faceAPICallCount++;

            return result;
        }
        private void InitializeDefaultFaceAttributes()
        {
            _faceAttributes.Add(FaceAttributeType.Age);
            _faceAttributes.Add(FaceAttributeType.Gender);
            _faceAttributes.Add(FaceAttributeType.HeadPose);
        }
        private void InitializeAllFaceAttributes()
        {
            InitializeDefaultFaceAttributes();
            _faceAttributes.Add(FaceAttributeType.Accessories);
            _faceAttributes.Add(FaceAttributeType.Blur);
            _faceAttributes.Add(FaceAttributeType.Emotion);
            _faceAttributes.Add(FaceAttributeType.Exposure);
            _faceAttributes.Add(FaceAttributeType.FacialHair);
            _faceAttributes.Add(FaceAttributeType.Glasses);
            _faceAttributes.Add(FaceAttributeType.Hair);
            _faceAttributes.Add(FaceAttributeType.Makeup);
            _faceAttributes.Add(FaceAttributeType.Noise);
            _faceAttributes.Add(FaceAttributeType.Occlusion);
            _faceAttributes.Add(FaceAttributeType.Smile);
        }
        private string SummarizeDefaultFaceAttributes(FaceAttributes faceAttributes)
        {
            var result = string.Empty;

            var attributes = new List<string>();
            if (faceAttributes.Gender != null) attributes.Add(faceAttributes.Gender);
            if (faceAttributes.Age > 0) attributes.Add(faceAttributes.Age.ToString());
            if (faceAttributes.HeadPose != null)
            {
                bool facing = Math.Abs(faceAttributes.HeadPose.Yaw) < 25;
                attributes.Add(facing ? "facing camera" : "not facing camera");
            }
            result = string.Join(", ", attributes);

            return result;
        }
        private void AddAndCalculateAgeStatistics(double age)
        {
            _ageArray.Add(age);
        }
        private double GetAgeStatistics()
        {
            var result = 0.0;

            result = _ageArray.Average();

            return result;
        }
    }
}
