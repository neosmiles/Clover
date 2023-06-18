using Api.Data;
using Api.Data.Seeder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("ServiceStringMsSql")));
//Add health check endpoint
builder.Services.AddHealthChecks();
//Database connection data, WARNING: DO NEVER USER SA USER, this is just to showcase 
//var server = Environment.GetEnvironmentVariable("DBServer");
//var port = Environment.GetEnvironmentVariable("DBPort");
//var user = Environment.GetEnvironmentVariable("DBUser");
//var password = Environment.GetEnvironmentVariable("DBPassword");
//var Database = Environment.GetEnvironmentVariable("Database");

//Database connection for docker
//string connectionString = $"Server={server},{port};Initial Catalog={Database};User ID={user};Password={password}; TrustServerCertificate=true";
//builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionString));

var app = builder.Build();

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<DataContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
    await Seed.SeedIris(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
