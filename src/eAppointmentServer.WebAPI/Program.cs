using eAppointmentServer.Application;
using eAppointmentServer.Infrastructure;
using eAppointmentServer.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDI();
builder.Services.AddInfrastructureDI(builder.Configuration);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.MapMcp("/mcp");

await Helper.CreateUserAsync(app);

app.Run();
