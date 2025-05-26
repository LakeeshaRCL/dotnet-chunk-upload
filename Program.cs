using DotNetChunkUpload.Services;
using DotNetChunkUpload.Services.ChunkUploadManager;
using DotNetChunkUpload.Services.StorageService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddSingleton<IChunkUploadManager, ChunkUploadManager>(di => new ChunkUploadManager());

// cors policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p =>
        p.AllowAnyOrigin().AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapControllers();

await app.RunAsync();