using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ProjectOxford.Face.Contract;
using OpenCvSharp;
using RealTimeFaceAnalytics.Core.Enums;
using RealTimeFaceAnalytics.Core.Interfaces;

namespace RealTimeFaceAnalytics.Core.Services
{
    public class OpenCvService : IOpenCvService
    {
        private readonly CascadeClassifier _cascadeClassifier;

        public OpenCvService()
        {
            _cascadeClassifier = new CascadeClassifier();
        }

        public CascadeClassifier DefaultFrontalFaceDetector()
        {
            return GetLoadedClassifier(HaarCascade.FrontalFaceAlt2);
        }

        public void MatchAndReplaceFaces(Face[] faces, Rect[] clientRects)
        {
            MatchAndReplaceFaceRectangles(faces, clientRects);
        }

        private CascadeClassifier GetLoadedClassifier(HaarCascade haarCascade)
        {
            var result = _cascadeClassifier;

            _cascadeClassifier.Load(GetHaarCascadeDataPath(haarCascade));

            return result;
        }

        private string GetHaarCascadeDataPath(HaarCascade haarCascade)
        {
            var result = string.Empty;

            switch (haarCascade)
            {
                case HaarCascade.Eye:
                    break;
                case HaarCascade.EyeTreeEyeglasses:
                    break;
                case HaarCascade.FrontalCatFace:
                    break;
                case HaarCascade.FrontalCatFaceExtended:
                    break;
                case HaarCascade.FrontalFaceAlt:
                    result = "Data/haarcascade_frontalface_alt.xml";
                    break;
                case HaarCascade.FrontalFaceAlt2:
                    result = "Data/haarcascade_frontalface_alt2.xml";
                    break;
                case HaarCascade.FrontalFaceAltTree:
                    result = "Data/haarcascade_frontalface_alt_tree.xml";
                    break;
                case HaarCascade.FrontalFaceDefault:
                    result = "Data/haarcascade_frontalface_default.xml";
                    break;
                case HaarCascade.FulBody:
                    break;
                case HaarCascade.LeftEye2Splits:
                    break;
                case HaarCascade.LowerBody:
                    break;
                case HaarCascade.ProfileFace:
                    break;
                case HaarCascade.RightEye2Splits:
                    break;
                case HaarCascade.Smile:
                    break;
                case HaarCascade.UpperBody:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(haarCascade), haarCascade, null);
            }

            return result;
        }

        private static void MatchAndReplaceFaceRectangles(IReadOnlyCollection<Face> faces,
            IReadOnlyCollection<Rect> clientRects)
        {
            var sortedResultFaces = faces
                .OrderBy(f => f.FaceRectangle.Left + 0.5 * f.FaceRectangle.Width)
                .ToArray();

            var sortedClientRects = clientRects
                .OrderBy(r => r.Left + 0.5 * r.Width)
                .ToArray();

            for (var i = 0; i < Math.Min(faces.Count, clientRects.Count); i++)
            {
                var r = sortedClientRects[i];
                sortedResultFaces[i].FaceRectangle =
                    new FaceRectangle {Left = r.Left, Top = r.Top, Width = r.Width, Height = r.Height};
            }
        }
    }
}