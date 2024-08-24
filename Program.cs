using XaniAPI.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using XaniAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddAuthentication(x =>
{
    x.AddScheme<MyAuthenticationHandler>("Basic", null);
    x.DefaultAuthenticateScheme = "Basic";
    x.DefaultChallengeScheme = "Basic";
});

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

/* offer the opportunity to authorise */
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapGet("/api/feed", () => "This endpoint requires authorization")
//    .RequireAuthorization();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
