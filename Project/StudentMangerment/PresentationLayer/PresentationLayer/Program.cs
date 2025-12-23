using BusinessAccessLayer.Services.RoleAdmin;
using BusinessLogicLayer.Services.Interface.RoleAdmin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Implementations.RoleAdmin;
using DataAccessLayer.Repositories.Implementations.RoleStudent;
using DataAccessLayer.Repositories.Implementations.RoleTeacher;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using OfficeOpenXml;
using PresentationLayer.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Defaul")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services (chỉ cần service, repository sẽ dùng qua UnitOfWork)
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<IClassSemesterService, ClassSemesterService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IGradeComponentService, GradeComponentService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

/////Teacher
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IGradeRepository, GradeRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassStudentRepository, DataAccessLayer.Repositories.Implementations.RoleTeacher.ClassStudentRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassRepository, ClassRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassSemesterRepository, DataAccessLayer.Repositories.Implementations.RoleTeacher.ClassSemesterRepository>();

builder.Services.AddScoped<BusinessLogicLayer.Services.Interface.RoleTeacher.ITeacherService, BusinessLogicLayer.Services.RoleTeacher.TeacherService>();

//////Student
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleStudent.IStudentGradeRepository, StudentGradeRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleStudent.IStudentRepository, DataAccessLayer.Repositories.Implementations.RoleStudent.StudentRepository>();

builder.Services.AddScoped<BusinessLogicLayer.Services.Interface.RoleStudent.IStudentService, BusinessLogicLayer.Services.RoleStudent.StudentService>();

builder.Services.AddLogging(loggingbuider =>
{
    loggingbuider.ClearProviders();
    loggingbuider.AddNLogWeb();
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Host.UseNLog();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSession();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllerRoute(
    name: "all",
    pattern: "{controller}/{action}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");


app.Run();
