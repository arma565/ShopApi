using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
using RealEstate.Helper;
using Shop.Data;
using Shop.Service;

var MyAllowSpeceficiOrigins = "_shopPolicy";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        MyAllowSpeceficiOrigins,
        policy =>
    {
        policy
        .WithOrigins("http://localhost:5068")
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ShopSqlConnection")));
builder
    .Services.AddIdentity<UserProfileIdentity, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<ShopService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<PasswordHelper>();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(MyAllowSpeceficiOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
