using RestaurantAPI_n;
using RestaurantAPI_n.Entities;
using AutoMapper;
using System.Reflection;
using RestaurantAPI_n.Interfaces;
using RestaurantAPI_n.Services;
using NLog;
using NLog.Web;
using RestaurantAPI_n.Middleware;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI_n.Models;
using RestaurantAPI_n.Models.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantAPI_n.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

// Add services to the container. - ConfigureServices

var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasNationality", optionBuilder => 
            optionBuilder.RequireClaim("Nationality", "German", "Polish"));
    options.AddPolicy("AtLeast20", optionBuilder => 
            optionBuilder.AddRequirements(new MinimumAgeRequirement(20)));
    options.AddPolicy("CreatedAtLeast2Restaurants", 
            optionBuilder => optionBuilder.AddRequirements(new CreatedMultipleRestauranstRequirement(2)));
});

builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CreatedMultipleRestauranstRequirementHandler>();

builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddDbContext<RestaurantDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDbConnection"))
    );

builder.Services.AddScoped<RestaurantSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();


builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policyBuilder =>
    policyBuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["AllowedOrigins"])
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline. - Configure
app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontEndClient");

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
seeder.Seed();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
});

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
