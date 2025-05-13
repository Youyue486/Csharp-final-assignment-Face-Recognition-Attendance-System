using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    public interface ICameraService:IDisposable
    {
        // 定义委托和事件
        public delegate void FrameProcessedHandler(OpenCvSharp.Mat frame);
        public event FrameProcessedHandler FrameProcessed;
        public void start();
        public void stop();
    }
}
