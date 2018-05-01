using System;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;

namespace RealTimeFaceAnalytics.Core.Interfaces
{
    public interface IDataInsertionService
    {
        void InitializeSession(TimeSpan analysisInterval);
        void InitializeSessionInterval();
        void AddSessionIntervalData();
        void InsertSessionData();
        void AddAge(double age);
        void AddAverageAge(double averageAge);
        void AddGender(string gender);
        void AddAverageGender(string averageGender);
        void AddEmotions(EmotionScores emotionScores);
        void AddAverageEmotions(EmotionScores averageEmotions);
        void AddHair(Hair hair);
        void AddFacialHair(FacialHair facialHair);
        void AddAdditionalFeatures(FaceAttributes additionalFeatures);
        void AddFaceApiCallCount(int faceApiCallCount);
        void AddSessionDuration(TimeSpan sessionDuration);
    }
}