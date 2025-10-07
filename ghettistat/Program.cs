using GhettiBergApi.Interfaces;
using GhettiBergApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITextProcessor, GhettiTextProcessor>();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Register endpoints from separate configuration
app.ConfigureGhettiProcessingEndpoints();

app.Run();