using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CrossLang.ApplicationCore;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.Infrastructure;
using CrossLang.Infrastructure.Database;
using CrossLang.API.Middlewares;
using Newtonsoft.Json.Serialization;
using System.Text;
using CrossLang.Authentication;
using CrossLang.Authentication.JWT;
using Microsoft.Extensions.Options;

namespace CrossLang.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //           .AllowAnyMethod()
            //           .AllowAnyHeader();
            //}));

            //services
            //   .Configure<JWTConfiguration>(Configuration)
            //   .AddSingleton(sp => sp.GetRequiredService<IOptions<JWTConfiguration>>().Value);

            //services.AddSingleton<IConfiguration>(Configuration);

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = Configuration["JwtConfig:Issuer"],
            //        ValidAudience = Configuration["JwtConfig:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtConfig:AccessTokenSecret"]))
            //    };
            //});

            //services.AddControllers();

            //services.AddSingleton<AccessTokenGenerator>();
            //services.AddSingleton<RefreshTokenGenerator>();
            //services.AddSingleton<RefreshTokenValidator>();
            //services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            //services.AddScoped<IAuthService, AuthService>();
            //services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            //services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            //services.AddTransient<IDBContext, DBContext>();


            //services.AddControllers().AddJsonOptions(options => {
            //    options.JsonSerializerOptions.IgnoreNullValues = true;
            //});

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CrossLang", Version = "v1" });
            //});

            //services.AddControllers()
            // .AddNewtonsoftJson(options =>
            // {
            //     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MISA.WorkScheduling v1"));
            //}

            //app.UseHttpsRedirection();

            ////app.UseSession();

            //app.UseRouting();

            ////app.UseCors();


            ////app.UseAuthentication();
            ////app.UseAuthorization();

            //// global error handler
            //app.UseMiddleware<ErrorHandlerMiddleware>();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            //// global error handler
            ////app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
