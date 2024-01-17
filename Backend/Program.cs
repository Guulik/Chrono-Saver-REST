using Backend.API.Services;
using Backend.Configuration;
using DatabaseAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ChronoDbContext>(x => x.UseSqlServer(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionStrings>>().Value.ChronoDbConnetionString));
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<GameService>();
builder.Services.AddTransient<SaveService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
