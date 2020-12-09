
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using server.Models;

namespace server.Recipients
{
    public class ScreenMirror : IRecipient
    {
        public string Resource => "take-screenshot";

		private byte[] takeScreenShot()
		{
			var bmp = new Bitmap(1920 / 2, 1080 / 2);
			using (var g = Graphics.FromImage(bmp))
			{
				g.CopyFromScreen(0,0,0,0, new Size(bmp.Width, bmp.Height));
				using (var stream = new MemoryStream())
				{
					bmp.Save(stream, ImageFormat.Png);
					return stream.ToArray();
				}
			}                 
		}

        public void HandleDelivery(Envelope message)
        {
			var screen = takeScreenShot();
			var txtImg = Convert.ToBase64String(screen);
			var complete = $"data:image/png;base64,{txtImg}";
			var response = $"{Resource}|{complete}";
			message.SendResponse(response);
        }
    }
}