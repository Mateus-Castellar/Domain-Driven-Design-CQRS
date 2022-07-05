using DDD.Catalogo.Application.AutoMapper;
using DDD.Catalogo.Application.Services;
using DDD.Catalogo.Data;
using DDD.Catalogo.Data.Repository;
using DDD.Catalogo.Domain;
using DDD.Catalogo.Domain.Events;
using DDD.Catalogo.Domain.Services;
using DDD.Core.Bus;
using DDD.Vendas.Application.Commands;
using DDD.WebApp.Mvc.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Base de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<CatalogoContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#endregion

#region Injecao de dependencias
builder.Services.AddScoped<IMediatrHandler, MediatrHandler>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<CatalogoContext>();

//eventos
builder.Services.AddScoped<INotificationHandler<ProdutoEstoqueAbaixoEvent>, ProdutoEventHandler>();
builder.Services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
#endregion

#region Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
#endregion

#region Configs MVC
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddAutoMapper(typeof(DomainToDTOMapping), typeof(DTOToDomainMapping));
builder.Services.AddControllersWithViews();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseMigrationsEndPoint();

else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vitrine}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();