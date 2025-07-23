using RRCodeTest.Server.DB;
using RRCodeTest.Server.Models;
using RRCodeTest.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace RRCodeTest.Server;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    if (builder.Environment.IsDevelopment())
    {
      builder.Services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        // the JWT security scheme config for Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.Http,
          Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
          }
        });
      });
    }


    // Cors policy
    builder.Services.AddCors(options =>
    {
      options.AddPolicy("AllowAngularApp", policy =>
      {
        policy.WithOrigins(
          "http://localhost:4200",
          "https://localhost:4200",
          "https://rrcodetest.azurewebsites.net",
          "https://rrcodetestnetlify.netlify.app"
        ).AllowAnyHeader().AllowAnyMethod();
      });
    });


    // Database and Identity
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DevelopmentConnection")));
    builder.Services
      .AddIdentity<User, IdentityRole>(options =>
      {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.User.RequireUniqueEmail = true;
      })
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders();


    // Custom services
    builder.Services.AddScoped<IAuthServices, AuthServices>();
    builder.Services.AddScoped<ITokenServices, TokenServices>();
    builder.Services.AddScoped<IBookServices, BookServices>();


    // JWT Authentication Configuration
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"];

    builder.Services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? "")),
        ClockSkew = TimeSpan.Zero
      };
    });


    var app = builder.Build();

    // Adding cors mw as early as possible in the pipeline
    app.UseCors("AllowAngularApp");


    // database init
    using (var scope = app.Services.CreateScope())
    {
      var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
      context.Database.EnsureCreated();
    }

    // Swagger
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();


    app.MapFallback(async context =>
    {
      context.Response.ContentType = "text/html";
      await context.Response.WriteAsync(@"
        <!DOCTYPE html>
        <html lang=""en"">
          <head>
              <meta charset=""UTF-8"">
              <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
              <title>API Endpoints</title>
          </head>
          <body>
              <h1>API overview</h1>
              <br/>
              <p>base domain: https://rrcodetest.azurewebsites.net</p>
              <br/>
              <p>/api</p>
              <p>&nbsp;&nbsp;/auth</p>
              <p>&nbsp;&nbsp;&nbsp;&nbsp;/register [post]</p>
              <p>&nbsp;&nbsp;&nbsp;&nbsp;/login [post]</p>
              <p>&nbsp;&nbsp;&nbsp;&nbsp;/refresh [post]</p>
              <p>&nbsp;&nbsp;&nbsp;&nbsp;/logout [post]</p>
              <br/>
              <p>&nbsp;&nbsp;/books [get] [protected]</p>
              <p>&nbsp;&nbsp;/books [post] [protected]</p>
              <p>&nbsp;&nbsp;&nbsp;&nbsp;/{id} [put] [protected]</p>
              <p>&nbsp;&nbsp;&nbsp;&nbsp;/{id} [delete] [protected]</p>
              <br/>
              <p>&nbsp;&nbsp;/users/profile [get][protected]</p>
              <br/>
              <p>&nbsp;&nbsp;/dev</p>
              <p>&nbsp;&nbsp;&nbsp;&nbsp;/users [get]</p>
              <p>&nbsp;&nbsp;&nbsp;&nbsp;/books [get]</p>
          </body>
        </html>
      ");
    });

    app.Run();
  }
}
