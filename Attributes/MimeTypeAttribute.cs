using System.ComponentModel.DataAnnotations;

namespace DotNetChunkUpload.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter , AllowMultiple = false)]
public class MimeTypeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if(value is not null && value is string)
        {
            List<string> validMimes = new List<string>
            {
                "image/jpeg",
                "image/png",
                "video/mp4",
                "application/pdf",
                "application/msword"
            };

            if (validMimes.Contains(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    public override string FormatErrorMessage(string name)
    {
        return "Invalid mime type";
    }

}
