using AutoMapper;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrbanFiesta.Mapper;
using UrbanFiesta.Repository;
using UrbanFiesta.Requirements;
using UrbanFiesta.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddScoped<EventRepository>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddHostedService<EventsStatusUpdateService>();
builder.Services.AddHostedService<EventReminder>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("NotBanned", policy =>
        policy.AddRequirements(new NotBannedRequirement()));
});
builder.Services.AddTransient<IAuthorizationHandler, NotBannedHandler>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("UrbanFiesta")));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddScoped(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AppMappingProfile(
        provider.GetRequiredService<AppDbContext>()));
}).CreateMapper());

builder.Services.AddIdentity<Citizen, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    //password's settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;

});

builder.Services.AddCors();

var app = builder.Build();

await SeedData(app);

async Task SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory!.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<DataSeeder>();
        await service!.SeedAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
    .WithOrigins(
        Environment.GetEnvironmentVariable("ORIGIN")?? "")
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.UseAuthorization();
app.MapControllers();
app.MapGet("/", async () =>
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    var scope = scopedFactory.CreateScope();
    var context = scope.ServiceProvider.GetService<AppDbContext>();
    return context.Set<Event>().FirstOrDefault(eve => eve.Title == "asd");
});
app.Run();