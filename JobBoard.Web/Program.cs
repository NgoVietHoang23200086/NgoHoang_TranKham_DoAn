using JobBoard.Infrastructure;
using JobBoard.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// === ĐĂNG KÝ SERVICES VÀO DI CONTAINER ===
// Sử dụng Extension Method từ Infrastructure layer
builder.Services.AddInfrastructureServices(builder.Configuration);

// Đăng ký Razor Pages
builder.Services.AddRazorPages();

// Đăng ký Session để lưu trạng thái đăng nhập (Mô phỏng)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Đăng ký HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// === TẠO DATABASE VÀ SEED DỮ LIỆU MẪU ===
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated(); // Tạo DB nếu chưa có
    DbSeeder.SeedData(context);       // Seed dữ liệu mẫu
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Kích hoạt Session
app.UseAuthorization();
app.MapRazorPages();

app.Run();
