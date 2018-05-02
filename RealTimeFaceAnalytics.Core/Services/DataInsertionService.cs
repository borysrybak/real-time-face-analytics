using System;
using System.Collections.Generic;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Models;
using FacialHair = Microsoft.ProjectOxford.Face.Contract.FacialHair;
using Hair = Microsoft.ProjectOxford.Face.Contract.Hair;

namespace RealTimeFaceAnalytics.Core.Services
{
    public class DataInsertionService : IDataInsertionService
    {
        #region Fields

        private Customer _customer;
        private Session _session;
        private SessionInterval _sessionInterval;
        private List<SessionInterval> _sessionIntervals;

        #endregion Fields

        #region Methods

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

        public void AddHair(Hair hair)
        {
            AddHairToCustomer(hair);
        }

        public void AddFacialHair(FacialHair facialHair)
        {
            AddFacialHairToCustomer(facialHair);
        }

        public void AddAdditionalFeatures(FaceAttributes additionalFeatures)
        {
            AddAdditionalFeaturesToCustomer(additionalFeatures);
        }

        public void AddFaceApiCallCount(int faceApiCallCount)
        {
            AddFaceApiCallCountToSessionServicesDetails(faceApiCallCount);
        }

        public void AddSessionDuration(TimeSpan sessionDuration)
        {
            AddSessionDurationToSession(sessionDuration);
        }

        #endregion Methods

        #region Private Methods

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
            _sessionInterval.Emotions = new Emotions
            {
                Anger = emotionScores.Anger,
                Happiness = emotionScores.Happiness,
                Contempt = emotionScores.Contempt,
                Neutral = emotionScores.Neutral,
                Disgust = emotionScores.Disgust,
                Sadness = emotionScores.Sadness,
                Fear = emotionScores.Fear,
                Surprise = emotionScores.Surprise
            };
        }

        private void AddAverageEmotionsToCustomer(EmotionScores averageEmotions)
        {
            _customer.AverageEmotions = new Emotions
            {
                Anger = averageEmotions.Anger,
                Happiness = averageEmotions.Happiness,
                Contempt = averageEmotions.Contempt,
                Neutral = averageEmotions.Neutral,
                Disgust = averageEmotions.Disgust,
                Sadness = averageEmotions.Sadness,
                Fear = averageEmotions.Fear,
                Surprise = averageEmotions.Surprise
            };
        }

        private void AddHairToCustomer(Hair hair)
        {
            _customer.Hair = new Models.Hair
            {
                Bald = hair.Bald,
                IsInvisible = hair.Invisible
            };

            var hairColors = hair.HairColor;
            foreach (var hairColor in hairColors)
            {
                if (hairColor.Color == HairColorType.Black) _customer.Hair.Black = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Blond) _customer.Hair.Blond = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Brown) _customer.Hair.Brown = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Gray) _customer.Hair.Gray = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Other) _customer.Hair.Other = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Red) _customer.Hair.Red = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Unknown) _customer.Hair.Unknown = hairColor.Confidence;
                if (hairColor.Color == HairColorType.White) _customer.Hair.White = hairColor.Confidence;
            }
        }

        private void AddFacialHairToCustomer(FacialHair facialHair)
        {
            _customer.FacialHair = new Models.FacialHair
            {
                Moustache = facialHair.Moustache,
                Beard = facialHair.Beard,
                Sideburns = facialHair.Sideburns
            };
        }

        private void AddAdditionalFeaturesToCustomer(FaceAttributes additionalFeatures)
        {
            var glasses = additionalFeatures.Glasses.ToString();
            _customer.AdditionalFeatures = new AdditionalFeatures
            {
                Glasses = glasses,
                IsEyeMakeup = additionalFeatures.Makeup.EyeMakeup,
                IsLipMakeup = additionalFeatures.Makeup.LipMakeup
            };

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

        private void AddFaceApiCallCountToSessionServicesDetails(int faceApiCallCount)
        {
            _session.SessionServicesDetails.FaceApiCalls = faceApiCallCount;
        }

        private void AddSessionDurationToSession(TimeSpan sessionDuration)
        {
            _session.SessionDuration = sessionDuration;
            _sessionInterval.SessionIntervalTime = sessionDuration;
        }

        private void InitializeSessionVariables(TimeSpan analysisInterval)
        {
            _session = new Session
            {
                SessionServicesDetails = new SessionServicesDetails {IntervalSet = analysisInterval},
                SessionDate = DateTime.Now.Date,
                SessionStartTime = DateTime.Now.TimeOfDay
            };

            _customer = new Customer();
            _session.Customer = _customer;

            _sessionIntervals = new List<SessionInterval>();
            _session.SessionIntervals = _sessionIntervals;
        }

        private void InitializeSessionIntervalVariable()
        {
            _sessionInterval = new SessionInterval {CurrentTime = DateTime.Now.TimeOfDay};
        }

        #endregion Private Methods
    }
}