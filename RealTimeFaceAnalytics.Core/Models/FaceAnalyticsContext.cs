using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual DbSet<SessionInterval> SessionInterval { get; set; }
        public virtual DbSet<Hair> Hair { get; set; }
        public virtual DbSet<FacialHair> FacialHair { get; set; }
        public virtual DbSet<AdditionalFeatures> AdditionalFeatures { get; set; }
        public virtual DbSet<Accessories> Accessories { get; set; }
        public virtual DbSet<SessionServicesDetails> CognitiveService { get; set; }
        public virtual DbSet<Session> Session { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public double AverageAge { get; set; }
        public string AverageGender { get; set; }
        public Emotions AverageEmotions { get; set; }
        public Hair Hair { get; set; }
        public FacialHair FacialHair { get; set; }
        public AdditionalFeatures AdditionalFeatures { get; set; }

        public virtual Session Session { get; set; }
    }

    public class Emotions
    {
        public int EmotionsId { get; set; }
        public double Anger { get; set; }
        public double Happiness { get; set; }
        public double Contempt { get; set; }
        public double Neutral { get; set; }
        public double Disgust { get; set; }
        public double Sadness { get; set; }
        public double Fear { get; set; }
        public double Surprise { get; set; }
    }

    public class SessionInterval
    {
        public int SessionIntervalId { get; set; }
        public TimeSpan? CurrentTime { get; set; }
        public TimeSpan? SessionIntervalTime { get; set; }

        public double Age { get; set; }
        public string Gender { get; set; }
        public Emotions Emotions { get; set; }
    }

    public class Hair 
    {
        public int HairId { get; set; }
        public double Bald { get; set; }
        public double Brown { get; set; }
        public double Black { get; set; }
        public double Gray { get; set; }
        public double White { get; set; }
        public double Blond { get; set; }
        public double Other { get; set; }
        public double Red { get; set; }
        public double Unknown { get; set; }
        public bool IsInvisible { get; set; }
    }

    public class FacialHair
    {
        public int FacialHairId { get; set; }
        public double Moustache { get; set; }
        public double Beard { get; set; }
        public double Sideburns { get; set; }
    }

    public class AdditionalFeatures
    {
        public int AdditionalFeaturesId { get; set; }
        public string Glasses { get; set; }
        public bool IsEyeMakeup { get; set; }
        public bool IsLipMakeup { get; set; }
        public Accessories Accessories { get; set; }
    }

    public class Accessories
    {
        public int AccessoriesId { get; set; }
        public double HeadwearConfidence { get; set; }
        public double GlassesConfidence { get; set; }
        public double MaskConfidence { get; set; }
    }

    public class SessionServicesDetails
    {
        public int SessionServicesDetailsId { get; set; }
        public int FaceApiCalls { get; set; }
        public int EmotionApiCalls { get; set; }
        public int VisionApiCalls { get; set; }

        public TimeSpan? IntervalSet { get; set; }
    }

    public class Session
    {
        [ForeignKey("Customer")]
        public int SessionId { get; set; }
        public SessionServicesDetails SessionServicesDetails { get; set; }
        public DateTime? SessionDate { get; set; }
        public TimeSpan? SessionStartTime { get; set; }
        public TimeSpan? SessionEndTime { get; set; }
        public TimeSpan? SessionDuration { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual List<SessionInterval> SessionIntervals { get; set; }
    }
}