using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealTimeFaceAnalytics.Core.Interfaces
{
    public interface IVideoFrameAnalyzerService
    {
        List<string> GetAvailableCameraList();
        void InitializeFrameGrabber();
        void StartProcessing(string selectedCamera);
        Task StopProcessing();
    }
}