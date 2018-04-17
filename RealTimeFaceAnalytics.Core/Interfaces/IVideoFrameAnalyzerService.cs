using System.Collections.Generic;

namespace RealTimeFaceAnalytics.Core.Interfaces
{
    public interface IVideoFrameAnalyzerService
    {
        List<string> GetAvailableCameraList();
        void InitializeFrameGrabber();
        void StartProcessing(string selectedCamera);
        void StopProcessing();
    }
}
