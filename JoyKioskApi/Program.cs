using JoyKioskApi.Services.CommonServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Services
#region Services

builder.Services.AddScoped<ICommonService, CommonService>();


#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Allow Cors
builder.Services.AddCors(o => o.AddPolicy("CorsJoyKioskWebAPI", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("CorsJoyKioskWebAPI");

app.MapControllers();

app.Run();
