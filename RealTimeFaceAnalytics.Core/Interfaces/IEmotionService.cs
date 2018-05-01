using System.IO;
using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;

namespace RealTimeFaceAnalytics.Core.Interfaces
{
    public interface IEmotionService
    {
        EmotionServiceClient GetEmotionServiceClient();
        Emotion[] RecognizeEmotions(MemoryStream imageStream);
        Emotion[] RecognizeEmotions(string imagePath);
        Emotion[] RecognizeEmotionsWithLocalFaceDetections(MemoryStream memoryStream, Rectangle[] faceRectangles);
        Emotion[] RecognizeEmotionsWithLocalFaceDetections(string imagePath, Rectangle[] faceRectangles);
        int GetEmotionServiceClientApiCallCount();
        string SummarizeEmotionScores(EmotionScores emotionScores);
        void AddEmotionScoresToStatistics(EmotionScores emotionScores);
        EmotionScores CalculateEmotionScoresStatistics();
        void ResetEmotionServiceLocalData();
    }
}