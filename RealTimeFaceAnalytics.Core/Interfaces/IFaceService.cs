using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeFaceAnalytics.Core.Models;
using VideoFrameAnalyzer;

namespace RealTimeFaceAnalytics.Core.Interfaces
{
    public interface IFaceService
    {
        FaceServiceClient GetFaceServiceClient();
        void InitializeFaceServiceClient();
        Face[] DetectFaces(MemoryStream imageStream, IEnumerable<FaceAttributeType> faceAttributeTypes = null);
        Face[] DetectFaces(string imagePath, IEnumerable<FaceAttributeType> faceAttriubuteTypes = null);
        Face[] DetectFacesWithDefaultAttributes(MemoryStream imageStream);
        Face[] DetectFacesWithDefaultAttributes(string imagePath);
        int GetFaceServiceClientApiCallCount();
        void ResetFaceServiceLocalData();
        Task<LiveCameraResult> FacesAnalysisFunction(VideoFrame frame);
        string SummarizeFaceAttributes(FaceAttributes faceAttributes);
        void AddAgeToStatistics(double age);
        void AddGenderToStatistics(string gender);
        double CalculateAverageAge();
        string CalculateAverageGender();
    }
}