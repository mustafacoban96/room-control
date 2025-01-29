using System.Text;
using api.Data;
using api.Helper;
using api.Interface;
using api.Services;

//using api.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
// DATABASE
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var _authkey = builder.Configuration.GetValue<string>("TokenService:_key");
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
var _tokenservice = builder.Configuration.GetSection("TokenService");
builder.Services.Configure<TokenService>(_tokenservice);
// DI
//builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITokenService,TokenService>();
builder.Services.AddTransient<IRoleService,RoleService>();
//

//Auto mapper
var automapper = new MapperConfiguration(item => item.AddProfile(new AutoMapperHandler()));
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);
//
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // Swagger'ı etkinleştirir.
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//auth
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

