using JoyKioskApi.Datas;
using JoyKioskApi.Hubs;
using JoyKioskApi.Repositories.Logings;
using JoyKioskApi.Repositories.Users;
using JoyKioskApi.Services.Authentications;
using JoyKioskApi.Services.CommonServices;
using JoyKioskApi.Services.Customers;
using JoyKioskApi.Services.Otps;
using JoyKioskApi.Services.Payments;
using JoyKioskApi.Services.Promotions;
using JoyKioskApi.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>();

// Add Services
#region Services

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBankPaymentService, BankPaymentService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();

// Repositories
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserTokenRepo, UserTokenRepo>();
builder.Services.AddScoped<ILogTxnRepository, LogTxnRepository>();


#endregion

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample Joy Kiosk API Document", Version = "v.1" });
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });
    c.DocInclusionPredicate((name, api) => true);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer abcdef12345')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
        }
    });
    //c.OperationFilter<SecurityRequirementsOperationFilter>();
    c.CustomSchemaIds(a => a.FullName);

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwts:Issuer"],
        ValidAudience = builder.Configuration["Jwts:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwts:Key"]!)),
        ClockSkew = TimeSpan.Zero
    };
});


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

app.UseCors("CorsJoyKioskWebAPI");

app.MapControllers();
app.MapHub<MessageHub>("/api/payment/callback");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
