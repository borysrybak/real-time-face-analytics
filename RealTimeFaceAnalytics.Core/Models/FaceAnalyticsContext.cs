using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace RealTimeFaceAnalytics.Core.Models
{
    public class FaceAnalyticsContext : DbContext
    {
        public FaceAnalyticsContext() : base("name=FaceAnalyticsContext")
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Emotions> Emotions { get; set; }
        public virtual DbSet<EmotionsInterval> EmotionsInterval { get; set; }
        public virtual DbSet<Hair> Hair { get; set; }
        public virtual DbSet<FacialHair> FacialHair { get; set; }
        public virtual DbSet<AdditionalFeatures> AdditionalFeatures { get; set; }
        public virtual DbSet<Accessories> Accessories { get; set; }
        public virtual DbSet<Session> Session { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string AverageAge { get; set; }
        public Gender Gender { get; set; }
        public Emotions AverageEmotions { get; set; }
        public Hair AverageHair { get; set; }
        public FacialHair FacialHair { get; set; }
        public AdditionalFeatures AdditionalFeatures { get; set; }

        public Session Session { get; set; }
    }

    public class Emotions
    {
        public int EmotionsId { get; set; }
        public string Anger { get; set; }
        public string Happiness { get; set; }
        public string Contempt { get; set; }
        public string Neutral { get; set; }
        public string Disgust { get; set; }
        public string Sadness { get; set; }
        public string Fear { get; set; }
        public string Surprise { get; set; }
    }

    public class EmotionsInterval
    {
        public int EmotionsIntervalId { get; set; }
        public TimeSpan CurrentTime { get; set; }
        public TimeSpan SessionIntervalTime { get; set; }
        public Emotions Emotions { get; set; }

        public int SessionId { get; set; }
        public virtual Session Session { get; set; }
    }

    public class Hair 
    {
        public int HairId { get; set; }
        public string Bald { get; set; }
        public string Brown { get; set; }
        public string Black { get; set; }
        public string Gray { get; set; }
        public string White { get; set; }
        public string Blond { get; set; }
        public string Other { get; set; }
        public string Red { get; set; }
        public string Unknown { get; set; }
        public bool IsInvisible { get; set; }
    }

    public class FacialHair
    {
        public int FacialHairId { get; set; }
        public string Moustache { get; set; }
        public string Beard { get; set; }
        public string Sideburns { get; set; }
    }

    public class AdditionalFeatures
    {
        public int AdditionalFeaturesId { get; set; }
        public Glasses Glasses { get; set; }
        public bool IsEyeMakeup { get; set; }
        public bool IsLipMakeup { get; set; }
        public Accessories Accessories { get; set; }
    }

    public class Accessories
    {
        public int AccessoriesId { get; set; }
        public bool IsHeadwear { get; set; }
        public bool IsGlasses { get; set; }
        public bool IsMask { get; set; }
    }

    public class Session
    {
        public int SessionsId { get; set; }
        public int CognitiveServiceAPICalls { get; set; }
        public DateTime CurrentDate { get; set; }
        public TimeSpan CurrentTime { get; set; }
        public TimeSpan SessionStart { get; set; }
        public TimeSpan SessionEnd { get; set; }

        public Customer Customer { get; set; }
        public virtual List<EmotionsInterval> EmotionsIntervals { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum Glasses
    {
        NoGlasses,
        ReadingGlasses,
        Sunglasses,
        SwimmingGoggles
    }
}