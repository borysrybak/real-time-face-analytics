using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Vision.Contract;
using OpenCvSharp.Extensions;
using RealTimeFaceAnalytics.Core.Interfaces;
using RealTimeFaceAnalytics.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VideoFrameAnalyzer;

namespace RealTimeFaceAnalytics.Core.Services
{
    public class VisualizationService : IVisualizationService
    {
        private readonly SolidColorBrush s_lineBrush = new SolidColorBrush(new System.Windows.Media.Color { R = 255, G = 0, B = 0, A = 255 });
        private readonly Typeface s_typeface = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

        private readonly IOpenCVService _openCVService;
        private readonly IFaceService _faceService;
        private readonly IEmotionService _emotionService;

        public VisualizationService(IOpenCVService openCVService, IFaceService faceService, IEmotionService emotionService)
        {
            _openCVService = openCVService;
            _faceService = faceService;
            _emotionService = emotionService;
        }

        public BitmapSource DrawTags(BitmapSource baseImage, Tag[] tags)
        {
            return DrawTagsContext(baseImage, tags);
        }

        public BitmapSource DrawFaces(BitmapSource baseImage, Microsoft.ProjectOxford.Face.Contract.Face[] faces, EmotionScores[] emotionScores, string[] celebName)
        {
            return DrawFacesContext(baseImage, faces, emotionScores, celebName);
        }

        public BitmapSource Visualize(VideoFrame videoFrame, LiveCameraResult currentLiveCameraResult)
        {
            return VisualizeResult(videoFrame, currentLiveCameraResult);
        }

        public System.Windows.Shapes.Rectangle ComposeRectangleBar(EmotionScores emotionScores)
        {
            return ComposeRectangleBarFromEmotions(emotionScores);
        }

        public List<System.Windows.Shapes.Rectangle> MixHairColor(HairColor[] hairColor)
        {
            return MixHairColorAsRectangles(hairColor);
        }

