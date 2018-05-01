using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Vision.Contract;
using RealTimeFaceAnalytics.Core.Models;
using VideoFrameAnalyzer;
using Face = Microsoft.ProjectOxford.Face.Contract.Face;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace RealTimeFaceAnalytics.Core.Interfaces
{
    public interface IVisualizationService
    {
        BitmapSource DrawTags(BitmapSource baseImage, Tag[] tags);
        BitmapSource DrawFaces(BitmapSource baseImage, Face[] faces, EmotionScores[] emotionScores, string[] celebName);
        BitmapSource Visualize(VideoFrame videoFrame, LiveCameraResult currentLiveCameraResult);
        Rectangle ComposeRectangleBar(EmotionScores emotionScores);
        List<Rectangle> MixHairColor(HairColor[] hairColors);
    }
}