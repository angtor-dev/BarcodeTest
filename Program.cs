// Activar IronBarCode
IronBarCode.License.LicenseKey = "IRONBARCODE.ANGELTORRES.20947-CDC65D93FB-DDKT3U-LURHLGLLTFB2-DQ2Z6BYQFRY7-ALIRWV32WIZ2-OFUGWFDJCASQ-YDPQOIR6HOBD-OMKP5Z-TWSEZ7SZALKIUA-DEPLOYMENT.TRIAL-42BE6F.TRIAL.EXPIRES.15.JAN.2023";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
