using Microsoft.ProjectOxford.Face.Contract;
using OpenCvSharp;

namespace RealTimeFaceAnalytics.Core.Interfaces
{
    public interface IOpenCvService
    {
        CascadeClassifier DefaultFrontalFaceDetector();
        void MatchAndReplaceFaces(Face[] faces, Rect[] clientRects);
    }
}
