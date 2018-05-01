using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Models;
using RealTimeFaceAnalytics.Core.Properties;
using RealTimeFaceAnalytics.Core.Utils;
using VideoFrameAnalyzer;

namespace RealTimeFaceAnalytics.Core.Services
{
    public class FaceService : IFaceService
    {
        private readonly List<FaceAttributeType> _faceAttributes;
        private List<double> _ageArray = new List<double>();
        private int _faceApiCallCount;
        private FaceServiceClient _faceServiceClient;
        private List<string> _genderArray = new List<string>();

        public FaceService()
        {
            InitializeFaceServiceClient();
            _faceAttributes = new List<FaceAttributeType>();
            InitializeAllFaceAttributes(); //InitializeDefaultFaceAttributes();
        }

        public FaceServiceClient GetFaceServiceClient()
        {
            return _faceServiceClient;
        }

        public void InitializeFaceServiceClient()
        {
            InitializeFaceApiClient();
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

        public int GetFaceServiceClientApiCallCount()
        {
            return _faceApiCallCount;
        }

        public void ResetFaceServiceLocalData()
        {
            ResetLocalVariables();
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

        public void AddGenderToStatistics(string gender)
        {
            AddAndCalculateGenderStatistics(gender);
        }

        public double CalculateAverageAge()
        {
            return GetAgeStatistics();
        }

        public string CalculateAverageGender()
        {
            return GetGenderStatistics();
        }

        private void InitializeFaceApiClient()
        {
            var faceServiceClientSubscriptionKey = Settings.Default.FaceAPIKey.Trim();
            var faceServiceClientApiRoot = Settings.Default.FaceAPIHost;
            _faceServiceClient = new FaceServiceClient(faceServiceClientSubscriptionKey, faceServiceClientApiRoot);
        }

        private async Task<LiveCameraResult> SubmitFacesAnalysisFunction(VideoFrame frame)
        {
            var result = new LiveCameraResult();

            var frameImage = frame.Image.ToMemoryStream(".jpg", ImageEncodingParameter.JpegParams);
            var faces = await DetectFacesFromImage(frameImage, _faceAttributes);
            result.Faces = faces;

            return result;
        }

        private async Task<Face[]> DetectFacesFromImage(dynamic image,
            IEnumerable<FaceAttributeType> faceAttributeTypes = null)
        {
            var result = await _faceServiceClient.DetectAsync(image, true, false, faceAttributeTypes);

            _faceApiCallCount++;

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

        private static string SummarizeDefaultFaceAttributes(FaceAttributes faceAttributes)
        {
            var attributes = new List<string>();
            if (faceAttributes.Gender != null) attributes.Add(faceAttributes.Gender);
            if (faceAttributes.Age > 0) attributes.Add(faceAttributes.Age.ToString(CultureInfo.InvariantCulture));
            if (faceAttributes.HeadPose != null)
            {
                var facing = Math.Abs(faceAttributes.HeadPose.Yaw) < 25;
                attributes.Add(facing ? "facing camera" : "not facing camera");
            }

            var result = string.Join(", ", attributes);

            return result;
        }

        private void AddAndCalculateAgeStatistics(double age)
        {
            _ageArray.Add(age);
        }

        private void AddAndCalculateGenderStatistics(string gender)
        {
            _genderArray.Add(gender);
        }

        private double GetAgeStatistics()
        {
            var result = _ageArray.Average();

            return result;
        }

        private string GetGenderStatistics()
        {
            var result = _genderArray.GroupBy(s => s)
                .OrderByDescending(s => s.Count())
                .First().Key;

            return result;
        }

        private void ResetLocalVariables()
        {
            _faceApiCallCount = 0;
            _ageArray = new List<double>();
            _genderArray = new List<string>();
        }
    }
}