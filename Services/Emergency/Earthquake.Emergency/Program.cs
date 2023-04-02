global using FastEndpoints;
using Amazon.SecretsManager;
using Earthquake.Emergency.Contexts;
using Earthquake.Emergency.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
//builder.Services.AddDbContext<ApplicationContext>(async opt =>
//{
//    var client = new AmazonSecretsManagerClient();
//    var connectionString = await client.GetSecretValueAsync(new Amazon.SecretsManager.Model.GetSecretValueRequest
//    {
//        SecretId = "rds-connection-string"
//    });
//    opt.UseNpgsql(connectionString.SecretString);
//});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost", "http://localhost").AllowAnyHeader()
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
app.UseFastEndpoints();

app.UseHttpsRedirection();

app.Run();
