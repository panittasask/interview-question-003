using ApproveDocument.Data;
using ApproveDocument.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.Documents.Any())
    {
        dbContext.Documents.AddRange(
            new Document
            {
                DocumentName = "สัญญาจ้างงาน",
                DocumentDetail = "เอกสารสัญญาจ้างงานพนักงานใหม่",
                Status = DocumentStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Document
            {
                DocumentName = "ใบเสนอราคา",
                DocumentDetail = "เอกสารใบเสนอราคาสำหรับลูกค้า",
                Status = DocumentStatus.Approved,
                StatusReason = "อนุมัติตามเงื่อนไขที่ตกลง",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

        dbContext.SaveChanges();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
