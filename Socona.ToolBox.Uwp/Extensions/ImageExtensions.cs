using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Socona.ToolBox.Uwp.Extensions
{
    public static class ImageExtensions
    {

        public static async Task<string> ToBase64StringsAsync(this SoftwareBitmap image)
        {

            using (InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream())
            {
                BitmapEncoder pngEncoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, ras);
                pngEncoder.SetSoftwareBitmap(image);

                await pngEncoder.FlushAsync();

                IBuffer buffer = new global::Windows.Storage.Streams.Buffer((uint)ras.Size);
                await ras.ReadAsync(buffer, (uint)ras.Size, InputStreamOptions.None);
                string base64 = CryptographicBuffer.EncodeToBase64String(buffer);
                return base64;
            }
        }

        public static async Task<SoftwareBitmap> ToSoftwareBitmapAsync(this string base64String)
        {
            var buffer = CryptographicBuffer.DecodeFromBase64String(base64String);
            using (InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream())
            {
                await ras.WriteAsync(buffer);
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(ras);
                return await decoder.GetSoftwareBitmapAsync();
            }
        }
        public static async Task<SoftwareBitmap> LoadSoftwareBitmapAsync(this StorageFile file)
        {
            using (IRandomAccessStream ras = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(ras);
                return await decoder.GetSoftwareBitmapAsync();
            }
        }
        public static async Task SaveSoftwareBitmapToFile(this SoftwareBitmap softwareBitmap, StorageFile outputFile)
        {
            var propertySet = new BitmapPropertySet();
            var qualityValue = new BitmapTypedValue(
                1.0, //Maximum
                global::Windows.Foundation.PropertyType.Single);
            propertySet.Add("ImageQuality", qualityValue);

            using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {

                // Create an encoder with the desired format
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream, propertySet);

                // Set the software bitmap
                encoder.SetSoftwareBitmap(softwareBitmap);

                // Set additional encoding parameters, if needed
                encoder.BitmapTransform.ScaledWidth = (uint)softwareBitmap.PixelWidth;
                encoder.BitmapTransform.ScaledHeight = (uint)softwareBitmap.PixelHeight;
                //encoder.BitmapTransform.Rotation = Windows.Graphics.Imaging.BitmapRotation.Clockwise90Degrees;
                //encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.IsThumbnailGenerated = true;

                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception err)
                {
                    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
                    switch (err.HResult)
                    {
                        case WINCODEC_ERR_UNSUPPORTEDOPERATION:
                            // If the encoder does not support writing a thumbnail, then try again
                            // but disable thumbnail generation.
                            encoder.IsThumbnailGenerated = false;
                            break;
                        default:
                            throw;
                    }
                }

                if (encoder.IsThumbnailGenerated == false)
                {
                    await encoder.FlushAsync();
                }


            }
        }

        public static async Task<SoftwareBitmapSource> GetSoftwareBitmapSourceAsync(this SoftwareBitmap bitmap)
        {
            var srcAvatar = new SoftwareBitmapSource();

            if (bitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
           bitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
            {
                bitmap = SoftwareBitmap.Convert(bitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }
            await srcAvatar.SetBitmapAsync(bitmap);
            return srcAvatar;
        }
    }
}
