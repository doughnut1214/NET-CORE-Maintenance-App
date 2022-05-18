using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TicketApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Administration : Controller
    {
        //Identity has built in classes for roles, no need to make custom models 
        private RoleManager<IdentityRole> roleManager;
        public Administration(RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult AllRoles()
        {
            
            return View(roleManager.Roles);
        }
        public IActionResult CreateRole() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                //else
                //    Errors(result);
            }
            return View(name);
        }
    }
}
