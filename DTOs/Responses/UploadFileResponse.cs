using System.Diagnostics.CodeAnalysis;

namespace DotNetChunkUpload.DTOs.Responses;

public class UploadFileResponse
{
    public required string key { get; set; }

    public UploadFileResponse()
    {
        // default
    }

    [SetsRequiredMembers]
    public UploadFileResponse(string key)
    {   
        this.key = key;
    }
}