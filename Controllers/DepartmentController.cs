using EmployeeManagement.Constants;
using EmployeeManagement.DapperContent;
using EmployeeManagement.Modals;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmployeeManagement.Controllers
{
    [Route("api/[Controller]")]
    public class DepartmentController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IDapperContent _dapperContent;

        public DepartmentController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IDapperContent dapperContent)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dapperContent = dapperContent;
        }

        // GET: DepartmentController/Create
        [HttpGet(nameof(Create))]
        public ActionResult Create()
        {
            Department department = new Department();
            return View(department);
        }

        // GET: EmployeeController/Details/5
        [Route("~/api/Department")]
        [HttpGet(nameof(GetAllDepartments))]
        public async Task<ActionResult> GetAllDepartments()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            IdentityUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                string query = $" SELECT 1 FROM AspNetUserRoles ur INNER JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = '{user.Id}' AND r.Name = '{Roles.Admin.ToString()}'";
                int result = await Task.FromResult(_dapperContent.Get<int>(query, null, commandType: CommandType.Text));
                if (result != 1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            string departmentQuery = $"Select * from Department";
            List<Department> departments = await Task.FromResult(_dapperContent.GetAll<Department>(departmentQuery, null, commandType: CommandType.Text));
            return View(departments);
            
        }

        // POST: DepartmentController/Create
        [HttpPost(nameof(CreateDepartment))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDepartment([FromForm] Department department)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            IdentityUser? user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                string query = $" SELECT 1 FROM AspNetUserRoles ur INNER JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = '{user.Id}' AND r.Name = '{Roles.Admin.ToString()}'";
                int result = await Task.FromResult(_dapperContent.Get<int>(query, null, commandType: CommandType.Text));
                if (result != 1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            try
            {
                string query = $"Select * from Department";
                List<Department> departments = await Task.FromResult(_dapperContent.GetAll<Department>(query, null, commandType: CommandType.Text));
                if (departments != null)
                {
                    var existingDepartment = departments.Where(x => string.Equals(x.Name, department.Name.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (existingDepartment != null)
                    {
                        department.Errors.Add("There is already department with same name");
                        return View(nameof(Create), department);
                    }
                    else
                    {
                        department.Name = department.Name.Trim();
                        string updateQuery = "Insert into Department Values(@Name); SELECT CAST(SCOPE_IDENTITY() as int);";
                        department.Id=await _dapperContent.Insert(updateQuery, department);
                        departments.Add(department);
                    }
                }
                return View(nameof(GetAllDepartments), departments);
            }
            catch(Exception ex)
            {

            }
            return View();
        }

        // GET: DepartmentController/Edit/5
        [HttpGet(nameof(Edit))]
        public async Task<ActionResult> Edit(int id)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            IdentityUser? user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                string query = $" SELECT 1 FROM AspNetUserRoles ur INNER JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = '{user.Id}' AND r.Name = '{Roles.Admin.ToString()}'";
                int result = await Task.FromResult(_dapperContent.Get<int>(query, null, commandType: CommandType.Text));
                if (result != 1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            string departmentQuery = $"Select * from Department Where Id={id}";
            Department department = await Task.FromResult(_dapperContent.Get<Department>(departmentQuery, null, commandType: CommandType.Text));
            
            return View(department);
        }

        // POST: DepartmentController/UpdateDepartment/5
        [HttpPost(nameof(UpdateDepartment))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateDepartment([FromForm] Department department)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            IdentityUser? user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                string query = $" SELECT 1 FROM AspNetUserRoles ur INNER JOIN AspNetRoles r ON ur.RoleId = r.Id WHERE ur.UserId = '{user.Id}' AND r.Name = '{Roles.Admin.ToString()}'";
                int result = await Task.FromResult(_dapperContent.Get<int>(query, null, commandType: CommandType.Text));
                if (result != 1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            try
            {
                if (department != null && department.Id > 0)
                {
                    string query = $"Select * from Department";
                    List<Department> departments = await Task.FromResult(_dapperContent.GetAll<Department>(query, null, commandType: CommandType.Text));
                    if (departments != null)
                    {
                        var existingDepartment = departments.Where(x => string.Equals(x.Name, department.Name.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (existingDepartment != null)
                        {
                            department.Errors.Add("There is already department with same name");
                            return View(nameof(Edit), department);
                        }
                        else
                        {
                            department.Name = department.Name.Trim();
                            string updateQuery = "Update Department Set Name=@Name Where Id=@Id";
                            await _dapperContent.Update(updateQuery, department);
                            
                            departments.Where(x => x.Id == department.Id).ToList().ForEach(x => x.Name = department.Name);
                        }
                    }
                    return View(nameof(GetAllDepartments), departments);
                }
            }
            catch(Exception ex) 
            {

            }
            return View();
        }

        // GET: DepartmentController/Delete/5
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
                int result = await Task.FromResult(_dapperContent.Get<int>(query, null, commandType: CommandType.Text));
                if (result != 1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            string departmentQuery = $"Select * from Department";
            List<Department> departments = await Task.FromResult(_dapperContent.GetAll<Department>(departmentQuery, null, commandType: CommandType.Text));
            string employeeQuery = $"Select * from Employee Where DepartmentId={id}";
            List<EmployeeDto> employees = await Task.FromResult(_dapperContent.GetAll<EmployeeDto>(employeeQuery, null, commandType: CommandType.Text));
            if(employees != null && employees.Any())
            {
                departments.Where(x=>x.Id==id).ToList().ForEach(x=>x.Errors.Add($"{x.Name} is linked to some employees, you cannot delete it"));
            }
            else
            {
                string deleteQuery = $"Delete Department Where Id = {id}";
                var result = await _dapperContent.Delete(deleteQuery);
            }
            return View(nameof(GetAllDepartments),departments);
        }

    }
}
