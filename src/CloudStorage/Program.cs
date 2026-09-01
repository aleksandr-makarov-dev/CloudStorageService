using Asp.Versioning;
using CloudStorage.Extensions;
using CloudStorage.Middlewares;

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
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        var httpContext = ctx.HttpContext;
        var problem = ctx.ProblemDetails;

        problem.Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}";

        problem.Extensions["traceId"] = httpContext.TraceIdentifier;
        problem.Extensions["timestamp"] = DateTimeOffset.UtcNow;
    };
});

builder.Services.AddCors(builder.Configuration);
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
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

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();