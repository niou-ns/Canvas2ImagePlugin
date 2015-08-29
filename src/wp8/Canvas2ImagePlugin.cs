using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System;
using System.IO;
using System.Text;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;

public class Canvas2ImagePlugin : BaseCommand
{
    public Canvas2ImagePlugin()
	{
	}

    public void saveImageDataToLibrary(string jsonArgs)
    {
        try
        {
            var options = JsonHelper.Deserialize<string[]>(jsonArgs);

            string imageData = options[0];
            byte[] imageBytes = Convert.FromBase64String(imageData);

            using (var imageStream = new MemoryStream(imageBytes))
            {
                imageStream.Seek(0, SeekOrigin.Begin);

                string fileName = String.Format("c2i_{0:yyyyMMdd_HHmmss}", DateTime.Now);
                var library = new MediaLibrary();
                var picture = library.SavePicture(fileName, imageStream);
                // Get path of saved file for social sharing
                var extraPath = MediaLibraryExtensions.GetPath(picture);

                if (picture.Name.Contains(fileName))
                {
                    DispatchCommandResult(new PluginResult(PluginResult.Status.OK, extraPath);
                }
                else
                {
                    DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR,
                        "Failed to save image: " + picture.Name));
                }
            }
        }
        catch (Exception ex)
        {
            DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, ex.Message));
        }
    }
}
