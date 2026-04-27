using ApiOAuthEmpleados.Helpers;
using ApiSecurityExam.Data;
using ApiSecurityExam.Helpers;
using ApiSecurityExam.Repositories;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using MvcCoreAzureStorage.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<HelperUsuarioToken>();
HelperCryptography.Initialize(builder.Configuration);

HelperActionOAuthService helper =
    new HelperActionOAuthService(builder.Configuration);

builder.Services.AddSingleton<HelperActionOAuthService>(helper);

builder.Services.AddAuthentication(helper.GetAuthenticationSchema())
    .AddJwtBearer(helper.GetJWTBearerOptions());
// Add services to the container.
string connectionString =
    builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddTransient<RepositoryLibros>();
builder.Services.AddDbContext<LibrosContext>
    (options => options.UseSqlServer(connectionString));

string azureKeys = builder.Configuration.GetValue<string>
    ("AzureKeys:StorageAccount");

BlobServiceClient blobServiceClient =
    new BlobServiceClient(azureKeys);
builder.Services.AddTransient<BlobServiceClient>
    (x => blobServiceClient);

builder.Services.AddTransient<ServiceStorageBlobs>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar");
    return Task.CompletedTask;
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
