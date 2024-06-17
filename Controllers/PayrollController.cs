using EmployeeManagement.DapperContent;
using EmployeeManagement.Modals;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    public class PayrollController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IDapperContent _dapperContent;

        public PayrollController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager,IDapperContent dapperContent)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dapperContent = dapperContent;
        }
        // GET: PayrollController
        [Route("~/api/Payroll")]
        [HttpGet(nameof(Index))]
        public async Task<ActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            PayrollDto payrollDto = new PayrollDto();
            var dateValue = DateTime.Now;
            int day = (int)dateValue.DayOfWeek;
            var currentDate = dateValue.AddDays((int)DayOfWeek.Sunday - day);
            var weekendDate = dateValue.AddDays((int)DayOfWeek.Saturday - day);
            IdentityUser? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user==null)
            {
                return RedirectToAction("Index", "Logout");
            }
            string query = $"Select * from Payroll Where AspNetUserId='{user.Id}' AND WorkedDate BETWEEN '{currentDate.Date}' AND '{weekendDate.Date}' Order By WorkedDate ASC";
            List<Payroll> dbPayrolls = await Task.FromResult(_dapperContent.GetAll<Payroll>(query, null, commandType: CommandType.Text));
            if(dbPayrolls != null && dbPayrolls.Any())
            {
                foreach (var payroll in dbPayrolls)
                {
                    payrollDto.HourlyRate = payroll.HourlyRate;
                    payrollDto.Payrolls.Add(new Payroll()
                    {
                        Id = payroll.Id,
                        AspNetUserId=payroll.AspNetUserId,
                        HourlyRate=payroll.HourlyRate,
                        HoursWorked=payroll.HoursWorked,
                        WorkedDate=payroll.WorkedDate,
                        WeekDay=payroll.WorkedDate.DayOfWeek.ToString(),
                    });
                }
                payrollDto.IsDataSubmitted=true;
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    payrollDto.Payrolls.Add(new Payroll()
                    {
                        WorkedDate = currentDate.AddDays(i).Date,
                        WeekDay = currentDate.AddDays(i).DayOfWeek.ToString()
                    });
                }
            }

            payrollDto.DateRanges.Add(new SelectListItem()
            {
                Value = $"{currentDate.ToString("MM/dd/yyyy")} - {weekendDate.ToString("MM/dd/yyyy")}",
                Text = $"{currentDate.ToString("MM/dd/yyyy")} - {weekendDate.ToString("MM/dd/yyyy")}"
            });

            
            return View(payrollDto);
        }


        // POST: PayrollController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePayroll([FromForm] PayrollDto payrollsDto)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Login");
            }
            IdentityUser? user=await _userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                if (payrollsDto != null && user != null)
                {
                    if (payrollsDto.Payrolls != null && payrollsDto.Payrolls.Any())
                    {
                        payrollsDto.Payrolls.ForEach(x =>
                        {
                            x.HourlyRate = payrollsDto.HourlyRate;
                            x.AspNetUserId = user.Id;
                        });
                    }
                    

                    if(!payrollsDto.IsDataSubmitted)
                    {
                        string query = "INSERT INTO Payroll(AspNetUserId,HoursWorked,HourlyRate,WorkedDate) Values(@AspNetUserId,@HoursWorked,@HourlyRate,@WorkedDate)";
                        await _dapperContent.Update(query, payrollsDto.Payrolls);

                        string vacationQuery = $"UPDATE Vacation SET NumberOfDays=NumberOfDays+1 Where EmployeeId IN (SELECT Id FROM Employee WHERE AspNetUserId=@Id)";
                        await _dapperContent.Update(vacationQuery, user);

                        payrollsDto.SuccessMessage = "Payroll updated Successfully";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                payrollsDto.ErrorMessage = "There is some error, Please try again";
            }
            return View(nameof(Index), payrollsDto);
        }

    }
}
