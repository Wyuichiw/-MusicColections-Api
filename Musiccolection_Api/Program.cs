using DataAccess.Data;


var builder = WebApplication.CreateBuilder(args);

// Додаємо конфігурацію для контролерів, включаючи налаштування серіалізації JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Налаштування для обробки циклічних залежностей
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 32;
    });

// Додаємо DbContext для роботи з базою даних
builder.Services.AddDbContext<MusicColectionsDbContext>();

// Додаємо Swagger для документування API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Додаємо CORS політику для дозволу запитів з конкретного домену
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:7085")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Налаштовуємо середовище розробки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Включаємо Swagger
    app.UseSwaggerUI(); // Включаємо інтерфейс Swagger UI
}

// Використовуємо CORS
app.UseCors("AllowAll");

// Інші налаштування
app.UseHttpsRedirection();  // Використовуємо HTTPS
app.UseAuthorization();     // Авторизація
app.MapControllers();       // Маршрутизація контролерів

// Запускаємо додаток
app.Run();
