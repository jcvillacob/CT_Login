using CT_Login.Repositories;
using CT_Login.Services;
using CT_Login.Helpers;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde .env
Env.Load();

// Configurar conexi�n a BD y JWT secret desde .env
builder.Configuration["JWT_SECRET_KEY"] = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
builder.Configuration["ConnectionStrings:Default"] = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

// Agregar servicios al contenedor.
builder.Services.AddControllers();

// Inyecci�n de dependencias
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IConductoresBukService, ConductoresBukService>();

// Configurar autenticaci�n JWT (si deseas proteger otros endpoints, aqu� se configura el middleware)
// Por simplicidad, este ejemplo se enfoca en la generaci�n y refresco de tokens.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Si implementas la validaci�n del token, agrega app.UseAuthentication(); app.UseAuthorization();

app.MapControllers();

app.Run();