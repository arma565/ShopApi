using Microsoft.EntityFrameworkCore;

var MyAllowSpeceficiOrigins = "_shop";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options=>{
    options.AddPolicy(MyAllowSpeceficiOrigins , policy=>{
        policy.WithOrigins("http://localhost:5047").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options=> options.UseSqlServer(builder.Configuration.GetConnectionString("ShopSqlConnection")));
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ShopService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(MyAllowSpeceficiOrigins);
app.UseAuthorization();
app.MapControllers();
app.Run();
