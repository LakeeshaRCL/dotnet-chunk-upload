using DotNetChunkUpload.DTOs.Chunks;
using DotNetChunkUpload.Helpers.Utils;

namespace DotNetChunkUpload.Services.ChunkUploadManager;

public class ChunkUploadManager : IChunkUploadManager
{
    private List<UploadFileAsChunkMetaDTO> uploadingFilesAsChunks;

    public ChunkUploadManager()
    {
        this.uploadingFilesAsChunks = new List<UploadFileAsChunkMetaDTO>();
    }
    

    private static string GenerateUploadId()
    {
        return $"{Guid.NewGuid().ToString()}-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}";
    }
    

    public string AddChunkUpload(long fileSizeBytes, long chunkSizeBytes, 
        string directoryPath, string fileName, string fileContentType)
    {
        try
        {
            UploadFileAsChunkMetaDTO uploadFileAsChunkMeta = new UploadFileAsChunkMetaDTO
            {
                uploadId = GenerateUploadId(),
                fileSizeBytes = fileSizeBytes,
                chunkSizeBytes = chunkSizeBytes,
                directoryPath = directoryPath,
                fileName = fileName,
                fileContentType = fileContentType
            };
            
            this.uploadingFilesAsChunks.Add(uploadFileAsChunkMeta);
            return uploadFileAsChunkMeta.uploadId;
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception : AddChunkUpload - " + e.Message );
            throw;
        }
    }

    public UploadFileAsChunkMetaDTO? GetUploadingFileAsChunkMeta(string uploadId)
    {
        return this.uploadingFilesAsChunks.FirstOrDefault(x => x.uploadId == uploadId);
    }

    public bool RemoveUploadingFileAsChunkMeta(string uploadId)
    {
        try
        {
            UploadFileAsChunkMetaDTO? itemToRemove = this.GetUploadingFileAsChunkMeta(uploadId);
            return itemToRemove != null && this.uploadingFilesAsChunks.Remove(itemToRemove);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public List<UploadFileAsChunkMetaDTO> GetAllUploadingFilesAsChunkMeta()
    {
        return this.uploadingFilesAsChunks;   
    }
}