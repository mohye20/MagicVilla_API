using MagicVilla_Web;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient<IVillaServices, VillaServices>();
builder.Services.AddScoped<IVillaServices, VillaServices>();
builder.Services.AddHttpClient<IVillaNumberServices, VillaNumberServices>();
builder.Services.AddScoped<IVillaNumberServices, VillaNumberServices>();

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();