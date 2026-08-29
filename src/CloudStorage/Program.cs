using Asp.Versioning;
using CloudStorage.Extensions;
using CloudStorage.HostedServices;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.AddOptions(builder.Configuration);

// Infrastructure
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddMinioStorage(builder.Configuration);

// Application
builder.Services.AddApplicationServices();
builder.Services.AddValidation();

// Api
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();