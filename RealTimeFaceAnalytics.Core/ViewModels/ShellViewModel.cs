using Caliburn.Micro;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using RealTimeFaceAnalytics.Core.Events;
using RealTimeFaceAnalytics.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RealTimeFaceAnalytics.Core.ViewModels
{
    public class ShellViewModel : Screen, IHandle<FrameImageProvidedEvent>, IHandle<ResultImageAvailableEvent>, IHandle<FaceAttributesResultEvent>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IVideoFrameAnalyzerService _videoFrameAnalyzerService;
        private readonly IVisualizationService _visualizationService;
        private readonly IEmotionService _emotionService;
        private readonly IFaceService _faceService;
        private readonly IDataInsertionService _dataInsertionService;
        private readonly DispatcherTimer _timer;

        private Stopwatch _stopWatch;

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

        private bool _isShellViewWindowsEnabled = true;
        public bool IsShellViewWindowsEnabled
        {
            get { return _isShellViewWindowsEnabled; }
            set
            {
                _isShellViewWindowsEnabled = value;
                NotifyOfPropertyChange(() => IsShellViewWindowsEnabled);
            }
        }

        private string _databaseStatement;
        public string DatabaseStatement
        {
            get { return _databaseStatement; }
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
                if (availableCameraList.Count != 0) { CameraListEnable = true; }
                return availableCameraList;
            }
        }

        private string _selectedCameraList;
        public string SelectedCameraList
        {
            get { return _selectedCameraList; }
            set
            {
                _selectedCameraList = value;
                NotifyOfPropertyChange(() => SelectedCameraList);
                CanStartAnalyze = true;
            }
        }

        private bool _canStartAnalyze = false;
        public bool CanStartAnalyze
        {
            get { return _canStartAnalyze; }
            set
            {
                _canStartAnalyze = value;
                NotifyOfPropertyChange(() => CanStartAnalyze);
            }
        }

        private bool _canStopAnalyze = false;
        public bool CanStopAnalyze
        {
            get { return _canStopAnalyze; }
            set
            {
                _canStopAnalyze = value;
                NotifyOfPropertyChange(() => CanStopAnalyze);
            }
        }

        private bool _cameraListEnable;
        public bool CameraListEnable
        {
            get { return _cameraListEnable; }
            set
            {
                _cameraListEnable = value;
                NotifyOfPropertyChange(() => CameraListEnable);
            }
        }

        private BitmapSource _frameImage;
        public BitmapSource FrameImage
        {
            get { return _frameImage; }
            set
            {
                _frameImage = value;
                NotifyOfPropertyChange(() => FrameImage);
            }
        }

        private BitmapSource _resultImage;
        public BitmapSource ResultImage
        {
            get { return _resultImage; }
            set
            {
                _resultImage = value;
                NotifyOfPropertyChange(() => ResultImage);
            }
        }

        private double _age;
        public double Age
        {
            get { return _age; }
            set
            {
                _age = value;
                NotifyOfPropertyChange(() => Age);
            }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                NotifyOfPropertyChange(() => Gender);
            }
        }

        private double _roll;
        public double Roll
        {
            get { return _roll; }
            set
            {
                _roll = value;
                NotifyOfPropertyChange(() => Roll);
            }
        }

        private double _yaw;
        public double Yaw
        {
            get { return _yaw; }
            set
            {
                _yaw = value;
                NotifyOfPropertyChange(() => Yaw);
            }
        }

        private double _pitch;
        public double Pitch
        {
            get { return _pitch; }
            set
            {
                _pitch = value;
                NotifyOfPropertyChange(() => Pitch);
            }
        }

        private double _bald;
        public double Bald
        {
            get { return _bald; }
            set
            {
                _bald = value;
                NotifyOfPropertyChange(() => Bald);
            }
        }

        private bool _isInvisible;
        public bool IsInvisible
        {
            get { return _isInvisible; }
            set
            {
                _isInvisible = value;
                NotifyOfPropertyChange(() => IsInvisible);
            }
        }

        private double _black;
        public double Black
        {
            get { return _black; }
            set
            {
                _black = value;
                NotifyOfPropertyChange(() => Black);
            }
        }

        private double _blond;
        public double Blond
        {
            get { return _blond; }
            set
            {
                _blond = value;
                NotifyOfPropertyChange(() => Blond);
            }
        }

        private double _brown;
        public double Brown
        {
            get { return _brown; }
            set
            {
                _brown = value;
                NotifyOfPropertyChange(() => Brown);
            }
        }

        private double _gray;
        public double Gray
        {
            get { return _gray; }
            set
            {
                _gray = value;
                NotifyOfPropertyChange(() => Gray);
            }
        }

        private double _other;
        public double Other
        {
            get { return _other; }
            set
            {
                _other = value;
                NotifyOfPropertyChange(() => Other);
            }
        }

        private double _red;
        public double Red
        {
            get { return _red; }
            set
            {
                _red = value;
                NotifyOfPropertyChange(() => Red);
            }
        }

        private double _unknown;
        public double Unknown
        {
            get { return _unknown; }
            set
            {
                _unknown = value;
                NotifyOfPropertyChange(() => Unknown);
            }
        }

        private double _white;
        public double White
        {
            get { return _white; }
            set
            {
                _white = value;
                NotifyOfPropertyChange(() => White);
            }
        }

        private double _moustache;
        public double Moustache
        {
            get { return _moustache; }
            set
            {
                _moustache = value;
                NotifyOfPropertyChange(() => Moustache);
            }
        }

        private double _beard;
        public double Beard
        {
            get { return _beard; }
            set
            {
                _beard = value;
                NotifyOfPropertyChange(() => Beard);
            }
        }

        private double _sideburns;
        public double Sideburns
        {
            get { return _sideburns; }
            set
            {
                _sideburns = value;
                NotifyOfPropertyChange(() => Sideburns);
            }
        }

        private string _glasses;
        public string Glasses
        {
            get { return _glasses; }
            set
            {
                _glasses = value;
                NotifyOfPropertyChange(() => Glasses);
            }
        }

        private bool _isEyeMakeup;
        public bool IsEyeMakeup
        {
            get { return _isEyeMakeup; }
            set
            {
                _isEyeMakeup = value;
                NotifyOfPropertyChange(() => IsEyeMakeup);
            }
        }

        private bool _isLipMakeup;
        public bool IsLipMakeup
        {
            get { return _isLipMakeup; }
            set
            {
                _isLipMakeup = value;
                NotifyOfPropertyChange(() => IsLipMakeup);
            }
        }

        private string _accessories;
        public string Accessories
        {
            get { return _accessories; }
            set
            {
                _accessories = value;
                NotifyOfPropertyChange(() => Accessories);
            }
        }

        private double _averageAge;
        public double AverageAge
        {
            get { return _averageAge; }
            set
            {
                _averageAge = value;
                NotifyOfPropertyChange(() => AverageAge);
            }
        }

        private int _faceAPICallCount;
        public int FaceAPICallCount
        {
            get { return _faceAPICallCount; }
            set
            {
                _faceAPICallCount = value;
                NotifyOfPropertyChange(() => FaceAPICallCount);
            }
        }

        private float _anger;
        public float Anger
        {
            get { return _anger; }
            set
            {
                _anger = value;
                NotifyOfPropertyChange(() => Anger);
            }
        }

        private float _contempt;
        public float Contempt
        {
            get { return _contempt; }
            set
            {
                _contempt = value;
                NotifyOfPropertyChange(() => Contempt);
            }
        }

        private float _disgust;
        public float Disgust
        {
            get { return _disgust; }
            set
            {
                _disgust = value;
                NotifyOfPropertyChange(() => Disgust);
            }
        }

        private float _fear;
        public float Fear
        {
            get { return _fear; }
            set
            {
                _fear = value;
                NotifyOfPropertyChange(() => Fear);
            }
        }

        private float _happiness;
        public float Happiness
        {
            get { return _happiness; }
            set
            {
                _happiness = value;
                NotifyOfPropertyChange(() => Happiness);
            }
        }

        private float _neutral;
        public float Neutral
        {
            get { return _neutral; }
            set
            {
                _neutral = value;
                NotifyOfPropertyChange(() => Neutral);
            }
        }

        private float _sadness;
        public float Sadness
        {
            get { return _sadness; }
            set
            {
                _sadness = value;
                NotifyOfPropertyChange(() => Sadness);
            }
        }

        private float _surprise;
        public float Surprise
        {
            get { return _surprise; }
            set
            {
                _surprise = value;
                NotifyOfPropertyChange(() => Surprise);
            }
        }

        private float _averageAnger;
        public float AverageAnger
        {
            get { return _averageAnger; }
            set
            {
                _averageAnger = value;
                NotifyOfPropertyChange(() => AverageAnger);
            }
        }

        private float _averageContempt;
        public float AverageContempt
        {
            get { return _averageContempt; }
            set
            {
                _averageContempt = value;
                NotifyOfPropertyChange(() => AverageContempt);
            }
        }

        private float _averageDisgust;
        public float AverageDisgust
        {
            get { return _averageDisgust; }
            set
            {
                _averageDisgust = value;
                NotifyOfPropertyChange(() => AverageDisgust);
            }
        }

        private float _averageFear;
        public float AverageFear
        {
            get { return _averageFear; }
            set
            {
                _averageFear = value;
                NotifyOfPropertyChange(() => AverageFear);
            }
        }

        private float _averageHappiness;
        public float AverageHappiness
        {
            get { return _averageHappiness; }
            set
            {
                _averageHappiness = value;
                NotifyOfPropertyChange(() => AverageHappiness);
            }
        }

        private float _averageNeutral;
        public float AverageNeutral
        {
            get { return _averageNeutral; }
            set
            {
                _averageNeutral = value;
                NotifyOfPropertyChange(() => AverageNeutral);
            }
        }

        private float _averageSadness;
        public float AverageSadness
        {
            get { return _averageSadness; }
            set
            {
                _averageSadness = value;
                NotifyOfPropertyChange(() => AverageSadness);
            }
        }

        private float _averageSurprise;
        public float AverageSurprise
        {
            get { return _averageSurprise; }
            set
            {
                _averageSurprise = value;
                NotifyOfPropertyChange(() => AverageSurprise);
            }
        }

        private ObservableCollection<Rectangle> _emotionBars;
        public ObservableCollection<Rectangle> EmotionBars
        {
            get { return _emotionBars; }
            set
            {
                _emotionBars = value;
                NotifyOfPropertyChange(() => EmotionBars);
            }
        }

        private ObservableCollection<Rectangle> _hairColor;
        public ObservableCollection<Rectangle> HairColor
        {
            get { return _hairColor; }
            set
            {
                _hairColor = value;
                NotifyOfPropertyChange(() => HairColor);
            }
        }

        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                NotifyOfPropertyChange(() => CurrentTime);
            }
        }

        private string _currentSessionTimer = "00:00.000";
        public string CurrentSessionTimer
        {
            get { return _currentSessionTimer; }
            set
            {
                _currentSessionTimer = value;
                NotifyOfPropertyChange(() => CurrentSessionTimer);
            }
        }

        private bool _settingsPanelIsVisible = true;
        public bool SettingsPanelIsVisible
        {
            get { return _settingsPanelIsVisible; }
            set
            {
                _settingsPanelIsVisible = value;
                NotifyOfPropertyChange(() => SettingsPanelIsVisible);
            }
        }

        private bool _statisticsIsVisible;
        public bool StatisticsIsVisible
        {
            get { return _statisticsIsVisible; }
            set
            {
                _statisticsIsVisible = value;
                NotifyOfPropertyChange(() => StatisticsIsVisible);
            }
        }

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
            if (SettingsPanelIsVisible) { ShowHideSettings(); }
            if (!StatisticsIsVisible) { ShowHideStatistics(); }
        }

        public void StopAnalyze()
        {
            StopProcessing();
            StopStopwatch();
            CameraListEnable = true;
            CanStartAnalyze = true;
            CanStopAnalyze = false;
            if (!StatisticsIsVisible) { ShowHideStatistics(); }
        }

        public void ShowHideSettings()
        {
            SettingsPanelIsVisible = !SettingsPanelIsVisible;
        }

        public void ShowHideStatistics()
        {
            StatisticsIsVisible = !StatisticsIsVisible;
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
            ShowHideSettings();
        }

        public void Handle(FrameImageProvidedEvent message)
        {
            FrameImage = message.FrameImage;
        }

        public void Handle(ResultImageAvailableEvent message)
        {
            ResultImage = message.ResultImage;
        }

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

            var faceApiCallCount = _faceService.GetFaceServiceClientAPICallCount();
            AssignFaceApiCallCount(faceApiCallCount);
            _dataInsertionService.AddFaceApiCallCount(faceApiCallCount);

            _dataInsertionService.AddSessionIntervalData();
            _dataInsertionService.AddSessionDuration(_stopWatch.Elapsed);
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
        private void SetCurrentTimeHandler(object sender, EventArgs args)
        {
            CurrentTime = DateTime.Now.ToLongTimeString();
        }
        private void StartStopwatch()
        {
            _stopWatch = Stopwatch.StartNew();
            _timer.Tick += StopwatchHandler;
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
                if (hairColor.Color == HairColorType.Black) { Black = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Blond) { Blond = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Brown) { Brown = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Gray) { Gray = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Other) { Other = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Red) { Red = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.Unknown) { Unknown = hairColor.Confidence; }
                if (hairColor.Color == HairColorType.White) { White = hairColor.Confidence; }
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
                    var accessoryConfidence = accessory.Confidence.ToString();
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
            FaceAPICallCount = faceApiCallCount; ;
        }
    }
}
