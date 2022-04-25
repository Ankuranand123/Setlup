using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Setlup.Models;
using Setlup.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<SetlupStoreDatabaseSettings>(
                builder.Configuration.GetSection(nameof(SetlupStoreDatabaseSettings)));

builder.Services.AddSingleton<ISetlupStoreDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<SetlupStoreDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
        new MongoClient(builder.Configuration.GetValue<string>("SetlupStoreDatabaseSettings:ConnectionString")));


builder.Services.AddScoped<IusersAddService, usersAddService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();