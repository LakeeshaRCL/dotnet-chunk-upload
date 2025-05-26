using System.Diagnostics.CodeAnalysis;

namespace DotNetChunkUpload.Helpers.Utils;

public class FileMetaUtils
{
    public required string name {  get; set; }
    public required string absolutePath { get; set; }

    [SetsRequiredMembers]
    public FileMetaUtils(string name, string path, string mimeType)
    {
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string guid = Guid.NewGuid().ToString();

        this.absolutePath = "uploads\\" + path.Trim('/').Replace('/','\\') + "\\";
        this.name = timestamp + guid + "_" + name.Trim('/').Replace('/','\\') +SupportUtils.GetFileExtension(mimeType);

    }
    
    
    [SetsRequiredMembers]
    public FileMetaUtils(string name, string rootDirectory, string path, string mimeType)
    {

        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string guid = Guid.NewGuid().ToString();
            
        string[] pathArray = {rootDirectory, "uploads"};
        pathArray= pathArray.Concat(path.Trim('/').Split('/')).ToArray();
            
        this.absolutePath = Path.Combine(pathArray);
        this.name = timestamp + guid + "_" + name.Trim('/').Replace('/','_') +SupportUtils.GetFileExtension(mimeType);

    }
    
    
    public string GetFullPath()
    {
        return Path.Combine(this.absolutePath, this.name);
    }

    public string GetKey()
    {
        // note: replace = as _ otherwise, combank API manger will not recognize the key correctly
        return SupportUtils.Base64Encode(this.GetFullPath()).Replace("=", "_"); 
    }
}
