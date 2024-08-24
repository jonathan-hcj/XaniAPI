using XaniAPI.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* supply the database connections to the controllers */
var connectionString = builder.Configuration.GetConnectionString("General");
builder.Services.AddDbContext<PostDbContext>(option =>
    option.UseSqlServer(connectionString));
builder.Services.AddDbContext<LikeDbContext>(option =>
    option.UseSqlServer(connectionString));
builder.Services.AddDbContext<UserDbContext>(option =>
    option.UseSqlServer(connectionString));
builder.Services.AddDbContext<FeedDbContext>(option =>
    option.UseSqlServer(connectionString));
builder.Services.AddDbContext<RepostDbContext>(option =>
    option.UseSqlServer(connectionString));




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
