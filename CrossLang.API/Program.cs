using CrossLang.API.Middlewares;
using CrossLang.ApplicationCore;
using CrossLang.ApplicationCore.Entities;
using CrossLang.ApplicationCore.Interfaces;
using CrossLang.ApplicationCore.Interfaces.IRepository;
using CrossLang.ApplicationCore.Interfaces.IService;
using CrossLang.ApplicationCore.Services;
using CrossLang.Authentication;
using CrossLang.Authentication.JWT;
using CrossLang.DBHelper;
using CrossLang.Infrastructure;
using CrossLang.Library;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:AccessTokenSecret"]))
    };
});

builder.Services.AddSingleton<AccessTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenValidator>();
builder.Services.AddSingleton<SessionData>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddScoped<IBaseService<DictionaryWord>, DictionaryWordService>();
builder.Services.AddScoped<IDictionaryWordService, DictionaryWordService>();
builder.Services.AddScoped<IDictionaryWordRepository, DictionaryWordRepository>();
builder.Services.AddScoped<IBaseRepository<DictionaryWord>, DictionaryWordRepository>();

builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IFlashCardRepository, FlashCardRepository>();
builder.Services.AddScoped<IFlashCardService, FlashCardService>();

builder.Services.AddScoped<IFlashCardCollectionRepository, FlashCardCollectionRepository>();
builder.Services.AddScoped<IFlashCardCollectionService, FlashCardCollectionService>();

builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();

builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddScoped<ILessonCommentService, LessonCommentService>();
builder.Services.AddScoped<ILessonCommentRepository, LessonCommentRepository>();

builder.Services.AddScoped<IUpgradeCodeService, UpgradeCodeService>();
builder.Services.AddScoped<IUpgradeCodeRepository, UpgradeCodeRepository>();

builder.Services.AddScoped<IRedeemHistoryService, RedeemHistoryService>();
builder.Services.AddScoped<IRedeemHistoryRepository, RedeemHistoryRepository>();

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddTransient<IDBContext, DBContext>();
builder.Services.AddTransient<IMongoDBContext, MongoContext>();

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(5001); // to listen for incoming http connection on port 5001
//    options.ListenAnyIP(7001, configure => configure.UseHttps()); // to listen for incoming https connection on port 7001
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("MyPolicy");
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

//app.MapControllers();

app.Run();
