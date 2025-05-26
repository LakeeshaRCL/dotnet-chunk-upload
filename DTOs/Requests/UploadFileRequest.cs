namespace DotNetChunkUpload.DTOs.Requests;

public class UploadFileRequest : UploadFileBaseRequest
{
    public required IFormFile file {  get; set; }
}