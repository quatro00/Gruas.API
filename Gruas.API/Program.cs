using Gruas.API.Data;
using Gruas.API.Repositories.Implementation;
using Gruas.API.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<GruasContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IAspNetUsersRepository, AspNetUsersRepository>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
builder.Services.AddScoped<ICatalogoRepository, CatalogoRepository>();
builder.Services.AddScoped<IGruaRepository, GruaRepository>();
//builder.Services.AddScoped<ITokenRepository, TokenRepository>();
//builder.Services.AddScoped<IMensajesInstitucionalesRepository, MensajesInstitucionalesRepository>();
//builder.Services.AddScoped<ICatTipoMensajesInstitucionalesRepository, CatTipoMensajesInstitucionalesRepository>();
//builder.Services.AddScoped<IAspNetUsersRepository, AspNetUsersRepository>();
//builder.Services.AddScoped<IPermisosRepository, PermisosRepository>();
//builder.Services.AddScoped<IOrdenCompraRepository, OrdenCompraRepository>();
//builder.Services.AddScoped<IDepartamentosRepository, DepartamentosRepository>();
//builder.Services.AddScoped<ITiendasRepository, TiendasRepository>();
//builder.Services.AddScoped<ICitasRepository, CitasRespository>();
//builder.Services.AddScoped<IAsnRepository, AsnRepository>();
//builder.Services.AddScoped<ICedisRepository, CedisRepository>();
//builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
//builder.Services.AddScoped<IMaterialEntregaRepository, MaterialEntregaRepository>();
//builder.Services.AddScoped<ICentroRepository, CentroRepository>();
//builder.Services.AddScoped<IHorariosRepository, HorarioRepository>();
//builder.Services.AddScoped<IRielRepository, RielRepository>();
//builder.Services.AddScoped<IBloqueoAndenesRepository, BloqueoAndenesRepository>();
//builder.Services.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();
//builder.Services.AddScoped<IFacturacionRepository, FacturacionRepository>();
//builder.Services.AddScoped<IIncidenciasRepository, IncidenciasRepository>();
//builder.Services.AddScoped<IPagosRepository, PagosRepository>();
//builder.Services.AddScoped<IEtiquetasRepository, EtiquetasRepository>();
//builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
//builder.Services.AddScoped<IReporteRepository, ReporteRepository>();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("BDS")
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            AuthenticationType = "Jwt",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseCors("AllowAny");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
