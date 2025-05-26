using DotNetChunkUpload.DTOs.Chunks;

namespace DotNetChunkUpload.Services.ChunkUploadManager;

public interface IChunkUploadManager
{
    string AddChunkUpload( long fileSizeBytes, long chunkSizeBytes, string directoryPath, string fileName, string fileContentType);
    UploadFileAsChunkMetaDTO? GetUploadingFileAsChunkMeta(string uploadId);
    bool RemoveUploadingFileAsChunkMeta(string uploadId);
    List<UploadFileAsChunkMetaDTO> GetAllUploadingFilesAsChunkMeta();
}