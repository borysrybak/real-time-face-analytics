using OpenCvSharp;

namespace RealTimeFaceAnalytics.Core.Utils
{
    public class ImageEncodingParameter
    {
        /// <summary> Gets JpegQuality parameters (<see cref="ImageEncodingParam"/>) for encoding frame image. </summary>
        /// <value> Jpeg quality parameters. </value>
        public static ImageEncodingParam[] JpegParams { get; } = {new ImageEncodingParam(ImwriteFlags.JpegQuality, 100)};
    }
}