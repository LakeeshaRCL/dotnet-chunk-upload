namespace DotNetChunkUpload.DTOs.Requests.UploadAsChunksRequests;

public class UploadChunkRequest
{
    public required string uploadId {  get; set; }
    public required int chunkId {  get; set; }
    public required IFormFile chunk {  get; set; }
}