using BusinessLogicLayer.Services.Interface.RoleAdmin;
using BusinessLogicLayer.Services.RoleAdmin;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Implementations.RoleAdmin;
using DataAccessLayer.Repositories.Implementations.RoleStudent;
using DataAccessLayer.Repositories.Implementations.RoleTeacher;
using DataAccessLayer.Repositories.Interface.RoleAdmin;
using DataAccessLayer.UnitOfWork;
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

// UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services 
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<IClassSemesterService, ClassSemesterService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IGradeComponentService, GradeComponentService>();

/// Teacher
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IGradeRepository, GradeRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassStudentRepository, DataAccessLayer.Repositories.Implementations.RoleTeacher.ClassStudentRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassRepository, ClassRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleTeacher.IClassSemesterRepository, DataAccessLayer.Repositories.Implementations.RoleTeacher.ClassSemesterRepository>();

builder.Services.AddScoped<BusinessLogicLayer.Services.Interface.RoleTeacher.ITeacherService, BusinessLogicLayer.Services.RoleTeacher.TeacherService>();

/// Student
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleStudent.IStudentGradeRepository, StudentGradeRepository>();
builder.Services.AddScoped<DataAccessLayer.Repositories.Interface.RoleStudent.IStudentRepository, DataAccessLayer.Repositories.Implementations.RoleStudent.StudentRepository>();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});




// Excel license
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// -------------------- Build app --------------------
var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); 
    app.UseHsts();
}
app.UseCors("AllowReact");

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
