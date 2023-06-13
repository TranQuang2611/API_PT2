using API_PT2;
using API_PT2.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Add DI
        builder.Services.AddSingleton(typeof(BookStoreContext));
        builder.Services.Configure<AppSetttings>(builder.Configuration.GetSection("AppSettings"));
        var secretKey = builder.Configuration["AppSettings:SecretKey"];
        var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
            {
                // tự cấp token nên không cần
                ValidateIssuer = false,
                ValidateAudience = false,
               
                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyByte),
                ClockSkew = TimeSpan.Zero
            }); 

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}