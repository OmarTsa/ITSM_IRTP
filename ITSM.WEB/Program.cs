// ... (código anterior de la conexión a Oracle)
using ITSM.Datos;

builder.Services.AddDbContext<ContextoBD>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- NUEVO: REGISTRO DE SERVICIOS DE NEGOCIO ---
builder.Services.AddScoped<ITSM.Negocio.UsuarioNegocio>();
// -----------------------------------------------

var app = builder.Build();
// ... (resto del código)