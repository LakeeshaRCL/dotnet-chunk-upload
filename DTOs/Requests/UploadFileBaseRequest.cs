using System.ComponentModel.DataAnnotations;

namespace DotNetChunkUpload.DTOs.Requests;

public class UploadFileBaseRequest
{
    [Required, RegularExpression(@"^[^\s*$%&@!~]+$", ErrorMessage = "No special characters and spaces are supported")]
    public required string name { get; set; }

    [Required, RegularExpression(@"^[^\s*$%&@!~]+$", ErrorMessage = "No special characters and spaces are supported")]
    public required string directoryPath { get; set; }
    
}