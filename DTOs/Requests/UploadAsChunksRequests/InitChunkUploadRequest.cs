namespace DotNetChunkUpload.DTOs.Requests.UploadAsChunksRequests;

public class InitChunkUploadRequest : UploadFileBaseRequest
{
    public required long fileSizeBytes {  get; set; }
    public required long chunkSizeBytes {  get; set; }
    
    public required string contentType {  get; set; }
}