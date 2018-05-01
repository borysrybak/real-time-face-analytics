using System.IO;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace RealTimeFaceAnalytics.Core.Interfaces
{
    public interface IComputerVisionService
    {
        VisionServiceClient GetVisionServiceClient();
        Tag[] GetTags(MemoryStream imageStream);
        Tag[] GetTags(string imagePath);
        int GetVisionServiceClientApiCallCount();
    }
}