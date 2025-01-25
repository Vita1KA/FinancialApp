var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ApiService>();

builder.Services.AddHttpClient("FinanceApi", client =>
{
    client.BaseAddress = new Uri("https://backendfinancialapp-cdbah3dzbecth8hq.westeurope-01.azurewebsites.net/");
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();