using DDD.Catalogo.Application.AutoMapper;
using DDD.Catalogo.Application.Services;
using DDD.Catalogo.Data;
using DDD.Catalogo.Data.Repository;
using DDD.Catalogo.Domain;
using DDD.Catalogo.Domain.Events;
using DDD.Catalogo.Domain.Services;
using DDD.Core.Communication.Mediator;
using DDD.Core.Messages.CommonMessages.Notifications;
using DDD.Pagamentos.AntiCorruption;
using DDD.Pagamentos.Business.Interfaces;
using DDD.Pagamentos.Business.Services;
using DDD.Pagamentos.Data;
using DDD.Pagamentos.Data.Repository;
using DDD.Vendas.Application.Commands;
using DDD.Vendas.Application.Events;
using DDD.Vendas.Application.Queries;
using DDD.Vendas.Data;
using DDD.Vendas.Data.Repository;
using DDD.Vendas.Domain;
using DDD.WebApp.Mvc.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ConfigurationManager = DDD.Pagamentos.AntiCorruption.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

#region Base de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<CatalogoContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<VendasContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<PagamentosContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
#endregion

#region Injecao de dependencias
builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<AplicarCupomPedidoCommand, bool>, PedidoCommandHandler>();

builder.Services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
builder.Services.AddScoped<INotificationHandler<ProdutoEstoqueAbaixoEvent>, ProdutoEventHandler>();
builder.Services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
builder.Services.AddScoped<INotificationHandler<PedidoAtualizadoEvent>, PedidoEventHandler>();
builder.Services.AddScoped<INotificationHandler<PedidoItemAdicionadoEvent>, PedidoEventHandler>();

builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
builder.Services.AddScoped<IPayPalGateway, PayPalGateway>();
builder.Services.AddScoped<IConfigurationManager, ConfigurationManager>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IPedidosQueries, PedidosQueries>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
builder.Services.AddScoped<CatalogoContext>();
builder.Services.AddScoped<VendasContext>();
builder.Services.AddScoped<PagamentosContext>();
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