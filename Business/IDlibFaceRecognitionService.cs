using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceRecognitionDotNet;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    public interface IDlibFaceRecognitionService:IDisposable
    {
        public (OpenCvSharp.Mat, FaceEncoding) GetFaceEncodings(OpenCvSharp.Mat frame);
        public (OpenCvSharp.Mat, FaceEncoding) DetectFace(OpenCvSharp.Mat frame);
        public Core.Models.User? GetUserFrom(FaceEncoding faceEncoding, double threshold = 0.6);
    }
}
