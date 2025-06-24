using HabitTracker.API.Repositories;
using HabitTracker.API.Services;

var builder = WebApplication.CreateBuilder(args);

// 加入 Controller 架構
builder.Services.AddControllers();

// Swagger 設定（自動產文件）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 加入 Repository DI（HabitRepository）
builder.Services.AddScoped<HabitRepository>();

// 加入 AuditService DI
builder.Services.AddScoped<AuditService>();

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
app.UseAuthorization();

// 掛載 Controller 路由
app.MapControllers();

app.Run();
