using DotNetChunkUpload.DTOs.Chunks;
using DotNetChunkUpload.DTOs.Requests;
using DotNetChunkUpload.DTOs.Requests.UploadAsChunksRequests;
using DotNetChunkUpload.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DotNetChunkUpload.Services;

public interface IStorageService
{
    ActionResult<UploadFileResponse> UploadFile(UploadFileRequest uploadFormFileRequest);
    ActionResult<UploadFileResponse> UploadChunk(UploadChunkRequest uploadChunkRequest);
    ActionResult<string> InitUploadFileAsChunks(InitChunkUploadRequest request);
}