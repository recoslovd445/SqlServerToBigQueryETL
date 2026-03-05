using Google.Cloud.BigQuery.V2;
using SqlServerToBigQueryETL.EndPoint;
using SqlServerToBigQueryETL.Services;
using SqlServerToBigQueryETL.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the 
builder.Services.AddSingleton(BigQueryClient.Create("project-1f07756d-2ea6-4ff5-9f2"));

builder.Services.AddTransient<ISqlService, SqlService>();
builder.Services.AddTransient<IBigQueryService, BigQueryService>();

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

app.UseHttpsRedirection();

app.MapEtlEndPoint();

app.Run();
