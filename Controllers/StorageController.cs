using DotNetChunkUpload.DTOs.Chunks;
using DotNetChunkUpload.DTOs.Requests;
using DotNetChunkUpload.DTOs.Requests.UploadAsChunksRequests;
using DotNetChunkUpload.DTOs.Responses;
using DotNetChunkUpload.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetChunkUpload.Controllers
{
    [Route("api/storage")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService storageService;

        public StorageController(IStorageService storageService)
        {
            this.storageService = storageService;
        }


        [HttpPost("upload")]
        public ActionResult<UploadFileResponse> UploadFile([FromForm]UploadFileRequest request)
        {
            return storageService.UploadFile(request);
        }
        
        
        [HttpPost("upload/chunks/init")]
        public ActionResult<string> InitChunkUpload(InitChunkUploadRequest request)
        {
            return storageService.InitUploadFileAsChunks(request);
        }

        
        [HttpPost("upload/chunks")]
        public ActionResult<UploadFileResponse> UploadChunk([FromForm]UploadChunkRequest request)
        {
            return storageService.UploadChunk(request);
        }
    }
}
