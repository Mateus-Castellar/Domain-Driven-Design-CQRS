using DDD.Catalogo.Application.AutoMapper;
using DDD.Catalogo.Application.Services;
using DDD.Catalogo.Data;
using DDD.Catalogo.Data.Repository;
using DDD.Catalogo.Domain;
using DDD.Catalogo.Domain.Events;
using DDD.Catalogo.Domain.Services;
using DDD.Core.Communication.Mediator;
using DDD.Core.Messages.CommonMessages.IntegrationEvents;
using DDD.Core.Messages.CommonMessages.Notifications;
using DDD.Pagamentos.AntiCorruption;
using DDD.Pagamentos.Business.Events;
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
using EventSourcing;
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
// Mediator
builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();

//Event Sourcing
builder.Services.AddSingleton<IEventStoreService, EventStoreService>();

// Notifications
builder.Services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

// Catalogo
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<CatalogoContext>();

builder.Services.AddScoped<INotificationHandler<ProdutoEstoqueAbaixoEvent>, ProdutoEventHandler>();
builder.Services.AddScoped<INotificationHandler<PedidoIniciadoEvent>, ProdutoEventHandler>();
builder.Services.AddScoped<INotificationHandler<PedidoProcessamentoCanceladoEvent>, ProdutoEventHandler>();

// Vendas
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidosQueries, PedidosQueries>();
builder.Services.AddScoped<VendasContext>();

builder.Services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<AplicarCupomPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<IniciarPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<FinalizarPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoCommand, bool>, PedidoCommandHandler>();
builder.Services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoEstonarEstoqueCommand, bool>, PedidoCommandHandler>();

builder.Services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
builder.Services.AddScoped<INotificationHandler<PedidoEstoqueRejeitadoEvent>, PedidoEventHandler>();
builder.Services.AddScoped<INotificationHandler<PagamentoRealizadoEvent>, PedidoEventHandler>();
builder.Services.AddScoped<INotificationHandler<PagamentoRecusadoEvent>, PedidoEventHandler>();

// Pagamento
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
builder.Services.AddScoped<IPayPalGateway, PayPalGateway>();
builder.Services.AddScoped<IConfigurationManager, ConfigurationManager>();
builder.Services.AddScoped<PagamentosContext>();

builder.Services.AddScoped<INotificationHandler<PedidoEstoqueConfirmadoEvent>, PagamentoEventHandler>();

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