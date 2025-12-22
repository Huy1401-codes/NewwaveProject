using BusinessLogicLayer.Services.Interface.RoleAdmin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using DataAccessLayer.Repositories.RoleAdmin;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;
using OfficeOpenXml;
using WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Add Services --------------------

// Add API controllers 
builder.Services.AddControllers();

// Configure DbContext
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Defaul")));

// Configure DI for Repositories and Services
/// Admin
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<ISemesterService, SemesterService>();

builder.Services.AddScoped<IClassSemesterRepository, ClassSemesterRepository>();
builder.Services.AddScoped<IClassSemesterService, ClassSemesterService>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ISubjectService, SubjectService>();

builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

builder.Services.AddScoped<IGradeComponentRepository, GradeComponentRepository>();
builder.Services.AddScoped<IGradeComponentService, GradeComponentService>();

/// Teacher
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IGradeRepository, DataAccessLayer.Repositories.RoleTeacher.GradeRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassStudentRepository, DataAccessLayer.Repositories.RoleTeacher.ClassStudentRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassRepository, DataAccessLayer.Repositories.RoleTeacher.ClassRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassSemesterRepository, DataAccessLayer.Repositories.RoleTeacher.ClassSemesterRepository>();

builder.Services.AddScoped<BusinessLogicLayer.Services.Interface.RoleTeacher.ITeacherService, BusinessLogicLayer.Services.RoleTeacher.TeacherService>();

/// Student
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleStudent.IStudentGradeRepository, DataAccessLayer.Repositories.RoleStudent.StudentGradeRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleStudent.IStudentRepository, DataAccessLayer.Repositories.RoleStudent.StudentRepository>();

builder.Services.AddScoped<BusinessLogicLayer.Services.Interface.RoleStudent.IStudentService, BusinessLogicLayer.Services.RoleStudent.StudentService>();

// Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLogWeb();
});

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // API vẫn có thể trả 401
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Host.UseNLog();
builder.Services.AddEndpointsApiExplorer(); // required for minimal APIs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "School API",
        Version = "v1",
        Description = "API quản lý học sinh, giáo viên, tài khoản"
    });

    // Nếu dùng JWT hoặc Cookie Auth, có thể thêm config security
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[]{ }
        }
    });
});
// Excel license
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// -------------------- Build app --------------------
var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // API có thể dùng middleware trả JSON lỗi
    app.UseHsts();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Custom request logging
app.UseMiddleware<RequestLoggingMiddleware>();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "School API v1");
        c.RoutePrefix = "swagger"; // truy cập: https://localhost:5001/swagger
    });
}
// Map API controllers
app.MapControllers(); 

app.Run();
