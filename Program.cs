using XaniAPI.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using XaniAPI;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.DependencyInjection;

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

/* offer the opportunity to authorise */
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    { 
        Title = "Xani API", 
        Version = "v1", 
        Description = "Simple social media micro-blogging site much in the style of twitter." ,
        Contact = new OpenApiContact() 
        { 
            Name = "Jonathan Campbell Jones", 
            Email = "jonathan.cj@ntlworld.com", 
            Url = new Uri("https://www.bbc.com") 
        },
        License = new OpenApiLicense
        {
            Name = "Employee API LICX",
            Url = new Uri("https://example.com/license"),
        }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Xani API V1");
        options.DocumentTitle = "Xani API";
        //options.InjectJavascript("/assets/xanicustom.js", "text/javascript");
        options.InjectStylesheet("/assets/xanicustom.css");
    });
}

//app.MapGet("/api/feed", () => "This endpoint requires authorization")
//    .RequireAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();



//http://localhost/Xani/api/feed?u_id=3