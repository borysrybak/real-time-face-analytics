using Caliburn.Micro;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Services;
using RealTimeFaceAnalytics.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RealTimeFaceAnalytics.WPF
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var config = new TypeMappingConfiguration
            {
                DefaultSubNamespaceForViews = "RealTimeFaceAnalytics.WPF.Views",
                DefaultSubNamespaceForViewModels = "RealTimeFaceAnalytics.Core.ViewModels"
            };
            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);

            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IComputerVisionService, ComputerVisionService>();
            _container.Singleton<IEmotionService, EmotionService>();
            _container.Singleton<IFaceService, FaceService>();
            _container.Singleton<IOpenCvService, OpenCvService>();
            _container.Singleton<IVideoFrameAnalyzerService, VideoFrameAnalyzerService>();
            _container.Singleton<IVisualizationService, VisualizationService>();
            _container.Singleton<IDataInsertionService, DataInsertionService>();

            _container.PerRequest<ShellViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
