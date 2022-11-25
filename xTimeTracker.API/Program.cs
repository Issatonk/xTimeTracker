using xTimeTracker.API;
using xTimeTracker.BusinessLogic;
using xTimeTracker.Core;
using xTimeTracker.Core.Repositories;
using xTimeTracker.Core.Services;
using xTimeTracker.DataAccess.MSSQL;
using xTimeTracker.DataAccess.MSSQL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ConnectionString>(provider => new ConnectionString 
    { Connection = builder.Configuration["ConnectionStrings:DefaultConnection"]});

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DataAccessMappingProfile>();
    cfg.AddProfile<ApiMappingProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }