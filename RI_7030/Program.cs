using Microsoft.EntityFrameworkCore;
using RI_7030.Data;
using RI_7030.Models;

var builder = WebApplication.CreateBuilder(args);

// ── In-Memory Database (no SQL Server required) ──────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("RI_7030_InMemory"));

// ── MVC ───────────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ── Session ───────────────────────────────────────────────────────────────────
builder.Services.AddSession(options =>
{
    options.IdleTimeout      = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly  = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ── Seed sample data ─────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    SeedData(db);
}

// ── Pipeline ──────────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

// ── Seed Method ──────────────────────────────────────────────────────────────
static void SeedData(AppDbContext db)
{
    if (db.Users.Any()) return; // already seeded

    // ── Admin User ───────────────────────────────────────────────────────────
    var admin = new User
    {
        UserId    = 1,
        FullName  = "Rajesh Kumar",
        Email     = "admin@ri.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
        Role      = "Admin",
        Phone     = "+91 98765 43210",
        Bio       = "Founder & Managing Director of Rajesh Industries",
        Country   = "India",
        CityState = "Rajkot, Gujarat",
        PostalCode = "360001",
        TaxId     = "ABCPK1234F",
        Address   = "Plot 42, GIDC Industrial Estate, Rajkot",
        CreatedAt = DateTime.Now.AddMonths(-12)
    };

    // ── Employee Users ───────────────────────────────────────────────────────
    var empUser1 = new User
    {
        UserId    = 2,
        FullName  = "Mahesh Patel",
        Email     = "mahesh@ri.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("emp123"),
        Role      = "Employee",
        EmployeeType = "Casting",
        Phone     = "+91 98765 11111",
        Country   = "India",
        CityState = "Ahmedabad, Gujarat",
        CreatedAt = DateTime.Now.AddMonths(-6)
    };
    var empUser2 = new User
    {
        UserId    = 3,
        FullName  = "Priya Sharma",
        Email     = "priya@ri.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("emp123"),
        Role      = "Employee",
        EmployeeType = "Finishing Touch",
        Phone     = "+91 98765 22222",
        Country   = "India",
        CityState = "Surat, Gujarat",
        CreatedAt = DateTime.Now.AddMonths(-4)
    };
    var empUser3 = new User
    {
        UserId    = 4,
        FullName  = "Amit Desai",
        Email     = "amit@ri.com",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("emp123"),
        Role      = "Employee",
        EmployeeType = "Gold Plating",
        Phone     = "+91 98765 33333",
        Country   = "India",
        CityState = "Vadodara, Gujarat",
        CreatedAt = DateTime.Now.AddMonths(-3)
    };

    db.Users.AddRange(admin, empUser1, empUser2, empUser3);
    db.SaveChanges();

    // ── Employees ────────────────────────────────────────────────────────────
    var employees = new[]
    {
        new Employee { EmployeeId = "RI_4001", FullName = "Mahesh Patel",   Email = "mahesh@ri.com",  Department = "Casting",        Salary = 25000, UserId = 2, CreatedAt = DateTime.Now.AddMonths(-6) },
        new Employee { EmployeeId = "RI_4002", FullName = "Priya Sharma",   Email = "priya@ri.com",   Department = "Finishing Touch", Salary = 22000, UserId = 3, CreatedAt = DateTime.Now.AddMonths(-4) },
        new Employee { EmployeeId = "RI_4003", FullName = "Amit Desai",     Email = "amit@ri.com",    Department = "Gold Plating",   Salary = 28000, UserId = 4, CreatedAt = DateTime.Now.AddMonths(-3) },
        new Employee { EmployeeId = "RI_4004", FullName = "Ravi Joshi",     Email = "ravi@ri.com",    Department = "Packaging",      Salary = 20000, CreatedAt = DateTime.Now.AddDays(-5) },
        new Employee { EmployeeId = "RI_4005", FullName = "Neha Trivedi",   Email = "neha@ri.com",    Department = "Casting",        Salary = 24000, CreatedAt = DateTime.Now.AddDays(-2) },
    };
    db.Employees.AddRange(employees);
    db.SaveChanges();

    // ── Products (Inventory) ─────────────────────────────────────────────────
    var products = new[]
    {
        new Product { ProductId = "RI_1001", Name = "Gold Chain",       Stock = 50,  UnitCost = 4500,  LowStock = false, Stage_Casting = true,  Stage_Finishing = true,  Stage_GoldPlating = true,  Stage_Packaging = true  },
        new Product { ProductId = "RI_1002", Name = "Ring Set",         Stock = 30,  UnitCost = 2800,  LowStock = false, Stage_Casting = true,  Stage_Finishing = true,  Stage_GoldPlating = false, Stage_Packaging = false },
        new Product { ProductId = "RI_1003", Name = "Necklace",         Stock = 20,  UnitCost = 5500,  LowStock = false, Stage_Casting = true,  Stage_Finishing = false, Stage_GoldPlating = false, Stage_Packaging = false },
        new Product { ProductId = "RI_1004", Name = "Earrings",         Stock = 100, UnitCost = 1200,  LowStock = false, Stage_Casting = true,  Stage_Finishing = true,  Stage_GoldPlating = true,  Stage_Packaging = true  },
        new Product { ProductId = "RI_1005", Name = "Gold Bangle",      Stock = 12,  UnitCost = 3800,  LowStock = true,  Stage_Casting = false, Stage_Finishing = false, Stage_GoldPlating = false, Stage_Packaging = false },
        new Product { ProductId = "RI_1006", Name = "Silver Earrings",  Stock = 8,   UnitCost = 900,   LowStock = true,  Stage_Casting = true,  Stage_Finishing = true,  Stage_GoldPlating = true,  Stage_Packaging = true  },
        new Product { ProductId = "RI_1007", Name = "Diamond Pendant",  Stock = 5,   UnitCost = 15000, LowStock = true,  Stage_Casting = true,  Stage_Finishing = true,  Stage_GoldPlating = false, Stage_Packaging = false },
    };
    db.Products.AddRange(products);
    db.SaveChanges();

    // ── Orders ───────────────────────────────────────────────────────────────
    var orders = new[]
    {
        new Order { OrderId = "RI_2001", CustomerName = "Mahesh Patel",       Email = "mahesh@gmail.com",        ProductId = "RI_1001", ProductName = "Gold Chain",      Quantity = 50,  UnitPrice = 5000,  TotalAmount = 250000,  DueDate = DateTime.Now.AddDays(-16), Status = "In Production",  Stage_C = true,  Stage_F = true,  Stage_G = true,  Stage_P = false, CreatedAt = DateTime.Now.AddDays(-30) },
        new Order { OrderId = "RI_2002", CustomerName = "Ramesh Jewellers",   Email = "ramesh@jewellers.com",    ProductId = "RI_1002", ProductName = "Ring Set",         Quantity = 30,  UnitPrice = 3000,  TotalAmount = 90000,   DueDate = DateTime.Now.AddDays(-14), Status = "In Production",  Stage_C = true,  Stage_F = true,  Stage_G = false, Stage_P = false, CreatedAt = DateTime.Now.AddDays(-25) },
        new Order { OrderId = "RI_2003", CustomerName = "Vijay Exports",      Email = "vijay@exports.com",       ProductId = "RI_1003", ProductName = "Necklace",         Quantity = 20,  UnitPrice = 6000,  TotalAmount = 120000,  DueDate = DateTime.Now.AddDays(-11), Status = "Pending",        Stage_C = true,  Stage_F = false, Stage_G = false, Stage_P = false, CreatedAt = DateTime.Now.AddDays(-20) },
        new Order { OrderId = "RI_2004", CustomerName = "Anita Stores",       Email = "anita@stores.com",        ProductId = "RI_1004", ProductName = "Earrings",         Quantity = 100, UnitPrice = 500,   TotalAmount = 50000,   DueDate = DateTime.Now.AddDays(-9),  Status = "Ready",          Stage_C = true,  Stage_F = true,  Stage_G = true,  Stage_P = true,  CreatedAt = DateTime.Now.AddDays(-15) },
        new Order { OrderId = "RI_2005", CustomerName = "Kishan Bros.",       Email = "kishan@bros.com",         ProductId = "RI_1005", ProductName = "Gold Bangle",      Quantity = 150, UnitPrice = 500,   TotalAmount = 75000,   DueDate = DateTime.Now.AddDays(-6),  Status = "Pending",        Stage_C = false, Stage_F = false, Stage_G = false, Stage_P = false, CreatedAt = DateTime.Now.AddDays(-10) },
        new Order { OrderId = "RI_2006", CustomerName = "Suresh Traders",     Email = "suresh@traders.com",      ProductId = "RI_1006", ProductName = "Silver Earrings",  Quantity = 200, UnitPrice = 200,   TotalAmount = 40000,   DueDate = DateTime.Now.AddDays(5),   Status = "Delivered",      Stage_C = true,  Stage_F = true,  Stage_G = true,  Stage_P = true,  CreatedAt = DateTime.Now.AddDays(-40) },
    };
    db.Orders.AddRange(orders);
    db.SaveChanges();

    // ── Transactions ─────────────────────────────────────────────────────────
    var transactions = new[]
    {
        // Sell transactions (Pending → receivable)
        new Transaction { TransactionId = "RI_3001", Type = "Sell", PartyName = "Mahesh Patel",     ProductId = "RI_1001", ProductName = "Gold Chain",     Quantity = 25,  Amount = 125000, Status = "Pending",   Category = "PRODUCT",     TransactionDate = DateTime.Now.AddDays(-8) },
        new Transaction { TransactionId = "RI_3002", Type = "Sell", PartyName = "Ramesh Jewellers", ProductId = "RI_1002", ProductName = "Ring Set",       Quantity = 10,  Amount = 30000,  Status = "Pending",   Category = "PRODUCT",     TransactionDate = DateTime.Now.AddDays(-5) },
        new Transaction { TransactionId = "RI_3003", Type = "Sell", PartyName = "Suresh Traders",   ProductId = "RI_1006", ProductName = "Silver Earrings",Quantity = 200, Amount = 40000,  Status = "Received",  Category = "PRODUCT",     PaymentMethod = "Bank Transfer", TransactionDate = DateTime.Now.AddDays(-40) },

        // Buy transactions (Pending → payable)
        new Transaction { TransactionId = "RI_3004", Type = "Buy",  PartyName = "Gold Supplier Co.",  ProductId = "RI_1001", ProductName = "Gold Chain",   Quantity = 100, Amount = 450000, Status = "Pending",   Category = "PRODUCT",     TransactionDate = DateTime.Now.AddDays(-12) },
        new Transaction { TransactionId = "RI_3005", Type = "Buy",  PartyName = "Metal Works Ltd.",   ProductId = "RI_1005", ProductName = "Gold Bangle",  Quantity = 50,  Amount = 190000, Status = "Paid",      Category = "PRODUCT",     PaymentMethod = "UPI", TransactionDate = DateTime.Now.AddDays(-20) },

        // Salary transactions
        new Transaction { TransactionId = "RI_3006", Type = "Buy",  PartyName = "Mahesh Patel",       Quantity = 1, Amount = 25000,  Status = "Paid",     Category = "EMP_SALARY",  PaymentMethod = "Bank Transfer", TransactionDate = DateTime.Now.AddDays(-3) },
        new Transaction { TransactionId = "RI_3007", Type = "Buy",  PartyName = "Priya Sharma",       Quantity = 1, Amount = 22000,  Status = "Paid",     Category = "EMP_SALARY",  PaymentMethod = "Bank Transfer", TransactionDate = DateTime.Now.AddDays(-3) },

        // Advance salary request (pending)
        new Transaction { TransactionId = "RI_3008", Type = "Buy",  PartyName = "Amit Desai",         Quantity = 1, Amount = 10000,  Status = "Pending",  Category = "EMP_ADVANCE", PaymentMethod = "Pending", TransactionDate = DateTime.Now.AddDays(-1) },
    };
    db.Transactions.AddRange(transactions);
    db.SaveChanges();
}