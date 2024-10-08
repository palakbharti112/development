using Job_PortalApi;
using Job_PortalApi.Models;
using Job_PortalApi.Respositories;
using Job_PortalApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 ValidIssuer = builder.Configuration["Jwt:Issuer"],
                 ValidAudience = builder.Configuration["Jwt:Audience"],
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secretkey"]))
             };
         });
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IgnoreNullValues = true; // Example option
    });
builder.Services.AddAutoMapper(typeof(AutomapperProfile).Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<JobportalContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("con"));

});

builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IJobServices, JobServices>();
builder.Services.AddScoped<IProfileServices, ProfileServices>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployerOnly", policy => policy.RequireRole("Employer"));
    options.AddPolicy("JobSeekerOnly", policy => policy.RequireRole("JobSeeker"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();


app.UseAuthorization();

app.MapControllers();

app.Run();
