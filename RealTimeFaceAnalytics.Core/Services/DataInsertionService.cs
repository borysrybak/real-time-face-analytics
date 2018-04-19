using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RealTimeFaceAnalytics.Core.Services
{
    public class DataInsertionService : IDataInsertionService
    {
        private Session _session;
        private SessionInterval _sessionInterval;
        private Customer _customer;
        private List<SessionInterval> _sessionIntervals;

        public void InitializeSession(TimeSpan analysisInterval)
        {
            InitializeSessionVariables(analysisInterval);
        }

        public void InitializeSessionInterval()
        {
            InitializeSessionIntervalVariable();
        }

        public void AddSessionIntervalData()
        {
            AddSessionIntervalDataToSession();
        }

        public void InsertSessionData()
        {
            InsertSessionDataToDatabaseContext();
        }

        public void AddAge(double age)
        {
            AddAgeToSessionInterval(age);
        }

        public void AddAverageAge(double averageAge)
        {
            AddAverageAgeToCustomer(averageAge);
        }

        public void AddGender(string gender)
        {
            AddGenderToSessionInterval(gender);
        }

        public void AddAverageGender(string averageGender)
        {
            AddAverageGenderToCustomer(averageGender);
        }

        public void AddEmotions(EmotionScores emotionScores)
        {
            AddEmotionsToSessionInterval(emotionScores);
        }

        public void AddAverageEmotions(EmotionScores averageEmotions)
        {
            AddAverageEmotionsToCustomer(averageEmotions);
        }

        public void AddHair(Microsoft.ProjectOxford.Face.Contract.Hair hair)
        {
            AddHairToCustomer(hair);
        }

        public void AddFacialHair(Microsoft.ProjectOxford.Face.Contract.FacialHair facialHair)
        {
            AddFacialHairToCustomer(facialHair);
        }

        public void AddAdditionalFeatures(FaceAttributes additionalFeatures)
        {
            AddAdditionalFeaturesToCustomer(additionalFeatures);
        }

        public void AddFaceApiCallCount(int faceAPICallCount)
        {
            AddFaceApiCallCountToSessionServicesDetails(faceAPICallCount);
        }

        public void AddSessionDuration(TimeSpan sessionDuration)
        {
            AddSessionDurationToSession(sessionDuration);
        }

        private async void InsertSessionDataToDatabaseContext()
        {
            _session.SessionEndTime = DateTime.Now.TimeOfDay;

            using (var databaseContext = new FaceAnalyticsContext())
            {
                databaseContext.Session.Add(_session);
                await databaseContext.SaveChangesAsync();
            }
        }
        private void AddSessionIntervalDataToSession()
        {
            _sessionIntervals.Add(_sessionInterval);
        }
        private void AddAgeToSessionInterval(double age)
        {
            _sessionInterval.Age = age;
        }
        private void AddAverageAgeToCustomer(double averageAge)
        {
            _customer.AverageAge = averageAge;
        }
        private void AddGenderToSessionInterval(string gender)
        {
            _sessionInterval.Gender = gender;
        }
        private void AddAverageGenderToCustomer(string averageGender)
        {
            _customer.AverageGender = averageGender;
        }
        private void AddEmotionsToSessionInterval(EmotionScores emotionScores)
        {
            _sessionInterval.Emotions = new Emotions();
            _sessionInterval.Emotions.Anger = emotionScores.Anger;
            _sessionInterval.Emotions.Happiness = emotionScores.Happiness;
            _sessionInterval.Emotions.Contempt = emotionScores.Contempt;
            _sessionInterval.Emotions.Neutral = emotionScores.Neutral;
            _sessionInterval.Emotions.Disgust = emotionScores.Disgust;
            _sessionInterval.Emotions.Sadness = emotionScores.Sadness;
            _sessionInterval.Emotions.Fear = emotionScores.Fear;
            _sessionInterval.Emotions.Surprise = emotionScores.Surprise;
        }
        private void AddAverageEmotionsToCustomer(EmotionScores averageEmotions)
        {
            _customer.AverageEmotions = new Emotions();
            _customer.AverageEmotions.Anger = averageEmotions.Anger;
            _customer.AverageEmotions.Happiness = averageEmotions.Happiness;
            _customer.AverageEmotions.Contempt = averageEmotions.Contempt;
            _customer.AverageEmotions.Neutral = averageEmotions.Neutral;
            _customer.AverageEmotions.Disgust = averageEmotions.Disgust;
            _customer.AverageEmotions.Sadness = averageEmotions.Sadness;
            _customer.AverageEmotions.Fear = averageEmotions.Fear;
            _customer.AverageEmotions.Surprise = averageEmotions.Surprise;
        }
        private void AddHairToCustomer(Microsoft.ProjectOxford.Face.Contract.Hair hair)
        {
            _customer.Hair = new Models.Hair();
            _customer.Hair.Bald = hair.Bald;
            _customer.Hair.IsInvisible = hair.Invisible;

            var hairColors = hair.HairColor;
            foreach (var hairColor in hairColors)
            {
                if (hairColor.Color == HairColorType.Black) { _customer.Hair.Black = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Blond) { _customer.Hair.Blond = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Brown) { _customer.Hair.Brown = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Gray) { _customer.Hair.Gray = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Other) { _customer.Hair.Other = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Red) { _customer.Hair.Red = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Unknown) { _customer.Hair.Unknown = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.White) { _customer.Hair.White = hairColor.Confidence; }
            }
        }
        private void AddFacialHairToCustomer(Microsoft.ProjectOxford.Face.Contract.FacialHair facialHair)
        {
            _customer.FacialHair = new Models.FacialHair();
            _customer.FacialHair.Moustache = facialHair.Moustache;
            _customer.FacialHair.Beard = facialHair.Beard;
            _customer.FacialHair.Sideburns = facialHair.Sideburns;
        }
        private void AddAdditionalFeaturesToCustomer(FaceAttributes additionalFeatures)
        {
            var glasses = additionalFeatures.Glasses.ToString();
            _customer.AdditionalFeatures = new AdditionalFeatures();
            _customer.AdditionalFeatures.Glasses = glasses;
            _customer.AdditionalFeatures.IsEyeMakeup = additionalFeatures.Makeup.EyeMakeup;
            _customer.AdditionalFeatures.IsLipMakeup = additionalFeatures.Makeup.LipMakeup;

            var accessories = additionalFeatures.Accessories;
            _customer.AdditionalFeatures.Accessories = new Accessories();
            if (accessories.Length <= 0) return;
            foreach (var accessory in accessories)
            {
                var accessoryType = accessory.Type;
                var accessoryConfidence = accessory.Confidence;
                switch (accessoryType)
                {
                    case AccessoryType.Headwear:
                        _customer.AdditionalFeatures.Accessories.HeadwearConfidence = accessoryConfidence;
                        break;
                    case AccessoryType.Glasses:
                        _customer.AdditionalFeatures.Accessories.GlassesConfidence = accessoryConfidence;
                        break;
                    case AccessoryType.Mask:
                        _customer.AdditionalFeatures.Accessories.MaskConfidence = accessoryConfidence;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        private void AddFaceApiCallCountToSessionServicesDetails(int faceAPICallCount)
        {
            _session.SessionServicesDetails.FaceApiCalls = faceAPICallCount;
        }
        private void AddSessionDurationToSession(TimeSpan sessionDuration)
        {
            _session.SessionDuration = sessionDuration;
            _sessionInterval.SessionIntervalTime = sessionDuration;
        }
        private void InitializeSessionVariables(TimeSpan analysisInterval)
        {
            _session = new Session();
            _session.SessionServicesDetails = new SessionServicesDetails();
            _session.SessionServicesDetails.IntervalSet = analysisInterval;
            _session.SessionDate = DateTime.Now.Date;
            _session.SessionStartTime = DateTime.Now.TimeOfDay;

            _customer = new Customer();
            _customer.Session = _session;
            _session.Customer = _customer;

            _sessionIntervals = new List<SessionInterval>();
            _session.SessionIntervals = _sessionIntervals;
        }
        private void InitializeSessionIntervalVariable()
        {
            _sessionInterval = new SessionInterval();
            _sessionInterval.CurrentTime = DateTime.Now.TimeOfDay;
        }
    }
}
