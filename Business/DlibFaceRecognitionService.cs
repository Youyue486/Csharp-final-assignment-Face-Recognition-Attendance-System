using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp_final_assignment_Face_Recognition_Attendance_System.Data;
using FaceRecognitionDotNet;
using OpenCvSharp;

namespace Csharp_final_assignment_Face_Recognition_Attendance_System.Business
{
    public class DlibFaceRecognitionService : IDlibFaceRecognitionService
    {
        private readonly FaceRecognition _faceRecognition;
        private readonly IUserRepository _userRepository;
        
        public DlibFaceRecognitionService(IUserRepository userRepository,string modelsPath)
        {
            
            _faceRecognition = FaceRecognition.Create(modelsPath);
            _userRepository = userRepository;
        }


        /// <summary>
        /// 检测人脸并在Mat上绘制方框,这是处理事件中的一部分，却不是事件本身
        /// </summary>
        public (Mat, FaceEncoding) DetectFace(Mat frame)
        {
            FaceEncoding? res = null;
            // 转换为Bitmap供FaceRecognition处理
            using (var bitmap = new System.Drawing.Bitmap(
                frame.Width,
                frame.Height,
                (int)frame.Step(),
                System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                frame.Data))
            using (var image = FaceRecognition.LoadImage(bitmap))
            {
                // 检测人脸位置
                var faceLocations = _faceRecognition.FaceLocations(image);
                if (faceLocations != null && faceLocations.FirstOrDefault() != null)
                {
                    var face = faceLocations.FirstOrDefault();
                    // 绘制红色方框
                    Cv2.Rectangle(
                        frame,
                        new OpenCvSharp.Point(face.Left, face.Top),
                        new OpenCvSharp.Point(face.Right, face.Bottom),
                        Scalar.Red,
                        2);
                    res = _faceRecognition.FaceEncodings(image, faceLocations.ToList()).First();
                    return (frame, res);

                }
                else
                {
                    return (frame, res);
                }
            }

        }

        /// <summary>
        /// 给定图像并返回其中人脸特征,这是处理事件中的一部分，却不是事件本身
        /// </summary>
        public (Mat, FaceEncoding) GetFaceEncodings(OpenCvSharp.Mat frame)
        {
            FaceEncoding? res = null;
            using (var bitmap = new System.Drawing.Bitmap(
                frame.Width,
                frame.Height,
                (int)frame.Step(),
                System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                frame.Data))
            using (var image = FaceRecognition.LoadImage(bitmap))
            {
                // 检测人脸位置
                var faceLocations = _faceRecognition.FaceLocations(image);
                if (faceLocations != null && faceLocations.FirstOrDefault() != null)
                {
                    var face = faceLocations.FirstOrDefault();
                    if (face == null)
                    {
                        throw new Exception("unsafe");
                    }
                    // 绘制红色方框
                    Cv2.Rectangle(
                        frame,
                        new OpenCvSharp.Point(face.Left, face.Top),
                        new OpenCvSharp.Point(face.Right, face.Bottom),
                        Scalar.Red,
                        2);
                    
                    res = _faceRecognition.FaceEncodings(image, faceLocations.ToList()).First();
                    var userName =  GetUserFrom(res).Name;
                    if (userName == null) {
                        userName = "未识别";
                    }
                    TransformationTools.AddTextToCenter(frame, userName,Scalar.Blue);
                }
            }
            return (frame,res);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="faceEncoding"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Core.Models.User ? GetUserFrom(FaceEncoding faceEncoding,double threshold=0.6)
        {
               var _registerFaceEncodings = _userRepository.GetAllUsers();
               if (_registerFaceEncodings.Count == 0 || faceEncoding == null)
               {
                   return null;
               }
               return _registerFaceEncodings
               .Select(user => new
               {
                   User=user,
                   Distance = FaceRecognition.FaceDistance(TransformationTools.BytesToFaceEncoding(user.Password), faceEncoding)
               })
               .OrderBy(x => x.Distance)
               .FirstOrDefault(x => x.Distance <= threshold)
                ?.User; 
        }
        
        public void Dispose()
        {
            _faceRecognition?.Dispose();
        }
    }


    public class TransformationTools
    {
        public static OpenCvSharp.Mat ImageToMat(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            try
            {
                // 加载图像，默认以彩色模式加载
                Mat mat = Cv2.ImRead(filePath, ImreadModes.Color);

                if (mat.Empty())
                {
                    Console.WriteLine("无法加载图像，可能文件格式不支持或文件已损坏。");
                    return null;
                }

                return mat;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载图像时出错: {ex.Message}");
                return null;
            }
        }
        public static byte[] FaceEncodingToBytes(FaceEncoding encoding)
        {
            if (encoding == null) return null;

            // 获取浮点数组
            double[] array = encoding.GetRawEncoding().ToArray();

            // 转换为字节数组
            byte[] bytes = new byte[array.Length * sizeof(float)];
            Buffer.BlockCopy(array, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        // 从 byte[] 还原 FaceEncoding
        public static FaceEncoding BytesToFaceEncoding(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;

            // 字节数组转回浮点数组
            double[] array = new double[bytes.Length / sizeof(double)];
            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
            if (array.Length != 128)
            {
                throw new ArgumentException("特征向量必须为128维");
            }
            // 创建 FaceEncoding
            return FaceRecognition.LoadFaceEncoding(array);
        }
        public static void AddTextToCenter(OpenCvSharp.Mat image, string text, Scalar color, double fontSize = 1.0, int thickness = 2)
        {
            // 计算文本尺寸
            Size textSize=Cv2.GetTextSize(text, HersheyFonts.HersheySimplex, fontSize, thickness,  out int baseline);

            // 计算文本中心点位置
            int x = (image.Width - textSize.Width) / 2;
            int y = (image.Height + textSize.Height) / 2;

            // 在图像上放置文本
            Cv2.PutText(
                img: image,
                text: text,
                org: new OpenCvSharp.Point(x, y),
                fontFace: HersheyFonts.HersheySimplex,
                fontScale: fontSize,
                color: color,
                thickness: thickness,
                lineType: LineTypes.Link8
            );
        }
    }
}
