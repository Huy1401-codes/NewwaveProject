using BusinessLogicLayer.Services.Interface.RoleAdmin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using DataAccessLayer.Repositories.RoleAdmin;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using OfficeOpenXml;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Defaul")));



builder.Services.AddHttpContextAccessor();

// Thêm Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // thời gian sống của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();


builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<ISemesterService, SemesterService>();

builder.Services.AddScoped<IClassSemesterRepository,ClassSemesterRepository>();
builder.Services.AddScoped<IClassSemesterService,ClassSemesterService>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ISubjectService, SubjectService>();

builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

builder.Services.AddScoped<IGradeComponentRepository, GradeComponentRepository>();
builder.Services.AddScoped<IGradeComponentService, GradeComponentService>();


/////Teacher

builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IGradeRepository, DataAccessLayer.Repositories.RoleTeacher.GradeRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassStudentRepository, DataAccessLayer.Repositories.RoleTeacher.ClassStudentRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassRepository, DataAccessLayer.Repositories.RoleTeacher.ClassRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassSemesterRepository, DataAccessLayer.Repositories.RoleTeacher.ClassSemesterRepository>();

builder.Services.AddScoped<BusinessLogicLayer.Services.Interface.RoleTeacher.ITeacherService, BusinessLogicLayer.Services.RoleTeacher.TeacherService>();

//////Student
///
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleStudent.IStudentGradeRepository, DataAccessLayer.Repositories.RoleStudent.StudentGradeRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleStudent.IStudentRepository, DataAccessLayer.Repositories.RoleStudent.StudentRepository>();


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



// Thêm NLog
builder.Host.UseNLog();

// EPPlus 8+ license setup
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "all",
    pattern: "{controller}/{action}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");


app.Run();
