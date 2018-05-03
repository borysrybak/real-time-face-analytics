# RealTimeFaceAnalytics

This sample project is an example of **analysing faces in real-time**,
by using **[Azure Cognitive Services](https://docs.microsoft.com/en-us/azure/cognitive-services/welcome)**,
mostly by **[Face API](https://docs.microsoft.com/en-us/azure/cognitive-services/Face/Overview)** service.

## Overview

### Description
Whole concept was invented up with **[Tauron](https://www.tauron.pl/dla-domu)** for a hackfest to create a **Proof of Concept** for:
- **Analysing in real-time** customer's face features in customer service points, features such as: gender, age, emotions, etc.
- Integrating devices with **cloud storage** and **cognitive services**.

We have made a decision with Tauron related to the target technology, in which the solution will be created (and with which services will be combined):
- Desktop application written in **[WPF](https://docs.microsoft.com/en-us/dotnet/framework/wpf/getting-started/introduction-to-wpf-in-vs) (C#)** and / or Python.
- **Face API, Emotion API, Computer Vision API**, to collect data, such as gender, age, emotions, number of clients, etc.
- **[Azure SQL Database](https://azure.microsoft.com/en-us/services/sql-database/)**, for the purpose of storing collected data.
- **Azure SQL Database** with **[DirectQuery](https://docs.microsoft.com/en-us/power-bi/desktop-use-directquery)**, to receive direct data for **[Power BI](https://powerbi.microsoft.com/en-us/)** reports.

We achieved:
- [x] WPF Application with
  - [x] [Caliburn.Micro](https://caliburnmicro.com/),
  - [x] [Haar Cascade](https://docs.opencv.org/3.4.1/d7/d8b/tutorial_py_face_detection.html) local detector,
  - [x] [Frame grabber for real-time analysis](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/vision-api-how-to-topics/howtoanalyzevideo_vision),
  - [x] [Entity Framework](https://docs.microsoft.com/en-us/ef/) databinding.
- [x] Cognitive Services Face API service call.
- [x] Azure SQL Database server connection.
- [x] Power BI with DirectQuery data connection.
- [ ] Connect with other Cloud Services.
- [ ] Use data with Machine Learning services.

### Solution Architecture Overview:
![tauron_diagram](assets/tauron_diagram.png)

## Table of Contents

- [Project](#realtimefaceanalytics)  
- [Overview](#overview)  
  - [Description](#description)  
  - [Solution Architecture Overview](#solution-architecture-overview)  
- [Table of Contents](#table-of-contents)  
- [Prerequisites](#prerequisites)  
  - [Accounts](#accounts)  
    - [Microsoft Account](#microsoft-account)  
    - [Azure Subscription](#azure-subscription)  
    - [Azure SQL Database](#azure-sql-database)
    - [Cognitive Services](#cognitive-services)
  - [Software](#software)  
    - [Tools & Libraries](#tools-libraries)
    - [Power Bi Desktop](#power-bi-desktop)
  - [Devices](#devices)  
    - [PC](#pc)
    - [Web Camera](#web-camera) 
- [Usage](#usage)
  - [Application Setup](#application-setup)
  - [Application Run](#application-run)
  - [Application Highlights](#application-highlights)
  - [Database Upload](#database-upload)
  - [Database Check](#database-check)
  - [DirectQuery with Power Bi](#directquery-with-power-bi)
- [Credits](#credits)
- [References](#references)

## Prerequisites

To successfully run the application and use it, we need to prepare some important things - if we do not do this, it may turn out that the application will not work properly.

Be sure that you have these... :joy:

### Accounts

First, we need accounts that allow us to use specific services or applications.

#### Microsoft Account

This account is needed in case of using Visual Studio and Azure Portal.
When you start Visual studio for the first time, you're asked to sign in and provide some basic registration information.
You should choose a Microsoft account or a work or school account that best represents you.
If you don't have any of these accounts, you can [create a Microsoft account for free](https://account.microsoft.com/account).

#### Azure Subscription

Be sure that you have a valid [Azure Subscription](https://azure.microsoft.com/en-us/account/) with funds for such things as Azure SQL Database and/or Cognitive Services.
If not, there are several offers that can help you.

- [Azure Free Account](https://azure.microsoft.com/en-us/offers/ms-azr-0044p/)
- [Azure for Students](https://azure.microsoft.com/en-us/free/students/)
- [Azure Pass](https://www.microsoftazurepass.com/Home/HowTo)

#### Azure SQL Database

You need a place where you will collect your data from real-time face analysis.
Adress, database name, user_id and password needed!
In that case you should create a new resource from your Azure Portal, which will be **Azure SQL Database**.

- [Create an Azure SQL database in the Azure portal](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-get-started-portal)

#### Cognitive Services

We need Face API key and endpoint address.
It is possible to use free plan (**F0**) of Cognitive Services,
however for better experience and results I recommend you to create a new plan (**S0**),
from higher pricing tier - [pricing](https://azure.microsoft.com/en-us/pricing/details/cognitive-services/face-api/).

- [Create a Cognitive Services APIs account in the Azure portal](https://docs.microsoft.com/en-us/azure/cognitive-services/cognitive-services-apis-create-account)

### Software

Of course it is possible to run this application without Visual Studio,
however in that case you should consider downloading proper DLL's, setting-up dependencies and references,
configurating application settings and more! :skull:

#### Tools & Libraries

Visual Studio installed, and proper frameworks to run **Windows Application** (WPF), **.NET Framework 4.5.2**.

- [Visual Studio IDE](https://www.visualstudio.com/vs/)

#### Power Bi Desktop

Power BI is a suite of business analytics tools that deliver insights throughout your organization.
We will use it to get insights about our data from face analysis.

- [Power BI Desktop](https://powerbi.microsoft.com/en-us/desktop/)

### Devices

#### PC

<img src="assets/obvious.jpg" alt="It's so obvious!" width="150"/>

#### Web Camera

For sure providing frames in the applications is a must,
and without camera you won't be able to achieve any results.

I used **Creative Live! Cam Chat HD VF0790**. I believe laptop or any USB-like camera will be enough.

<img src="assets/camera.jpg" alt="Creative Live! Cam Chat HD VF0790" width="150"/>

## Usage

First things first! Do not skip things that you should do first,
in that case you need to properly set-up the application.
Prepare your early created resources, connection strings, credentials, etc.

### Application Setup

1. Fill in **Cognitive Services keys and endpoints**.
(Don't worry if you miss that, you will be able to add these keys and endpoint during application runtime). 

- [RealTimeFaceAnalytics.Core/Properties/Settings.settings](RealTimeFaceAnalytics.Core/Properties/Settings.settings)

![keysendpoints](assets/keysendpoints_settings.PNG)

Face API service is enough, just put:
- **FaceAPIKey**
- **FaceAPIHost**
+ ***AnalysisInterval**, you can change it for different frame analysis frequency.

2. Configure **Connection String** in application configuration file
(you will find that particular block between line 36 and 40) with your **Azure SQL Database** credentials.

- [RealTimeFaceAnalytics.WPF/App.config](RealTimeFaceAnalytics.WPF/App.config)

```xml
<connectionStrings>
    <add name="FaceAnalyticsContext"
        connectionString="Server=tcp:{database_server_address};Database={database_name};User ID={user};Password={password};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
        providerName="System.Data.SqlClient" />
</connectionStrings>
```

Replace *connectionString* values. For example:

```
connectionString="Server=tcp:myserver.database.windows.net;Database=mydatabase_db;User ID=srvadmin;Password=*************;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

3. **Add rule** of your client IP address for Azure SQL Database server in **Firewalls and virtual networks** tab - [read more](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-vnet-service-endpoint-rule-overview).

### Application Run

1. In Visual Studio set **RealTimeFaceAnalytics.WPF** project as a StartUp Project.
2. Be sure to add missing references and packages.
3. Rebuild
4. Run


- *Before you start to analyse frames from your camera stream, you will be able to configure your settings again:*
![appstart](assets/app_start.PNG)

- *After clicking Save, choosing Camera and clicking Analyze, you will get a preview camera stream (without local detector) and main stream (with local detector):*
![apprun](assets/app_run.PNG)

### Application Highlights

1. WPF Application with MVVM Architecture (Caliburn.Micro).
Caliburn Micro solved my problem - as a small yet powerful framework, designed for building WPF applications.
Great support for MVVM patterns enabled me to build a solution quickly.

Most important thing I setup for Caliburn support was [Bootstrapper.cs](RealtimeFaceAnalytics.WPF/Bootstrapper.cs) file - with simple IoC container configuration:
```csharp
public class Bootstrapper : BootstrapperBase
{
    private SimpleContainer _container;
    ...
    protected override void Configure()
    {
        ...
        _container = new SimpleContainer();
        _container.Singleton<IWindowManager, WindowManager>();
        ...
        _container.PerRequest<ShellViewModel>();
    }
    ...
}
```

2. Starting Video Frame Analyzer Service process.


    2.1. Loading cameras:

    - in [ShellViewModel.cs](RealTimeFaceAnalytics.Core/ViewModels/ShellViewModel.cs)
    ```csharp
    public List<string> CameraList
    {
        get
        {
            var availableCameraList = _videoFrameAnalyzerService.GetAvailableCameraList();
            ...
        }
    }
    ```
    - in [VideoFrameAnalyzerService.cs](RealTimeFaceAnalytics.Core/Services/VideoFrameAnalyzerService.cs)
    ```csharp
    public List<string> GetAvailableCameraList()
    {
        return LoadCameraList();
    }
    ...
    private List<string> LoadCameraList()
    {
        var numberOfCameras = _frameGrabber.GetNumCameras();
        ...
        var cameras = Enumerable.Range(0, numberOfCameras).Select(i => $"Camera {i + 1}");
        return cameras.ToList();
    }
    ```
    - in [FrameGrabber.cs](VideoFrameAnalyzer/FrameGrabber.cs)
    ```csharp
    protected int NumCameras = -1;
    ...
    public int GetNumCameras()
    {
        if (NumCameras != -1) return NumCameras;
        ...
        while (NumCameras < 100)
        {
            using (var vc = VideoCapture.FromCamera(NumCameras))
            {
                if (vc.IsOpened())
                    ++NumCameras;
                else
                    break;
            }
        }
        return NumCameras;
    }
    ```

    2.2. Start processing with selected camera:

    - in [ShellViewModel.cs](RealTimeFaceAnalytics.Core/ViewModels/ShellViewModel.cs)
    ```csharp
    public void StartAnalyze()
    {
        ...
        _videoFrameAnalyzerService.StartProcessing(_selectedCameraList);
        ...
    }
    ```
    - in [VideoFrameAnalyzerService.cs](RealTimeFaceAnalytics.Core/Services/VideoFrameAnalyzerService.cs)
    ```csharp
    public void StartProcessing(string selectedCamera)
    {
        StartProcessingCamera(selectedCamera);
    }
    ...
    private async void StartProcessingCamera(string selectedCamera)
    {
        ...
        var selectedCameraIndex = GetSelectedCameraIndex(selectedCamera);
        await _frameGrabber.StartProcessingCameraAsync(selectedCameraIndex);
    }
    ```
    - in [FrameGrabber.cs](VideoFrameAnalyzer/FrameGrabber.cs)
    ```csharp
    public async Task StartProcessingCameraAsync(int cameraIndex = 0, double overrideFps = 0)
    {
        ...
        await StopProcessingAsync().ConfigureAwait(false);
        ...
        StartProcessing(TimeSpan.FromSeconds(1 / Fps), () => DateTime.Now);
        ...
    }
    ```

    2.3. Handling events with results of frames and data (read more about **EventAggregator** in Caliburn.Micro):

    - creating events
      - [FaceAttributesResultEvent.cs](RealTimeFaceAnalytics.Core/Events/FaceAttributesResultEvent.cs)
      - [FrameImageProvidedEvent.cs](RealTimeFaceAnalytics.Core/Events/FrameImageProvidedEvent.cs)
      - [ResultImageAvailableEvent.cs](RealTimeFaceAnalytics.Core/Events/ResultImageAvailableEvent.cs)

    ```csharp
    using Microsoft.ProjectOxford.Face.Contract;
    ...    
        public class FaceAttributesResultEvent
        {
            public FaceAttributes FaceAttributesResult;
        }
    ...
    using System.Windows.Media.Imaging;
    ...
        public class FrameImageProvidedEvent
        {
            public BitmapSource FrameImage;
        }
        ...
        public class ResultImageAvailableEvent
        {
            public BitmapSource ResultImage;
        }
    ...
    ```
    - setup listeners and publishing events in [VideoFrameAnalyzerService.cs](RealTimeFaceAnalytics.Core/Services/VideoFrameAnalyzerService.cs)
    ```csharp
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
    ...
    private void SetUpListenerNewResultFromApiCall()
    {
        _frameGrabber.NewResultAvailable += (s, e) =>
        {
            Application.Current.Dispatcher.BeginInvoke((Action) (() =>
            {
                if (e.TimedOut)
                ...
                else if (e.Exception != null)
                ...
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
    ```
    - handling events in [ShellViewModel.cs](RealTimeFaceAnalytics.Core/ViewModels/ShellViewModel.cs)   
    (*Updating Properties in ViewModel and Invoking Services*)
    ```csharp
    public class ShellViewModel : Screen, IHandle<FrameImageProvidedEvent>, IHandle<ResultImageAvailableEvent>,
        IHandle<FaceAttributesResultEvent>
    {
        ...
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
        ...
        public void Handle(FrameImageProvidedEvent message)
        {
            FrameImage = message.FrameImage;
        }
        ...
        public void Handle(ResultImageAvailableEvent message)
        {
            ResultImage = message.ResultImage;
        }
    }
    ```

### Database Upload

### Database Check

### DirectQuery with Power Bi

## Credits
## References

