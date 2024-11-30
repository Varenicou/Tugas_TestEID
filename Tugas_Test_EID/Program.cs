using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Tugas_Test_EID;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Directory.SetCurrentDirectory("/mnt/e/Interview/TugasASPNET_PID/Tugas_TestEID/Tugas_Test_EID/Tugas_Test_EID");

// Add services to the container.
builder.Services.AddControllersWithViews();

// JWT
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op => {
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "localhost",
        ValidAudience = "localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TugasTestEID2024_ElvinWilis202412"))
    };
});

// Database
builder.Services.AddDbContext<AppCtx>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL_DB"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/secure", [Authorize] () => "Secure Endpoint")
    .RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
