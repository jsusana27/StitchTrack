using CrochetBusinessAPI.Data;
using Microsoft.EntityFrameworkCore;
using CrochetBusinessAPI.Services;
using CrochetBusinessAPI.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Context Configuration
// Add DbContext service with the connection string (from appsettings.json or another config source)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CrochetDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Dependency Injection (DI) for Repositories and Services

// Repository registrations
builder.Services.AddScoped<YarnRepository>();
builder.Services.AddScoped<StuffingRepository>();
builder.Services.AddScoped<SafetyEyeRepository>();
builder.Services.AddScoped<OrderProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<FinishedProductMaterialRepository>();
builder.Services.AddScoped<FinishedProductRepository>();
builder.Services.AddScoped<CustomerPurchaseRepository>();
builder.Services.AddScoped<CustomerRepository>();

// Service registrations
builder.Services.AddScoped<YarnService>();
builder.Services.AddScoped<StuffingService>();
builder.Services.AddScoped<SafetyEyeService>();
builder.Services.AddScoped<OrderProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<FinishedProductMaterialService>();
builder.Services.AddScoped<FinishedProductService>();
builder.Services.AddScoped<CustomerPurchaseService>();
builder.Services.AddScoped<CustomerService>();

// 3. CORS Configuration
// Add CORS policy BEFORE calling builder.Build()
// Replace with your actual front-end URL if deploying in production
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policyBuilder =>
                      {
                          policyBuilder.WithOrigins("http://localhost:3000")  // Replace with actual front-end URL
                                       .AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .AllowCredentials(); // Enable if using cookies or authentication
                      });
});

// 4. JSON Serialization Configuration to handle cyclic references
// This avoids the circular reference issue in serialized entities and ignores null values when serializing
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Ignore cycles instead of preserving
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Ignore nulls
    });

// 5. Swagger configuration (for API documentation and testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 6. Enable CORS in middleware (use the policy created above)
app.UseCors(MyAllowSpecificOrigins);

// 7. HTTP request pipeline configuration
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI in development mode
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // Redirect HTTP to HTTPS
app.UseAuthorization();      // Enable Authorization middleware

// 8. Map all controller routes
app.MapControllers();

await app.RunAsync();