        private BitmapSource DrawOverlay(BitmapSource baseImage, Action<DrawingContext, double> drawAction)
        {
            RenderTargetBitmap result;

            var annotationScale = baseImage.PixelHeight / 320;
            var visual = new DrawingVisual();
            var drawingContext = visual.RenderOpen();

            drawingContext.DrawImage(baseImage, new Rect(0, 0, baseImage.Width, baseImage.Height));
            drawAction(drawingContext, annotationScale);
            drawingContext.Close();

            var outputBitmap = new RenderTargetBitmap(
                baseImage.PixelWidth, baseImage.PixelHeight,
                baseImage.DpiX, baseImage.DpiY, PixelFormats.Pbgra32);
            outputBitmap.Render(visual);
            result = outputBitmap;

            return result;
        }
        private BitmapSource DrawTagsContext(BitmapSource baseImage, Tag[] tags)
        {
            if (tags == null)
            {
                return baseImage;
            }

            Action<DrawingContext, double> drawAction = (drawingContext, annotationScale) =>
            {
                double y = 0;
                foreach (var tag in tags)
                {
                    var ft = new FormattedText(tag.Name,
                        CultureInfo.CurrentCulture, FlowDirection.LeftToRight, s_typeface,
                        42 * annotationScale, Brushes.Black);
                    var geom = ft.BuildGeometry(new System.Windows.Point(10 * annotationScale, y));
                    drawingContext.DrawGeometry(s_lineBrush, new Pen(Brushes.Black, 2 * annotationScale), geom);
                    y += 42 * annotationScale;
                }
            };

            return DrawOverlay(baseImage, drawAction);
        }
        private BitmapSource DrawFacesContext(BitmapSource baseImage, Microsoft.ProjectOxford.Face.Contract.Face[] faces, EmotionScores[] emotionScores, string[] celebName)
        {
            if (faces == null)
            {
                return baseImage;
            }

            Action<DrawingContext, double> drawAction = (drawingContext, annotationScale) =>
            {
                for (int i = 0; i < faces.Length; i++)
                {
                    var face = faces[i];
                    if (face.FaceRectangle == null) { continue; }

                    var faceRect = new Rect(
                        face.FaceRectangle.Left, face.FaceRectangle.Top,
                        face.FaceRectangle.Width, face.FaceRectangle.Height);
                    string text = "";

                    if (face.FaceAttributes != null)
                    {
                        text += _faceService.SummarizeFaceAttributes(face.FaceAttributes);
                    }

                    if (emotionScores?[i] != null)
                    {
                        text += _emotionService.SummarizeEmotionScores(emotionScores[i]);
                    }

                    if (celebName?[i] != null)
                    {
                        text += celebName[i];
                    }

                    faceRect.Inflate(6 * annotationScale, 6 * annotationScale);

                    var lineThickness = 4 * annotationScale;

                    drawingContext.DrawRectangle(
                        Brushes.Transparent,
                        new Pen(s_lineBrush, lineThickness),
                        faceRect);

                    if (text != "")
                    {
                        var ft = new FormattedText(text,
                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight, s_typeface,
                            16 * annotationScale, Brushes.Black);

                        var pad = 3 * annotationScale;

                        var ypad = pad;
                        var xpad = pad + 4 * annotationScale;
                        var origin = new System.Windows.Point(
                            faceRect.Left + xpad - lineThickness / 2,
                            faceRect.Top - ft.Height - ypad + lineThickness / 2);
                        var rect = ft.BuildHighlightGeometry(origin).GetRenderBounds(null);
                        rect.Inflate(xpad, ypad);

                        drawingContext.DrawRectangle(s_lineBrush, null, rect);
                        drawingContext.DrawText(ft, origin);
                    }
                }
            };

            return DrawOverlay(baseImage, drawAction);
        }
        private BitmapSource DrawRectangleContext(BitmapSource baseImage, Microsoft.ProjectOxford.Face.Contract.Face[] faces)
        {
            if (faces == null)
            {
                return baseImage;
            }

            Action<DrawingContext, double> drawAction = (drawingContext, annotationScale) =>
            {
                for (int i = 0; i < faces.Length; i++)
                {
                    var face = faces[i];
                    if (face.FaceRectangle == null) { continue; }

                    var faceRect = new Rect(
                        face.FaceRectangle.Left, face.FaceRectangle.Top,
                        face.FaceRectangle.Width, face.FaceRectangle.Height);

                    faceRect.Inflate(6 * annotationScale, 6 * annotationScale);

                    var lineThickness = 4 * annotationScale;

                    drawingContext.DrawRectangle(
                        Brushes.Transparent,
                        new Pen(s_lineBrush, lineThickness),
                        faceRect);
                }
            };

            return DrawOverlay(baseImage, drawAction);
        }
        private BitmapSource VisualizeResult(VideoFrame videoFrame, LiveCameraResult currentLiveCameraResult)
        {
            var result = videoFrame.Image.ToBitmapSource();
            if (currentLiveCameraResult != null)
            {
                var clientFaces = (OpenCvSharp.Rect[])videoFrame.UserData;
                if (clientFaces != null && currentLiveCameraResult.Faces != null)
                {
                    var faces = currentLiveCameraResult.Faces;
                    _openCVService.MatchAndReplaceFaces(faces, clientFaces);
                    result = DrawRectangleContext(result, faces);
                }
            }

            return result;
        }
        private System.Windows.Shapes.Rectangle ComposeRectangleBarFromEmotions(EmotionScores emotionScores)
        {
            var result = new System.Windows.Shapes.Rectangle();

            var emotionRankedList = emotionScores.ToRankedList();
            var gradientStopsCollection = new GradientStopCollection();
            var occuringEmotions = new List<KeyValuePair<string, float>>();
            foreach (var emotion in emotionRankedList.Reverse())
            {
                if (emotion.Value > 0.0) { occuringEmotions.Add(emotion); }
            }
            var occuringEmotionsCount = occuringEmotions.Count;
            var previousValue = 0.0f;
            for (int i = 0; i < occuringEmotionsCount; i++)
            {
                var emotion = occuringEmotions[i];
                var emotionColor = GetEmotionColor(emotion.Key);
                var emotionValue = emotion.Value;

                if (i == 0 || i == occuringEmotionsCount - 1)
                {
                    if (i == 0) { previousValue = emotionValue; }
                    var gradientStop = new GradientStop(emotionColor, previousValue);
                    gradientStopsCollection.Add(gradientStop);
                }
                else
                {
                    var gradientStopStartPoint = new GradientStop(emotionColor, previousValue);
                    gradientStopsCollection.Add(gradientStopStartPoint);
                    previousValue += emotionValue;
                    var gradientStopEndPoint = new GradientStop(emotionColor, previousValue);
                    gradientStopsCollection.Add(gradientStopEndPoint);
                }
            }
            var linearGradientBrush = new LinearGradientBrush(gradientStopsCollection, new System.Windows.Point(0.0, 0.0), new System.Windows.Point(0.0, 1.0));
            var composedBar = new System.Windows.Shapes.Rectangle();
            composedBar.Fill = linearGradientBrush;
            composedBar.Width = 5.0;
            composedBar.Margin = new Thickness(1.0, 0, 1.0, 0);
            result = composedBar;

            return result;
        }
        private System.Windows.Media.Color GetEmotionColor(string emotionName)
        {
            var result = new System.Windows.Media.Color();

            switch (emotionName)
            {
                case "Anger":
                    result = Colors.Red;
                    break;
                case "Contempt":
                    result = Colors.Orange;
                    break;
                case "Disgust":
                    result = Colors.Indigo;
                    break;
                case "Fear":
                    result = Colors.Violet;
                    break;
                case "Happiness":
                    result = Colors.Green;
                    break;
                case "Neutral":
                    result = Colors.Yellow;
                    break;
                case "Sadness":
                    result = Colors.Black;
                    break;
                case "Surprise":
                    result = Colors.Blue;
                    break;
                default:
                    result = Colors.Transparent;
                    break;
            }

            return result;
        }
        private List<System.Windows.Shapes.Rectangle> MixHairColorAsRectangles(HairColor[] hairColors)
        {
            var result = new List<System.Windows.Shapes.Rectangle>();

            var descendingConfidenceHairColors = hairColors.OrderByDescending(i => i.Confidence);
            var firstRectangle = true;
            foreach (var hairColor in descendingConfidenceHairColors)
            {
                var rectangle = new System.Windows.Shapes.Rectangle();
                rectangle.Height = 25.0;
                if (!firstRectangle) { rectangle.Margin = new Thickness(0.0, -25.0, 0.0, 0.0); }
                rectangle.Opacity = hairColor.Confidence;
                var mixedHairColor = GetHairColor(hairColor.Color);
                rectangle.Fill = new SolidColorBrush(mixedHairColor);
                result.Add(rectangle);
                firstRectangle = false;
            }

            return result;
        }
        private System.Windows.Media.Color GetHairColor(HairColorType hairColorType)
        {
            var result = new System.Windows.Media.Color();

            switch (hairColorType)
            {
                case HairColorType.Black:
                    result = Colors.Black;
                    break;
                case HairColorType.Blond:
                    result = Colors.Goldenrod;
                    break;
                case HairColorType.Brown:
                    result = Colors.Brown;
                    break;
                case HairColorType.Gray:
                    result = Colors.Gray;
                    break;
                case HairColorType.Red:
                    result = Colors.Red;
                    break;
                case HairColorType.White:
                    result = Colors.White;
                    break;
                default:
                    result = Colors.Transparent;
                    break;
            }

            return result;
        }
    }
}
