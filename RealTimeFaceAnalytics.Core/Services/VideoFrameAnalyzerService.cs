using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using RealTimeFaceAnalytics.Core.Events;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Models;
using RealTimeFaceAnalytics.Core.Properties;
using VideoFrameAnalyzer;
using Action = System.Action;

namespace RealTimeFaceAnalytics.Core.Services
{
    public class VideoFrameAnalyzerService : IVideoFrameAnalyzerService
    {
        private readonly IDataInsertionService _dataInsertionService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IFaceService _faceService;
        private readonly FrameGrabber<LiveCameraResult> _frameGrabber;
        private readonly CascadeClassifier _localFaceDetector;
        private readonly IOpenCvService _openCvService;
        private readonly IVisualizationService _visualizationService;

        private LiveCameraResult _currentLiveCameraResult;

        public VideoFrameAnalyzerService(IEventAggregator eventAggregator, IVisualizationService visualizationService,
            IOpenCvService openCvService, IFaceService faceService, IDataInsertionService dataInsertionService)
        {
            _frameGrabber = new FrameGrabber<LiveCameraResult>();
            _eventAggregator = eventAggregator;
            _visualizationService = visualizationService;
            _openCvService = openCvService;
            _faceService = faceService;
            _dataInsertionService = dataInsertionService;
            _localFaceDetector = _openCvService.DefaultFrontalFaceDetector();
        }

        public List<string> GetAvailableCameraList()
        {
            return LoadCameraList();
        }

        public void InitializeFrameGrabber()
        {
            SetUpListenerNewFrame();
            SetUpListenerNewResultFromApiCall();
            _openCvService.DefaultFrontalFaceDetector();
            _frameGrabber.AnalysisFunction = _faceService.FacesAnalysisFunction;
        }

        public void StartProcessing(string selectedCamera)
        {
            StartProcessingCamera(selectedCamera);
        }

        public async Task StopProcessing()
        {
            await StopProcessingCamera();
        }

        private List<string> LoadCameraList()
        {
            var numberOfCameras = _frameGrabber.GetNumCameras();
            if (numberOfCameras == 0)
            {
                //TODO: Listen from ShellViewModel for message about "No cameras found!"
            }

            var cameras = Enumerable.Range(0, numberOfCameras).Select(i => $"Camera {i + 1}");

            return cameras.ToList();
        }

        private int GetSelectedCameraIndex(string selectedCamera)
        {
            var cameraList = LoadCameraList();
            var selectedCameraIndex = cameraList.FindIndex(a => a == selectedCamera);

            var result = selectedCameraIndex;
            return result;
        }

        private void SetUpListenerNewFrame()
        {
            _frameGrabber.NewFrameProvided += (s, e) =>
            {
                var detectedFacesRectangles = _localFaceDetector.DetectMultiScale(e.Frame.Image);
                e.Frame.UserData = detectedFacesRectangles;

                Application.Current.Dispatcher.BeginInvoke((Action) (() =>
                {
                    var frameImage = e.Frame.Image.ToBitmapSource();
                    var resultImage = _visualizationService.Visualize(e.Frame, _currentLiveCameraResult);
                    _eventAggregator.PublishOnUIThread(new FrameImageProvidedEvent {FrameImage = frameImage});
                    _eventAggregator.PublishOnUIThread(new ResultImageAvailableEvent {ResultImage = resultImage});
                }));
            };
        }

        private void SetUpListenerNewResultFromApiCall()
        {
            _frameGrabber.NewResultAvailable += (s, e) =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action) (() =>
                {
                    if (e.TimedOut)
                    {
                        //TODO: MessageArea.Text = "API call timed out.";
                    }
                    else if (e.Exception != null)
                    {
                        //TODO: MessageArea.Text = "API Exception Message.";
                    }
                    else
                    {
                        _currentLiveCameraResult = e.Analysis;

                        if (_currentLiveCameraResult.Faces.Length > 0)
                        {
                            _dataInsertionService.InitializeSessionInterval();
                            var faceAttributes = _currentLiveCameraResult.Faces[0].FaceAttributes;
                            _eventAggregator.PublishOnUIThread(
                                new FaceAttributesResultEvent {FaceAttributesResult = faceAttributes});
                        }
                    }
                }));
            };
        }

        private async void StartProcessingCamera(string selectedCamera)
        {
            _faceService.InitializeFaceServiceClient();
            var analysisInterval = Settings.Default.AnalysisInterval;
            _dataInsertionService.InitializeSession(analysisInterval);
            _frameGrabber.TriggerAnalysisOnInterval(analysisInterval);
            var selectedCameraIndex = GetSelectedCameraIndex(selectedCamera);
            await _frameGrabber.StartProcessingCameraAsync(selectedCameraIndex);
        }

        private async Task StopProcessingCamera()
        {
            await _frameGrabber.StopProcessingAsync();

            _dataInsertionService.InsertSessionData();
        }
    }
}