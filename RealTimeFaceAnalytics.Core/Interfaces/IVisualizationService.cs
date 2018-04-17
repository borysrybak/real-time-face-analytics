using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Vision.Contract;
using RealTimeFaceAnalytics.Core.Models;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using VideoFrameAnalyzer;

namespace RealTimeFaceAnalytics.Core.Interfaces
{
    public interface IVisualizationService
    {
        BitmapSource DrawTags(BitmapSource baseImage, Tag[] tags);
        BitmapSource DrawFaces(BitmapSource baseImage, Microsoft.ProjectOxford.Face.Contract.Face[] faces, EmotionScores[] emotionScores, string[] celebName);
        BitmapSource Visualize(VideoFrame videoFrame, LiveCameraResult currentLiveCameraResult);
        System.Windows.Shapes.Rectangle ComposeRectangleBar(EmotionScores emotionScores);
        List<System.Windows.Shapes.Rectangle> MixHairColor(HairColor[] hairColors);
    }
}
