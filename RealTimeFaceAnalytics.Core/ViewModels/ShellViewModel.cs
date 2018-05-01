using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Caliburn.Micro;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeFaceAnalytics.Core.Events;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Properties;

namespace RealTimeFaceAnalytics.Core.ViewModels
{
    /// <summary>
    ///     Main View Model for entire application. Can be loaded with different views as UserControls.
    /// </summary>
    public class ShellViewModel : Screen, IHandle<FrameImageProvidedEvent>, IHandle<ResultImageAvailableEvent>,
        IHandle<FaceAttributesResultEvent>
    {
        #region Types

        public ShellViewModel(IEventAggregator eventAggregator, IVideoFrameAnalyzerService videoFrameAnalyzerService,
            IVisualizationService visualizationService, IEmotionService emotionService, IFaceService faceService,
            IDataInsertionService dataInsertionService)
        {
            _eventAggregator = eventAggregator;
            _videoFrameAnalyzerService = videoFrameAnalyzerService;
            _visualizationService = visualizationService;
            _emotionService = emotionService;
            _faceService = faceService;
            _dataInsertionService = dataInsertionService;
            _videoFrameAnalyzerService.InitializeFrameGrabber();

            _timer = new DispatcherTimer(DispatcherPriority.Render);
            SetCurrentTime();
        }

        #endregion Types

        #region Fields

        private readonly IDataInsertionService _dataInsertionService;
        private readonly IEmotionService _emotionService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IFaceService _faceService;
        private readonly DispatcherTimer _timer;
        private readonly IVideoFrameAnalyzerService _videoFrameAnalyzerService;
        private readonly IVisualizationService _visualizationService;

        private string _accessories;
        private double _age;
        private float _anger;
        private double _averageAge;
        private float _averageAnger;
        private float _averageContempt;
        private float _averageDisgust;
        private float _averageFear;
        private float _averageHappiness;
        private float _averageNeutral;
        private float _averageSadness;
        private float _averageSurprise;
        private double _bald;
        private double _beard;
        private double _black;
        private double _blond;
        private double _brown;
        private bool _cameraListEnable;
        private bool _canStartAnalyze;
        private bool _canStopAnalyze;
        private float _contempt;
        private string _currentSessionTimer = "00:00.000";
        private string _currentTime;
        private string _databaseStatement;
        private float _disgust;
        private ObservableCollection<Rectangle> _emotionBars;
        private int _faceApiCallCount;
        private float _fear;
        private BitmapSource _frameImage;
        private string _gender;
        private string _glasses;
        private double _gray;
        private ObservableCollection<Rectangle> _hairColor;
        private float _happiness;
        private bool _isEyeMakeup;
        private bool _isInvisible;
        private bool _isLipMakeup;
        private bool _isShellViewWindowsEnabled = true;
        private double _moustache;
        private float _neutral;
        private double _other;
        private double _pitch;
        private double _red;
        private BitmapSource _resultImage;
        private double _roll;
        private float _sadness;
        private string _selectedCameraList;
        private bool _settingsPanelIsVisible = true;
        private double _sideburns;
        private bool _statisticsIsVisible;
        private Stopwatch _stopWatch;
        private float _surprise;
        private double _unknown;
        private double _white;
        private double _yaw;

        #endregion Fields

        #region Properties

        public bool IsShellViewWindowsEnabled
        {
            get => _isShellViewWindowsEnabled;
            set
            {
                _isShellViewWindowsEnabled = value;
                NotifyOfPropertyChange(() => IsShellViewWindowsEnabled);
            }
        }

        public string DatabaseStatement
        {
            get => _databaseStatement;
            set
            {
                _databaseStatement = value;
                NotifyOfPropertyChange(() => DatabaseStatement);
            }
        }

        public List<string> CameraList
        {
            get
            {
                var availableCameraList = _videoFrameAnalyzerService.GetAvailableCameraList();
                if (availableCameraList.Count != 0) CameraListEnable = true;
                return availableCameraList;
            }
        }

        public string SelectedCameraList
        {
            get => _selectedCameraList;
            set
            {
                _selectedCameraList = value;
                NotifyOfPropertyChange(() => SelectedCameraList);
                CanStartAnalyze = true;
            }
        }

        public bool CanStartAnalyze
        {
            get => _canStartAnalyze;
            set
            {
                _canStartAnalyze = value;
                NotifyOfPropertyChange(() => CanStartAnalyze);
            }
        }

        public bool CanStopAnalyze
        {
            get => _canStopAnalyze;
            set
            {
                _canStopAnalyze = value;
                NotifyOfPropertyChange(() => CanStopAnalyze);
            }
        }

        public bool CameraListEnable
        {
            get => _cameraListEnable;
            set
            {
                _cameraListEnable = value;
                NotifyOfPropertyChange(() => CameraListEnable);
            }
        }

        public BitmapSource FrameImage
        {
            get => _frameImage;
            set
            {
                _frameImage = value;
                NotifyOfPropertyChange(() => FrameImage);
            }
        }

        public BitmapSource ResultImage
        {
            get => _resultImage;
            set
            {
                _resultImage = value;
                NotifyOfPropertyChange(() => ResultImage);
            }
        }

        public double Age
        {
            get => _age;
            set
            {
                _age = value;
                NotifyOfPropertyChange(() => Age);
            }
        }

        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                NotifyOfPropertyChange(() => Gender);
            }
        }

        public double Roll
        {
            get => _roll;
            set
            {
                _roll = value;
                NotifyOfPropertyChange(() => Roll);
            }
        }

        public double Yaw
        {
            get => _yaw;
            set
            {
                _yaw = value;
                NotifyOfPropertyChange(() => Yaw);
            }
        }

        public double Pitch
        {
            get => _pitch;
            set
            {
                _pitch = value;
                NotifyOfPropertyChange(() => Pitch);
            }
        }

        public double Bald
        {
            get => _bald;
            set
            {
                _bald = value;
                NotifyOfPropertyChange(() => Bald);
            }
        }

        public bool IsInvisible
        {
            get => _isInvisible;
            set
            {
                _isInvisible = value;
                NotifyOfPropertyChange(() => IsInvisible);
            }
        }

        public double Black
        {
            get => _black;
            set
            {
                _black = value;
                NotifyOfPropertyChange(() => Black);
            }
        }

        public double Blond
        {
            get => _blond;
            set
            {
                _blond = value;
                NotifyOfPropertyChange(() => Blond);
            }
        }

        public double Brown
        {
            get => _brown;
            set
            {
                _brown = value;
                NotifyOfPropertyChange(() => Brown);
            }
        }

        public double Gray
        {
            get => _gray;
            set
            {
                _gray = value;
                NotifyOfPropertyChange(() => Gray);
            }
        }

        public double Other
        {
            get => _other;
            set
            {
                _other = value;
                NotifyOfPropertyChange(() => Other);
            }
        }

        public double Red
        {
            get => _red;
            set
            {
                _red = value;
                NotifyOfPropertyChange(() => Red);
            }
        }

        public double Unknown
        {
            get => _unknown;
            set
            {
                _unknown = value;
                NotifyOfPropertyChange(() => Unknown);
            }
        }

        public double White
        {
            get => _white;
            set
            {
                _white = value;
                NotifyOfPropertyChange(() => White);
            }
        }

        public double Moustache
        {
            get => _moustache;
            set
            {
                _moustache = value;
                NotifyOfPropertyChange(() => Moustache);
            }
        }

        public double Beard
        {
            get => _beard;
            set
            {
                _beard = value;
                NotifyOfPropertyChange(() => Beard);
            }
        }

        public double Sideburns
        {
            get => _sideburns;
            set
            {
                _sideburns = value;
                NotifyOfPropertyChange(() => Sideburns);
            }
        }

        public string Glasses
        {
            get => _glasses;
            set
            {
                _glasses = value;
                NotifyOfPropertyChange(() => Glasses);
            }
        }

        public bool IsEyeMakeup
        {
            get => _isEyeMakeup;
            set
            {
                _isEyeMakeup = value;
                NotifyOfPropertyChange(() => IsEyeMakeup);
            }
        }

        public bool IsLipMakeup
        {
            get => _isLipMakeup;
            set
            {
                _isLipMakeup = value;
                NotifyOfPropertyChange(() => IsLipMakeup);
            }
        }

        public string Accessories
        {
            get => _accessories;
            set
            {
                _accessories = value;
                NotifyOfPropertyChange(() => Accessories);
            }
        }

        public double AverageAge
        {
            get => _averageAge;
            set
            {
                _averageAge = value;
                NotifyOfPropertyChange(() => AverageAge);
            }
        }

        public int FaceApiCallCount
        {
            get => _faceApiCallCount;
            set
            {
                _faceApiCallCount = value;
                NotifyOfPropertyChange(() => FaceApiCallCount);
            }
        }

        public float Anger
        {
            get => _anger;
            set
            {
                _anger = value;
                NotifyOfPropertyChange(() => Anger);
            }
        }

        public float Contempt
        {
            get => _contempt;
            set
            {
                _contempt = value;
                NotifyOfPropertyChange(() => Contempt);
            }
        }

        public float Disgust
        {
            get => _disgust;
            set
            {
                _disgust = value;
                NotifyOfPropertyChange(() => Disgust);
            }
        }

        public float Fear
        {
            get => _fear;
            set
            {
                _fear = value;
                NotifyOfPropertyChange(() => Fear);
            }
        }

        public float Happiness
        {
            get => _happiness;
            set
            {
                _happiness = value;
                NotifyOfPropertyChange(() => Happiness);
            }
        }

        public float Neutral
        {
            get => _neutral;
            set
            {
                _neutral = value;
                NotifyOfPropertyChange(() => Neutral);
            }
        }

        public float Sadness
        {
            get => _sadness;
            set
            {
                _sadness = value;
                NotifyOfPropertyChange(() => Sadness);
            }
        }

        public float Surprise
        {
            get => _surprise;
            set
            {
                _surprise = value;
                NotifyOfPropertyChange(() => Surprise);
            }
        }

        public float AverageAnger
        {
            get => _averageAnger;
            set
            {
                _averageAnger = value;
                NotifyOfPropertyChange(() => AverageAnger);
            }
        }

        public float AverageContempt
        {
            get => _averageContempt;
            set
            {
                _averageContempt = value;
                NotifyOfPropertyChange(() => AverageContempt);
            }
        }

        public float AverageDisgust
        {
            get => _averageDisgust;
            set
            {
                _averageDisgust = value;
                NotifyOfPropertyChange(() => AverageDisgust);
            }
        }

        public float AverageFear
        {
            get => _averageFear;
            set
            {
                _averageFear = value;
                NotifyOfPropertyChange(() => AverageFear);
            }
        }

        public float AverageHappiness
        {
            get => _averageHappiness;
            set
            {
                _averageHappiness = value;
                NotifyOfPropertyChange(() => AverageHappiness);
            }
        }

        public float AverageNeutral
        {
            get => _averageNeutral;
            set
            {
                _averageNeutral = value;
                NotifyOfPropertyChange(() => AverageNeutral);
            }
        }

        public float AverageSadness
        {
            get => _averageSadness;
            set
            {
                _averageSadness = value;
                NotifyOfPropertyChange(() => AverageSadness);
            }
        }

        public float AverageSurprise
        {
            get => _averageSurprise;
            set
            {
                _averageSurprise = value;
                NotifyOfPropertyChange(() => AverageSurprise);
            }
        }

        public ObservableCollection<Rectangle> EmotionBars
        {
            get => _emotionBars;
            set
            {
                _emotionBars = value;
                NotifyOfPropertyChange(() => EmotionBars);
            }
        }

        public ObservableCollection<Rectangle> HairColor
        {
            get => _hairColor;
            set
            {
                _hairColor = value;
                NotifyOfPropertyChange(() => HairColor);
            }
        }

        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                NotifyOfPropertyChange(() => CurrentTime);
            }
        }

        public string CurrentSessionTimer
        {
            get => _currentSessionTimer;
            set
            {
                _currentSessionTimer = value;
                NotifyOfPropertyChange(() => CurrentSessionTimer);
            }
        }

        public bool SettingsPanelIsVisible
        {
            get => _settingsPanelIsVisible;
            set
            {
                _settingsPanelIsVisible = value;
                NotifyOfPropertyChange(() => SettingsPanelIsVisible);
            }
        }

        public bool StatisticsIsVisible
        {
            get => _statisticsIsVisible;
            set
            {
                _statisticsIsVisible = value;
                NotifyOfPropertyChange(() => StatisticsIsVisible);
            }
        }

        #endregion Properties

        #region Methods

            #region Override Methods

        protected override void OnActivate()
        {
            _eventAggregator.Subscribe(this);
            base.OnActivate();
        }

        protected override void OnDeactivate(bool close)
        {
            _eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        #endregion Override Methods

            #region Commands

        /// <summary>
        /// Starts Video Frame Analyzer Service process.
        /// </summary>
        public void StartAnalyze()
        {
            _faceService.ResetFaceServiceLocalData();
            _emotionService.ResetEmotionServiceLocalData();
            EmotionBars = new ObservableCollection<Rectangle>();
            DatabaseStatement = string.Empty;

            _videoFrameAnalyzerService.StartProcessing(_selectedCameraList);
            StartStopwatch();
            CameraListEnable = false;
            CanStartAnalyze = false;
            CanStopAnalyze = true;
            if (SettingsPanelIsVisible) ShowHideSettings();
            if (!StatisticsIsVisible) ShowHideStatistics();
        }

        /// <summary>
        /// Stops Video Frame Analyzer Service process.
        /// </summary>
        public void StopAnalyze()
        {
            StopProcessing();
            StopStopwatch();
            CameraListEnable = true;
            CanStartAnalyze = true;
            CanStopAnalyze = false;
            if (!StatisticsIsVisible) ShowHideStatistics();
        }

        public void SaveSettings()
        {
            Settings.Default.Save();
            ShowHideSettings();
        }

        #endregion Commands
        
            #region Private Methods

        private void ShowHideSettings()
        {
            SettingsPanelIsVisible = !SettingsPanelIsVisible;
        }

        private void ShowHideStatistics()
        {
            StatisticsIsVisible = !StatisticsIsVisible;
        }
        
        private async void StopProcessing()
        {
            DatabaseStatement = "Adding to database";
            IsShellViewWindowsEnabled = false;
            await _videoFrameAnalyzerService.StopProcessing();
            DatabaseStatement = "Added";
            IsShellViewWindowsEnabled = true;
        }

        private void SetCurrentTime()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(250);
            _timer.Tick += SetCurrentTimeHandler;
            _timer.Start();
        }

        private void StartStopwatch()
        {
            _stopWatch = Stopwatch.StartNew();
            _timer.Tick += StopwatchHandler;
        }

        private void StopStopwatch()
        {
            _stopWatch.Stop();
            _timer.Tick -= StopwatchHandler;
        }

        private void AssignFaceAttributes(FaceAttributes faceAttributes)
        {
            AssignBasicAttributes(faceAttributes);
            AssignHairAttributes(faceAttributes);
            AssignFacialHairAttributes(faceAttributes);
            AssignAdditionalAttributes(faceAttributes);
            AssignEmotionAttributes(faceAttributes);
        }

        private void AssignBasicAttributes(FaceAttributes faceAttributes)
        {
            var age = faceAttributes.Age;
            Age = age;
            _faceService.AddAgeToStatistics(age);
            _dataInsertionService.AddAge(age);

            var gender = faceAttributes.Gender;
            Gender = gender;
            _faceService.AddGenderToStatistics(gender);
            _dataInsertionService.AddGender(gender);

            Roll = faceAttributes.HeadPose.Roll;
            Yaw = faceAttributes.HeadPose.Yaw;
            Pitch = faceAttributes.HeadPose.Pitch;
        }

        private void AssignHairAttributes(FaceAttributes faceAttributes)
        {
            Bald = faceAttributes.Hair.Bald;
            IsInvisible = faceAttributes.Hair.Invisible;

            var hairColors = faceAttributes.Hair.HairColor;
            foreach (var hairColor in hairColors)
            {
                if (hairColor.Color == HairColorType.Black) Black = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Blond) Blond = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Brown) Brown = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Gray) Gray = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Other) Other = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Red) Red = hairColor.Confidence;
                if (hairColor.Color == HairColorType.Unknown) Unknown = hairColor.Confidence;
                if (hairColor.Color == HairColorType.White) White = hairColor.Confidence;
            }

            var hair = faceAttributes.Hair;
            _dataInsertionService.AddHair(hair);
        }

        private void AssignFacialHairAttributes(FaceAttributes faceAttributes)
        {
            Moustache = faceAttributes.FacialHair.Moustache;
            Beard = faceAttributes.FacialHair.Beard;
            Sideburns = faceAttributes.FacialHair.Sideburns;

            var facialHair = faceAttributes.FacialHair;
            _dataInsertionService.AddFacialHair(facialHair);
        }

        private void AssignAdditionalAttributes(FaceAttributes faceAttributes)
        {
            Glasses = faceAttributes.Glasses.ToString();
            IsEyeMakeup = faceAttributes.Makeup.EyeMakeup;
            IsLipMakeup = faceAttributes.Makeup.LipMakeup;

            var accessories = faceAttributes.Accessories;
            if (accessories.Length > 0)
            {
                var accessoryList = new StringBuilder();
                foreach (var accessory in accessories)
                {
                    var accessoryType = accessory.Type.ToString();
                    var accessoryConfidence = accessory.Confidence.ToString(CultureInfo.InvariantCulture);
                    accessoryList.Append(accessoryType + ": " + accessoryConfidence + ", ");
                }

                Accessories = accessoryList.ToString();
            }
            else
            {
                Accessories = "None";
            }
        }

        private void AssignEmotionAttributes(FaceAttributes faceAttributes)
        {
            Anger = faceAttributes.Emotion.Anger;
            Contempt = faceAttributes.Emotion.Contempt;
            Disgust = faceAttributes.Emotion.Disgust;
            Fear = faceAttributes.Emotion.Fear;
            Happiness = faceAttributes.Emotion.Happiness;
            Neutral = faceAttributes.Emotion.Neutral;
            Sadness = faceAttributes.Emotion.Sadness;
            Surprise = faceAttributes.Emotion.Surprise;
        }

        private void GenerateAndPopulateEmotionBar(EmotionScores emotionScores)
        {
            var bar = _visualizationService.ComposeRectangleBar(emotionScores);
            EmotionBars.Add(bar);
        }

        private void AssignEmotionStatistics(EmotionScores emotionScoresStatistics)
        {
            AverageAnger = emotionScoresStatistics.Anger;
            AverageContempt = emotionScoresStatistics.Contempt;
            AverageDisgust = emotionScoresStatistics.Disgust;
            AverageFear = emotionScoresStatistics.Fear;
            AverageHappiness = emotionScoresStatistics.Happiness;
            AverageNeutral = emotionScoresStatistics.Neutral;
            AverageSadness = emotionScoresStatistics.Sadness;
            AverageSurprise = emotionScoresStatistics.Surprise;
        }

        private void AssignAverageAge(double averageAge)
        {
            AverageAge = averageAge;
        }

        private void GenerateHairColor(HairColor[] hairColors)
        {
            var mixedHairColor = _visualizationService.MixHairColor(hairColors);
            HairColor = new ObservableCollection<Rectangle>(mixedHairColor);
        }

        private void AssignFaceApiCallCount(int faceApiCallCount)
        {
            FaceApiCallCount = faceApiCallCount;
        }

        #endregion Private Methods

        #endregion Methods

        #region Handlers

        public void Handle(FaceAttributesResultEvent message)
        {
            _dataInsertionService.InitializeSessionInterval();
            var faceAttributes = message.FaceAttributesResult;
            AssignFaceAttributes(faceAttributes);
            _dataInsertionService.AddAdditionalFeatures(faceAttributes);

            var averageAge = _faceService.CalculateAverageAge();
            AssignAverageAge(averageAge);
            _dataInsertionService.AddAverageAge(averageAge);

            var averageGender = _faceService.CalculateAverageGender();
            _dataInsertionService.AddAverageGender(averageGender);

            var emotionScores = faceAttributes.Emotion;
            GenerateAndPopulateEmotionBar(emotionScores);
            _emotionService.AddEmotionScoresToStatistics(emotionScores);
            _dataInsertionService.AddEmotions(emotionScores);

            var emotionScoresStatistics = _emotionService.CalculateEmotionScoresStatistics();
            AssignEmotionStatistics(emotionScoresStatistics);
            _dataInsertionService.AddAverageEmotions(emotionScoresStatistics);

            var hairColors = faceAttributes.Hair.HairColor;
            GenerateHairColor(hairColors);

            var faceApiCallCount = _faceService.GetFaceServiceClientApiCallCount();
            AssignFaceApiCallCount(faceApiCallCount);
            _dataInsertionService.AddFaceApiCallCount(faceApiCallCount);

            _dataInsertionService.AddSessionIntervalData();
            _dataInsertionService.AddSessionDuration(_stopWatch.Elapsed);
        }

        public void Handle(FrameImageProvidedEvent message)
        {
            FrameImage = message.FrameImage;
        }

        public void Handle(ResultImageAvailableEvent message)
        {
            ResultImage = message.ResultImage;
        }

        private void SetCurrentTimeHandler(object sender, EventArgs args)
        {
            CurrentTime = DateTime.Now.ToLongTimeString();
        }

        private void StopwatchHandler(object sender, EventArgs args)
        {
            var stopWatchTimeSpan = _stopWatch.Elapsed;
            var elapsedMiliseconds = stopWatchTimeSpan.Milliseconds;
            var elapsedSeconds = stopWatchTimeSpan.Seconds;
            var elapsedMinutes = stopWatchTimeSpan.Minutes;

            var currentSessionTime = $"{elapsedMinutes}:{elapsedSeconds}.{elapsedMiliseconds}";
            CurrentSessionTimer = currentSessionTime;
        }

        #endregion Handlers
    }
}