using GhettiControlService.Interfaces;
using GhettiControlService.Services;
using GhettiControlService.Middleware;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Memory cache for request caching
builder.Services.AddMemoryCache();

// Http client for text processing service
builder.Services.AddHttpClient<ITextProcessingClient, TextProcessingClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["TextProcessingService:BaseUrl"] 
        ?? "http://localhost:5001");
});

// Queue service (swappable implementation)
builder.Services.AddSingleton<IQueueService, RabbitMQService>();

// Business services
builder.Services.AddScoped<IAuthService, ApiKeyAuthService>();
builder.Services.AddScoped<ITextBatchingService, TextBatchingService>();
builder.Services.AddScoped<IJobOrchestrationService, JobOrchestrationService>();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Custom auth middleware
app.UseMiddleware<ApiKeyAuthMiddleware>();

// Register endpoints
app.ConfigureControlEndpoints();

app.Run();