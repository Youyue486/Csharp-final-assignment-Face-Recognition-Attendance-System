using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    public class CameraService : ICameraService
    {
        private DispatcherTimer _timer;
        private OpenCvSharp.VideoCapture _capture;
        private OpenCvSharp.Mat _currentFrame;

        public event ICameraService.FrameProcessedHandler FrameProcessed;

        public CameraService()
        {
            if (_capture == null)
            {
                // 初始化摄像头
                _capture = new OpenCvSharp.VideoCapture(0);  // 0为默认摄像头
                _capture.FrameWidth = 640;       // 调整分辨率提升性能
                _capture.FrameHeight = 480;

                // 定时器刷新摄像头画面
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(30);

            }  // 约30FPS
        }
        public void Dispose()
        {
            _timer.Stop();
            _capture.Release();
        }
        private void ProcessFrame(object sender, EventArgs e)
        {
            _currentFrame = new OpenCvSharp.Mat();
            if (!_capture.Read(_currentFrame)) return;

            // 触发事件（执行外部逻辑）
            FrameProcessed?.Invoke(_currentFrame.Clone());  // 传递克隆避免资源冲突

            // 注意：如果需要显示原始画面，在此处添加渲染代码
        }
        public void start()
        {
            
            _timer.Tick += ProcessFrame;
            _timer.Start();
        }

        public void stop()
        {
            _timer.Tick -= ProcessFrame;
        }
    
}
}
