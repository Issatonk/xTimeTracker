using xTimeTracker.Core;
using xTimeTracker.Core.Repositories;
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

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DataAccessMappingProfile>();
});

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
