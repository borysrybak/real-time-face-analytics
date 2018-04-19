using OpenCvSharp;

namespace RealTimeFaceAnalytics.Core.Utils
{
    public class ImageEncodingParameter
    {
        public static ImageEncodingParam[] JpegParams { get; } = { new ImageEncodingParam(ImwriteFlags.JpegQuality, 60) };
    }
}
