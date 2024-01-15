
using DotNetCardsServer.Middlewares;
using DotNetCardsServer.Services.Data;
using DotNetCardsServer.Utils;

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
            builder.Services.AddSingleton(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                return MongoDbService.CreateMongoClient(configuration);
            });

            builder.Services.AddCors(options =>{
                options.AddPolicy("myCorsPolicy", policy =>
                {
                    policy.WithOrigins("http://www.someurl.com", "http://127.0.0.1:5500")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseCors("myCorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ReqResLoggerMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}