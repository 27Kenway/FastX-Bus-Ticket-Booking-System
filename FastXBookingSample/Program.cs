using AutoMapper;
using FastXBookingSample.Helper;
using FastXBookingSample.Interface;
using FastXBookingSample.Models;
using FastXBookingSample.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
//Hey its Bharat this side editing the Project
namespace FastXBookingSample
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

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            builder.Services.AddTransient<IBusRepository,BusRepository>();
            builder.Services.AddTransient<IBoardingPointRepository,BoardingPointRepository>();
            builder.Services.AddTransient<IDroppingPointRepository,DroppingPointRepository>();
            builder.Services.AddTransient<IAmenityRepository,AmenityRepository>();
            builder.Services.AddTransient<IRouteRepository,RouteRepository>();
            builder.Services.AddTransient<IUserRepository,UserRepository>();
            builder.Services.AddTransient<IBusSeatRepository,BusSeatRepository>();
            builder.Services.AddTransient<IBusDepartureRepository,BusDepartureRepository>();
            builder.Services.AddTransient<IBookingRepository,BookingRepository>();
            builder.Services.AddTransient<ISeatRepository,SeatRepository>();
            builder.Services.AddTransient<IBusOperatorRepository,BusOperatorRepository>();
            builder.Services.AddTransient<IAdminRepository,AdminRepository>();
            builder.Services.AddTransient<IBookingHistoryRepository,BookingHistoryRepository>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddDbContext<BookingContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:myconnection"]));
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Logging.AddLog4Net();
            builder.Services.AddHostedService<SeatCleanupService>();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
