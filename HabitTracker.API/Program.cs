using HabitTracker.API.Repositories;
using HabitTracker.API.Services;

var builder = WebApplication.CreateBuilder(args);

// �[�J Controller �[�c
builder.Services.AddControllers();

// Swagger �]�w�]�۰ʲ����^
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// �[�J Repository DI�]HabitRepository�^
builder.Services.AddScoped<HabitRepository>();

// �[�J AuditService DI
builder.Services.AddScoped<AuditService>();

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
app.UseAuthorization();

// ���� Controller ����
app.MapControllers();

app.Run();
