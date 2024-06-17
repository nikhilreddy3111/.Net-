using Dapper;
using EmployeeManagement.Constants;
using EmployeeManagement.DapperContent;
using EmployeeManagement.Modals;
using EmployeeManagement.Models;
using EmployeeManagement.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EmployeeManagement.Controllers
{
    [Route("api/[Controller]")]
    public class EmployeeController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IDapperContent _dapperContent;
        private readonly IValidationService _validationService;

        public EmployeeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IDapperContent dapperContent, IValidationService validationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dapperContent = dapperContent;
            _validationService = validationService;
        }

        // GET: EmployeeController/Details/5
        [Route("~/api/Employee")]
        [HttpGet(nameof(GetAllEmployees))]
        public async Task<ActionResult> GetAllEmployees()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            IdentityUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            if (user != null)
            {
                string query = $" SELECT 1 FROM AspNetUserRoles ur INNER JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = '{user.Id}' AND r.Name = '{Roles.Admin.ToString()}'";
                int result=await Task.FromResult(_dapperContent.Get<int>(query, null, commandType: CommandType.Text));
                if (result != 1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            List<EmployeeDto> employees = new();
            try
            {
                string query = $"Select E.*,Asp.Email,D.Id as DepartmentId,D.Name as DepartmentName,V.ID as VacationId,V.NumberOfDays from Employee E JOIN Department D ON E.DepartmentId=D.Id JOIN Vacation V ON E.Id=V.EmployeeId JOIN AspNetUsers Asp ON E.AspNetUserId=Asp.Id";
                employees = await Task.FromResult(_dapperContent.GetAll<EmployeeDto>(query, null, commandType: CommandType.Text));

            }
            catch (Exception ex)
            {

            }
            return View(employees);
        }

        // GET: EmployeeController/Details/5
        [HttpGet(nameof(Details))]
        public async Task<ActionResult> Details(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            string query = $"Select E.*,Asp.Email,D.Id as DepartmentId,D.Name as DepartmentName from Employee E JOIN Department D ON E.DepartmentId=D.Id JOIN AspNetUsers Asp ON E.AspNetUserId=Asp.Id Where E.Id={id}";
            EmployeeDto employee = await Task.FromResult(_dapperContent.Get<EmployeeDto>(query, null, commandType: CommandType.Text));
            return View(employee);
        }

        // GET: EmployeeController/Create
        [HttpGet(nameof(Create))]
        public async Task<ActionResult> Create()
        {
            EmployeeDto employee = new EmployeeDto();
            await GetDepartments(employee);
            return View(employee);
        }

        // POST: EmployeeController/CreateEmployee
        [HttpPost(nameof(CreateEmployee))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEmployee([FromForm] EmployeeDto employee)
        {
            try
            {
                employee.Errors = _validationService.ValidateEmployee(employee);
                await GetDepartments(employee);
                if (!employee.Errors.Any())
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = employee.Email,
                        Email = employee.Email
                    };
                    await _userManager.CreateAsync(user, employee.Password);

                    string getRoleQuery = $"Select Id from AspNetRoles Where Name='{employee.Role}'";
                    string roleId = await Task.FromResult(_dapperContent.Get<string>(getRoleQuery, null, commandType: CommandType.Text));

                    string insertRoleQuery = $"Insert into AspNetUserRoles(UserId,RoleId) Values('{user.Id}','{roleId}'); ";
                    await Task.FromResult(_dapperContent.Get<int>(insertRoleQuery, null, commandType: CommandType.Text));

                    employee.AspNetUserId = user.Id;

                    employee.CreatedOn = DateTime.Now;
                    employee.LastLoggedIn = DateTime.Now;
                    string insertQuery = "Insert into Employee(AspNetUserId,Name,DepartmentId,Address,PhoneNumber,CreatedOn,LastLoggedIn) Values(@AspNetUserId,@Name,@DepartmentId,@Address,@PhoneNumber,@CreatedOn,@LastLoggedIn); " +
                        "SELECT CAST(SCOPE_IDENTITY() as int);";
                    employee.Id = await _dapperContent.Insert(insertQuery, employee);
                    if (employee.Id > 0)
                    {
                        string vacationQuery = "Insert into Vacation Values(@Id,14); SELECT CAST(SCOPE_IDENTITY() as int);";
                        employee.VacationId = await _dapperContent.Insert(vacationQuery, employee);
                        employee.NumberOfDays = 14;
                        Microsoft.AspNetCore.Identity.SignInResult signIn = await _signInManager.PasswordSignInAsync(employee.Email, employee.Password, false, true);
                        return RedirectToAction(nameof(Details), new { id = employee.Id });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(nameof(Create), employee);
        }

        // GET: EmployeeController/Edit/5
        [HttpGet(nameof(Edit))]
        public async Task<ActionResult> Edit(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            string query = $"Select E.*,Asp.Email,D.Id as DepartmentId,D.Name as DepartmentName from Employee E JOIN Department D ON E.DepartmentId=D.Id JOIN AspNetUsers Asp ON E.AspNetUserId=Asp.Id Where ";
            if (id == 0)
            {
                IdentityUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
                query += $"E.AspNetUserId='{user.Id}'";
            }
            else
            {
                query += $"E.Id={id}";
            }

            EmployeeDto employee = await Task.FromResult(_dapperContent.Get<EmployeeDto>(query, null, commandType: CommandType.Text));
            await GetDepartments(employee);
            return View(employee);
        }

        // POST: EmployeeController/UpdateEmployee/
        [HttpPost(nameof(UpdateEmployee))]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateEmployee([FromForm] EmployeeDto employee)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            try
            {
                string query = $"Update Employee SET Name=@Name,Address=@Address,PhoneNumber=@PhoneNumber Where id={employee.Id}";
                employee.Id = await _dapperContent.Update(query, employee);
                await GetDepartments(employee);

                employee.SuccessMessage = "Employee details updated Successfully";

                return View(nameof(Edit), employee);
            }
            catch
            {
                employee.ErrorMessage = "There is some error, Please try again";
                return RedirectToAction(nameof(Edit));
            }
        }

        // GET: EmployeeController/Delete/5
        [HttpGet(nameof(Delete))]
        public async Task<ActionResult> Delete(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            IdentityUser? user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                string query = $" SELECT 1 FROM AspNetUserRoles ur INNER JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = '{user.Id}' AND r.Name = '{Roles.Admin.ToString()}'";
                int result = await Task.FromResult(_dapperContent.Get<int>(query, null, CommandType.Text));
                if (result != 1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@userId", user.Id);
            await Task.FromResult(_dapperContent.Execute("sp_DeleteUser", dynamicParameters,  CommandType.StoredProcedure));

            return RedirectToAction(nameof(GetAllEmployees));
        }

        private async Task<EmployeeDto> GetDepartments(EmployeeDto employee)
        {
            employee.Departments = new List<SelectListItem>();
            string query = $"Select * from Department";
            List<Department> departments = await Task.FromResult(_dapperContent.GetAll<Department>(query, null, commandType: CommandType.Text));
            employee.Departments.Add(new SelectListItem()
            {
                Value = "-1",
                Text = "Select",
                Selected = employee.DepartmentId <= 0
            });
            if (departments != null && departments.Any())
            {

                foreach (Department item in departments)
                {
                    employee.Departments.Add(new SelectListItem()
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name,
                        Selected = employee.DepartmentId == item.Id
                    });
                }
            }
            return employee;
        }
    }
}
