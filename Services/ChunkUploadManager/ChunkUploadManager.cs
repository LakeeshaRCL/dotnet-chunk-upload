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
    

    /// <summary>
    /// This can be used to generate a unique upload id
    /// </summary>
    /// <returns></returns>
    private static string GenerateUploadId()
    {
        return $"{Guid.NewGuid().ToString()}-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}";
    }
    
    
    /// <summary>
    /// This method can be used to add a new chunk upload to the manager
    /// </summary>
    /// <param name="fileSizeBytes">final file size in bytes</param>
    /// <param name="chunkSizeBytes">chunk size in bytes</param>
    /// <param name="directoryPath">directory path to be uploaded</param>
    /// <param name="fileName">file name to be stored</param>
    /// <param name="fileContentType">file content type</param>
    /// <returns>newly added meta-data identifier as uploadId</returns>
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

    
    /// <summary>
    /// This method can be used to get uploading file meta-data by uploadId
    /// </summary>
    /// <param name="uploadId">meta-data upload id</param>
    /// <returns>UploadFileAsChunkMetaDTO instance if it is available, null otherwise</returns>
    public UploadFileAsChunkMetaDTO? GetUploadingFileAsChunkMeta(string uploadId)
    {
        return this.uploadingFilesAsChunks.FirstOrDefault(x => x.uploadId == uploadId);
    }

    /// <summary>
    /// This method can be used to remove uploading file meta-data by uploadId
    /// </summary>
    /// <param name="uploadId">meta-data upload id</param>
    /// <returns></returns>
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
    
    /// <summary>
    /// This method can be used to get all uploading files meta-data
    /// </summary>
    /// <returns></returns>
    public List<UploadFileAsChunkMetaDTO> GetAllUploadingFilesAsChunkMeta()
    {
        return this.uploadingFilesAsChunks;   
    }
}