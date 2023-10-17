using ImageMagick;
using Services_Layer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Layer.Service
{
    public class CheckImageService : ICheckImageService
    {
        public bool IsImage(string filePath)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://localhost:7019"); // Thay thế bằng địa chỉ cơ sở của bạn
                    byte[] imageBytes = httpClient.GetByteArrayAsync(filePath).Result;

                    // Kiểm tra xem tệp tải xuống có phải là hình ảnh không
                    using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                    {
                        using (MagickImage image = new MagickImage(memoryStream))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
