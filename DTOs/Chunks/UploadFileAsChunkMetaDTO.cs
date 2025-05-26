namespace DotNetChunkUpload.DTOs.Chunks;

public class UploadFileAsChunkMetaDTO
{
    public required string uploadId { get; set; }
    public required long fileSizeBytes {  get; set; }
    public required long chunkSizeBytes {  get; set; }
    public required string directoryPath { get; set; }
    public required string fileName { get; set; }
    
    public required string fileContentType { get; set; }
}