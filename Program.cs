
using DotNetCardsServer.Auth;
using DotNetCardsServer.Interfaces;
using DotNetCardsServer.Middlewares;
using DotNetCardsServer.Services.Cards;
using DotNetCardsServer.Services.Data;
using DotNetCardsServer.Services.Users;
using DotNetCardsServer.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DotNetCardsServer
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                return MongoDbService.CreateMongoClient(configuration);
            });

            builder.Services.AddCors(options =>{
                options.AddPolicy("myCorsPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:5500")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {

                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = "CardsServer",
                                    ValidAudience = "CardReactFront",
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtHelper.secretKey))
                                };
                            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBeAdmin", policy => policy.RequireClaim("type", "Admin"));  
                options.AddPolicy("MustBeBusinessOrAdmin", policy => policy.RequireClaim("type", "Business","Admin"));

            });

            builder.Services.AddScoped<ICardsService, CardsServiceMongoDb>();
            builder.Services.AddScoped<IUsersService, UsersServiceMongoDb>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseCors("myCorsPolicy");
            app.UseHttpsRedirection();

            app.UseMiddleware<ReqResLoggerMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}