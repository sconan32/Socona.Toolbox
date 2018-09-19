using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Socona.ImVehicle.Infrastructure.Extensions
{
    public static class ViewModelExtensions
    {
        public static IHostingEnvironment HostingEnvironment { get; set; }
        private static string[] acceptableExt = new[] { ".png", ".bmp", ".jpg", ".jpeg", "gif", };
        public async static Task<byte[]> GetPictureByteArray(this IFormFile formFile)
        {
            if (formFile != null)
            {
                if (acceptableExt.Contains(Path.GetExtension(formFile?.FileName)?.ToLower()))
                {
                    MemoryStream stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);
                    stream.Close();
                    return stream.ToArray();
                }
            }
            return null;
        }
        public async static Task<byte[]> GetPictureByteArray(this IFormFile formFile, string waterMark)
        {
            if (formFile != null)
            {
                if (acceptableExt.Contains(Path.GetExtension(formFile?.FileName)?.ToLower()))
                {
                    MemoryStream stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);
                    stream.Close();
                    var outStream = stream;//WaterMark.Mark(stream, waterMark,HostingEnvironment.WebRootPath+ "/fonts/FZYouH_510M.TTF");
                    return outStream.ToArray();
                }
            }
            return null;
        }

    }
}
