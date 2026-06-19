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

    var seedDocuments = new List<Document>
    {
        new()
        {
            DocumentName = "สัญญาจ้างงาน",
            DocumentDetail = "เอกสารสัญญาจ้างงานพนักงานใหม่",
            Status = DocumentStatus.Pending
        },
        new()
        {
            DocumentName = "ใบเสนอราคา",
            DocumentDetail = "เอกสารใบเสนอราคาสำหรับลูกค้า",
            Status = DocumentStatus.Approved,
            StatusReason = "อนุมัติตามเงื่อนไขที่ตกลง"
        },
        new()
        {
            DocumentName = "ใบขออนุมัติซื้ออุปกรณ์",
            DocumentDetail = "คำขออนุมัติจัดซื้ออุปกรณ์สำนักงานประจำไตรมาส",
            Status = DocumentStatus.Pending
        },
        new()
        {
            DocumentName = "บันทึกขออนุมัติเดินทาง",
            DocumentDetail = "เอกสารขออนุมัติเดินทางไปพบลูกค้าต่างจังหวัด",
            Status = DocumentStatus.Pending
        },
        new()
        {
            DocumentName = "สรุปรายงานผลการประชุม",
            DocumentDetail = "รายงานผลการประชุมประจำเดือนของฝ่ายปฏิบัติการ",
            Status = DocumentStatus.Approved,
            StatusReason = "ตรวจสอบครบถ้วนและอนุมัติแล้ว"
        },
        new()
        {
            DocumentName = "ใบคำขอเบิกค่าใช้จ่าย",
            DocumentDetail = "เอกสารเบิกค่าใช้จ่ายการเดินทางและที่พัก",
            Status = DocumentStatus.Rejected,
            StatusReason = "เอกสารแนบไม่ครบถ้วน"
        },
        new()
        {
            DocumentName = "แผนงานพัฒนาระบบ",
            DocumentDetail = "แผนงานพัฒนาระบบภายในสำหรับปีงบประมาณถัดไป",
            Status = DocumentStatus.Pending
        },
        new()
        {
            DocumentName = "ใบเสนอจ้างผู้รับเหมา",
            DocumentDetail = "เอกสารเสนอจ้างผู้รับเหมางานปรับปรุงพื้นที่สำนักงาน",
            Status = DocumentStatus.Approved,
            StatusReason = "ผ่านการพิจารณาความเหมาะสมด้านราคา"
        },
        new()
        {
            DocumentName = "คำขอเพิ่มสิทธิ์การใช้งานระบบ",
            DocumentDetail = "คำขอเพิ่มสิทธิ์เข้าถึงระบบ ERP สำหรับพนักงานใหม่",
            Status = DocumentStatus.Pending
        },
        new()
        {
            DocumentName = "แบบฟอร์มลาออก",
            DocumentDetail = "เอกสารแจ้งลาออกพร้อมส่งมอบงาน",
            Status = DocumentStatus.Rejected,
            StatusReason = "ยังไม่ครบกำหนดแจ้งล่วงหน้า"
        },
        new()
        {
            DocumentName = "ข้อตกลงการรักษาความลับ",
            DocumentDetail = "สัญญาการรักษาความลับข้อมูลสำหรับคู่ค้าใหม่",
            Status = DocumentStatus.Approved,
            StatusReason = "ฝ่ายกฎหมายตรวจสอบและอนุมัติแล้ว"
        },
        new()
        {
            DocumentName = "ใบขอเปลี่ยนแปลงข้อมูลพนักงาน",
            DocumentDetail = "คำขอแก้ไขข้อมูลที่อยู่และบัญชีธนาคารพนักงาน",
            Status = DocumentStatus.Pending
        }
    };

    var now = DateTime.UtcNow;
    var existingNames = dbContext.Documents
        .Select(x => x.DocumentName)
        .ToHashSet();

    var documentsToAdd = seedDocuments
        .Where(x => !existingNames.Contains(x.DocumentName))
        .Select(x =>
        {
            x.CreatedAt = now;
            x.UpdatedAt = now;
            return x;
        })
        .ToList();

    if (documentsToAdd.Count > 0)
    {
        dbContext.Documents.AddRange(documentsToAdd);
        dbContext.SaveChanges();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
