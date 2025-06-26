using HabitTracker.API.Repositories;
using HabitTracker.API.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// �[�J Controller �[�c
builder.Services.AddControllers();

// Swagger �]�w�]�۰ʲ����^
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
        Description = "�п�J 'Bearer {token}' �ӱ��v"
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

// �[�J Repository DI�]HabitRepository�^
builder.Services.AddScoped<HabitRepository>();

// �[�J AuditService DI
builder.Services.AddScoped<AuditService>();

// �[�J LogMaintenanceService DI
builder.Services.AddScoped<LogMaintenanceService>();

// Hangfire �A�ȵ��U
builder.Services.AddHangfire(config => config
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// JWT �{�ҳ]�w
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

// CORS �]�w�]���O�H�i���I�s�^
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// �}�o������� Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �����h�t�m
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();   // ����
app.UseAuthorization();

// ���� Controller ����
app.MapControllers();

// ���� Hangfire Dashboard�]�i��^
app.UseHangfireDashboard();

// �إ� scope ���o�A�Ȩõ��U�w�ɥ���
using (var scope = app.Services.CreateScope())
{
    var logMaintenance = scope.ServiceProvider.GetRequiredService<LogMaintenanceService>();

    RecurringJob.AddOrUpdate(
        "log-cleanup-job",
        () => logMaintenance.CleanupOldLogsAsync(30),
        "0 3 * * *" // �C�ѭ��3�I����
    );
}

app.Run();
