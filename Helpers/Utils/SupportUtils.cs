using System.Text;
using DotNetChunkUpload.Attributes;

namespace DotNetChunkUpload.Helpers.Utils;

public class SupportUtils
{
    public static string GetFileExtension([MimeType] string mimeType)
    {
        string extension = ".temp";

        if (mimeType == "image/jpeg")
        {
            extension = ".jpg";
        }
        else if (mimeType == "image/png")
        {
            extension = ".png";
        }
        else if (mimeType == "video/mp4")
        {
            extension = ".mp4";
        }
        else if (mimeType == "application/pdf")
        {
            extension = ".pdf";
        }
        else if (mimeType == "application/msword")
        {
            extension = ".doc";
        }

        return extension;
    }
    
    
    public static byte[] GetBytes(string base64) {
        return Convert.FromBase64String(base64);
    }

    public static byte[] GetBytes(MemoryStream ms)
    {
        return ms.ToArray();
    }
        
    public static byte[] GetBytes(Stream stream)
    {
        using MemoryStream ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    public static string GetBase64(byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }


    public static string Base64Encode(string text){
        return GetBase64(Encoding.UTF8.GetBytes(text));
    }


    public static string Base64Decode(string base64) {
        return Encoding.UTF8.GetString(GetBytes(base64));
    }
}
