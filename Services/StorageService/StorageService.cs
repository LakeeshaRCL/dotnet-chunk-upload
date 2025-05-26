using System.Text.RegularExpressions;
using DotNetChunkUpload.DTOs.Chunks;
using DotNetChunkUpload.DTOs.Requests;
using DotNetChunkUpload.DTOs.Requests.UploadAsChunksRequests;
using DotNetChunkUpload.DTOs.Responses;
using DotNetChunkUpload.Helpers.Utils;
using DotNetChunkUpload.Services.ChunkUploadManager;
using Microsoft.AspNetCore.Mvc;

namespace DotNetChunkUpload.Services.StorageService;

/// <summary>
/// This service is responsible for handling file upload and chunk upload
/// </summary>
public class StorageService : IStorageService
{
    private readonly string rootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
    private readonly string rootTempDirectory;
    private readonly IChunkUploadManager chunkUploadManager;


    public StorageService(IChunkUploadManager chunkUploadManager)
    {
        this.chunkUploadManager = chunkUploadManager;
        rootTempDirectory =  Path.Combine(rootDirectory, "temp");
    }
    
    /// <summary>
    /// This method can be used to upload a file in a regular way
    /// </summary>
    /// <param name="uploadFormFileRequest"></param>
    /// <returns></returns>
    public ActionResult<UploadFileResponse> UploadFile(UploadFileRequest uploadFormFileRequest)
    {

        try
        {
            // define absolute file name of the requested file
            FileMetaUtils fileMeta = new FileMetaUtils(
                uploadFormFileRequest.name, 
                this.rootDirectory, 
                uploadFormFileRequest.directoryPath,
                uploadFormFileRequest.file.ContentType);

            // create a directory if not exists
            EnsureDirectoryExists(fileMeta.absolutePath);

            // upload the file 
            System.IO.File.WriteAllBytes(fileMeta.GetFullPath(), SupportUtils.GetBytes(uploadFormFileRequest.file.OpenReadStream()));

            // return path if successfully upload
            return  new UploadFileResponse(fileMeta.GetKey());
               
        }
        catch (Exception ex)
        {
            Console.WriteLine("StorageService - UploadFile - Exception : " + ex.Message);
            return new StatusCodeResult(500);
        }
    }


    /// <summary>
    /// This method can be used to upload a file in chunks
    /// </summary>
    /// <param name="uploadChunkRequest">UploadChunkRequest</param>
    /// <returns></returns>
    public ActionResult<UploadFileResponse> UploadChunk(UploadChunkRequest uploadChunkRequest)
    {
        try
        {
            // query upload manager and get uploading file meta-data
            UploadFileAsChunkMetaDTO? uploadFileAsChunkMeta = 
                chunkUploadManager.GetUploadingFileAsChunkMeta(uploadChunkRequest.uploadId);

            // check uploading file meta exists (if not, means bad request or already uploaded)
            if (uploadFileAsChunkMeta == null) return new StatusCodeResult(StatusCodes.Status400BadRequest);
            
            // get available chunks
            FileMetaUtils fileMeta = new FileMetaUtils(
                $"{uploadFileAsChunkMeta.uploadId}_chunk_{uploadChunkRequest.chunkId}", 
                this.rootTempDirectory, 
                "chunks",
                uploadChunkRequest.chunk.ContentType);

            // create a directory if not exists
            EnsureDirectoryExists(fileMeta.absolutePath);
            
            // upload current chunk
            System.IO.File.WriteAllBytes(fileMeta.GetFullPath(), 
                SupportUtils.GetBytes(uploadChunkRequest.chunk.OpenReadStream()));

            // get uploaded chunks to order by chunk number
            List<string> uploadedChunks = Directory
                .GetFiles(fileMeta.absolutePath, $"*_{uploadFileAsChunkMeta.uploadId}_chunk_*")
                .OrderBy(path =>
                {
                    // Extract chunk number using regex
                    Match match = Regex.Match(path, @"_chunk_(\d+)");
                    return match.Success ? int.Parse(match.Groups[1].Value) : int.MaxValue;
                })
                .ToList();

            // calculate total chunks that should be uploaded
            int totalChunks = (int)Math
                .Ceiling((double)uploadFileAsChunkMeta.fileSizeBytes / uploadFileAsChunkMeta.chunkSizeBytes);

            // check if all chunks are uploaded
            if (uploadedChunks.Count == totalChunks)
            {
                // merge chunks into a final file
                FileMetaUtils mergeFileMeta =new FileMetaUtils(
                    uploadFileAsChunkMeta.fileName, 
                    this.rootDirectory, 
                    uploadFileAsChunkMeta.directoryPath,
                    uploadFileAsChunkMeta.fileContentType);
                
                // create a directory if not exists
                EnsureDirectoryExists(mergeFileMeta.absolutePath);
                
                using(FileStream finalStream = new FileStream(mergeFileMeta.GetFullPath(), FileMode.CreateNew))
                    foreach (string chunk in uploadedChunks)
                    {
                        using FileStream chunkStream = new FileStream(chunk, FileMode.Open);
                        chunkStream.CopyTo(finalStream);
                        chunkStream.Close();
                        
                        // delete chunk after merge
                        File.Delete(chunk);
                    }
                // removing file meta from the manager
                chunkUploadManager.RemoveUploadingFileAsChunkMeta(uploadChunkRequest.uploadId);
                return new UploadFileResponse(fileMeta.GetKey());
            }
            else
            {
                // allow uploading more chunks
                return new StatusCodeResult(StatusCodes.Status202Accepted);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("StorageService - UploadChunk - Exception : " + ex.Message);
            return new StatusCodeResult(500);
        }
    }


    /// <summary>
    /// This method can be used to initiate a file upload in a chunk session
    /// </summary>
    /// <param name="request">InitChunkUploadRequest</param>
    /// <returns></returns>
    public ActionResult<string> InitUploadFileAsChunks(InitChunkUploadRequest request)
    {
        try
        {
            return chunkUploadManager.AddChunkUpload(
                request.fileSizeBytes, 
                request.chunkSizeBytes, 
                request.directoryPath, 
                request.name,
                request.contentType);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Error";
        }
    }


    private static void EnsureDirectoryExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}

