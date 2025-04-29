using DataAccess.Data;


var builder = WebApplication.CreateBuilder(args);

// ������ ������������ ��� ����������, ��������� ������������ ���������� JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // ������������ ��� ������� �������� �����������
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 32;
    });

// ������ DbContext ��� ������ � ����� �����
builder.Services.AddDbContext<MusicColectionsDbContext>();

// ������ Swagger ��� �������������� API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ������ CORS ������� ��� ������� ������ � ����������� ������
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

// ����������� ���������� ��������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // �������� Swagger
    app.UseSwaggerUI(); // �������� ��������� Swagger UI
}

// ������������� CORS
app.UseCors("AllowAll");

// ���� ������������
app.UseHttpsRedirection();  // ������������� HTTPS
app.UseAuthorization();     // �����������
app.MapControllers();       // ������������� ����������

// ��������� �������
app.Run();
