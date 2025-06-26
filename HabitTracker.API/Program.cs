using HabitTracker.API.Repositories;
using HabitTracker.API.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 加入 Controller 架構
builder.Services.AddControllers();

// Swagger 設定（自動產文件）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "請輸入 'Bearer {token}' 來授權"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 加入 Repository DI（HabitRepository）
builder.Services.AddScoped<HabitRepository>();

// 加入 AuditService DI
builder.Services.AddScoped<AuditService>();

// 加入 LogMaintenanceService DI
builder.Services.AddScoped<LogMaintenanceService>();

// Hangfire 服務註冊
builder.Services.AddHangfire(config => config
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// JWT 認證設定
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

// CORS 設定（讓別人可跨域呼叫）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// 開發環境顯示 Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 中介層配置
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();   // 必須
app.UseAuthorization();

// 掛載 Controller 路由
app.MapControllers();

// 掛載 Hangfire Dashboard（可選）
app.UseHangfireDashboard();

// 建立 scope 取得服務並註冊定時任務
using (var scope = app.Services.CreateScope())
{
    var logMaintenance = scope.ServiceProvider.GetRequiredService<LogMaintenanceService>();

    RecurringJob.AddOrUpdate(
        "log-cleanup-job",
        () => logMaintenance.CleanupOldLogsAsync(30),
        "0 3 * * *" // 每天凌晨3點執行
    );
}

app.Run();
