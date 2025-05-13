using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using OpenCvSharp.WpfExtensions;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class PunchCardModelView : ObservableObject
    {
        private readonly INavigationService _navigator;
        private readonly ICameraService _cameraService;
        private readonly IDlibFaceRecognitionService _faceRecognitionService;
        private readonly IAttendanceService _attendanceService;
        [ObservableProperty]
        private BitmapSource _cameraImage;
        private int times = 0;
        private int frequency = 10;
        public PunchCardModelView(INavigationService navigationService,
            ICameraService cameraService,IDlibFaceRecognitionService dlibFaceRecognitionService
            ,IAttendanceService attendanceService
            )
        {
            _attendanceService = attendanceService;
            _navigator = navigationService;
            _cameraService = cameraService;
            _faceRecognitionService = dlibFaceRecognitionService;
        }

        [RelayCommand]
        public void Start()
        {
            _cameraService.start();
            _cameraService.FrameProcessed += OnFrameProcessed;
        }

        [RelayCommand] 
        public void Exit() 
        {
            _cameraService?.stop();
            _cameraService.FrameProcessed -= OnFrameProcessed;
            _navigator.NavigateTo<HomeViewModel>();
        }
        private void OnFrameProcessed(OpenCvSharp.Mat frame)
        {
            OpenCvSharp.Mat? mat = null;
            if (times % frequency == 0)
            {
                var res = _faceRecognitionService.DetectFace(frame);
                mat = res.Item1;
                //检测到人脸；
                if (res.Item2 != null)
                {
                    
                    var tar = res.Item2;
                    var user = _faceRecognitionService.GetUserFrom(tar);
                    if (user != null && user.userRole == Core.UserRole.Normal)
                    {
                        _attendanceService.ClockIn(user);
                    }
                }
            }
            else
            {
                mat = frame;
            }
            times += 1;
            CameraImage = BitmapSourceConverter.ToBitmapSource(mat);
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    CameraImage.Source = BitmapSourceConverter.ToBitmapSource(mat);
            //});
        }
    }
}
