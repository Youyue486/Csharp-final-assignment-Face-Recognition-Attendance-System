using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Business;
using OpenCvSharp.WpfExtensions;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.ViewModel
{
    public partial class EmployeeLoginViewModel : ObservableObject
    {
        private readonly INavigationService _navigator;
        private readonly ICameraService _cameraService;
        private readonly IDlibFaceRecognitionService _faceRecognitionService;
        [ObservableProperty]
        private BitmapSource _cameraImage;
        private int times=0;
        private int frequency = 10;
        public EmployeeLoginViewModel(INavigationService navigator,IDlibFaceRecognitionService dlibFaceRecognitionService,ICameraService cameraService)
        {
            _navigator = navigator;
            _cameraService = cameraService;
            _faceRecognitionService=dlibFaceRecognitionService;
        }
        /// <summary>
        /// 
        /// </summary>
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
            _cameraService.FrameProcessed-= OnFrameProcessed;
            _navigator.NavigateTo<LoginViewModel>();
        }
        private void OnFrameProcessed(OpenCvSharp.Mat frame)
        {
            OpenCvSharp.Mat mat=null;
            if (times % frequency == 0)
            {
                var res = _faceRecognitionService.DetectFace(frame);
                mat = res.Item1;
                //检测到人脸；
                if (res.Item2 != null)
                {
                    
                    var tar = res.Item2;
                    //var user=_faceRecognitionService.GetUserFrom(tar);
                    //if (user != null && user.userRole == Core.UserRole.Admin)
                    //{
                    //    _navigator.NavigateTo<AdminViewModel>();
                    //}

                    //为调试开的后门
                    if (true)
                    {
                        _navigator.NavigateTo<AdminViewModel>();
                    }
                }
            }
            else
            {
                mat = frame;
            }
            times += 1;
            
            CameraImage = BitmapSourceConverter.ToBitmapSource(mat);
        }
    }
}
