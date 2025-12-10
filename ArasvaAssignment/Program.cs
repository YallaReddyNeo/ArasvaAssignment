using Arasva.Core;
using Arasva.Core.Interface;
using Arasva.Core.Repository;
using Arasva.Core.Services.Implementation;
using Arasva.Core.Services.Interfaces;
using Arasva.Data.Data;
using Arasva.Data.Repository.Implementation;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add all repositories  
builder.Services.AddTransient<IReadURLContentRepository, ReadURLContentRepository>(); 
builder.Services.AddSingleton<IRateLimitRepository, RateLimitRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddScoped<IBorrowingHistoryRepository, BorrowingHistoryRepository>();
builder.Services.AddScoped<IBorrowingService, BorrowingService>();


//#RateLimit
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("RateLimitPolicy", httpContext =>
    {
        //Used to read for Request
        // Attempt to get the 'userId' from the query string
        var userId = httpContext.Request.Query["userId"].FirstOrDefault();

        // Fallback: if 'userId' query param is missing, use the connection ID or handle as needed
        if (string.IsNullOrEmpty(userId))
        {
            // You might choose a default, or fall back to previous logic
            userId = "anonymous";
        }

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: userId,
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 2,
                Window = TimeSpan.FromSeconds(60),
                QueueLimit = 0 // Reject immediately if limit is exceeded
            });
    });

   //options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; 
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;   // <- custom error code
        context.HttpContext.Response.ContentType = "application/json";

        var response = new GlobalResponse
        { 
            success = false,   
            errorMessage = "Rate limit exceeded. Please try again later."
        };

        await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken: token);
    };
});


// Configure the database connection
var connectionString = builder.Configuration.GetConnectionString("OnboardDB");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


var app = builder.Build();


// --- Automatic Database Creation and Migration ---
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    Console.WriteLine("Attempting to migrate/create database...");
//    // This command automatically creates the database if it doesn't exist 
//    // and runs pending migrations.
//    dbContext.Database.Migrate();
//    Console.WriteLine("Database ready.");
//}

// Configure the HTTP request pipeline.
app.UseSwagger(option =>
{
    option.RouteTemplate = "api/v1/arasva/{documentname}/swagger.json";
});

app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint("/api/v1/arasva/v1/swagger.json", "Arasva API V1");
    option.RoutePrefix = "api/v1/arasva";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Enable the rate limiting middleware #RateLimit
app.UseRateLimiter();


app.MapControllers();

app.Run();
