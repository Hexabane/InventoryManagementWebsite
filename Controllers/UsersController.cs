using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FarmCentralApp.Models;
using FarmCentralApp.OOP;

namespace FarmCentralApp.Controllers
{//start of Namespace

    public class UsersController : Controller
    {//start of Controller

        //Static variables initialized so it can be globally used.
        public static int usernameId = 0;
        public static List<User> userList = new List<User>();
        public static List<Product> productList = new List<Product>();
        public static int check;
        public static string checkRole;

        //Check variables to check who is logged in.
        public static int farmer;
        public static int employee;


        ST10116273_FarmCentralContext db = new ST10116273_FarmCentralContext();

        private readonly ST10116273_FarmCentralContext _context;

        public UsersController(ST10116273_FarmCentralContext context)
        {
            _context = context;
        }

        //gets the list of users/farmers to display available for the employee
        public async Task<IActionResult> Index()
        {//start of Index() method

            //If the user tries to access any features of the website without logging in.
            if (usernameId == 0)
            {
                return RedirectToAction("Login", "Users");
            }
            //Checks whether if the user is a farmer and if true then the user is not allowed access to this page/feature
            if (checkRole.Equals("Farmer"))
            {
                ViewBag.Message1 = "Access Denied";
                return RedirectToAction("Index", "Home");
            }

            //These variables are created/initialized with values as checks with regards to the current role of the user and then the necessary view is displayed.
            ViewBag.roleEmployee = employee;
            ViewBag.roleFarmer = farmer;

            return View(await _context.Users.Where(x => x.UsersRole.Equals("Farmer")).ToListAsync());

        }//end of Index() method


        //A create account method which is only available for the employee
        public IActionResult Create()
        {//start of create() Method

            //If the user tries to access any features of the website without logging in.
            if (usernameId == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            //Checks whether if the user is a farmer and if true then the user is not allowed access to this page/feature
            if (checkRole.Equals("Farmer"))
            {
                ViewBag.Message1 = "Access Denied";
                return RedirectToAction("Index", "Home");
            }

            //These variables are created/initialized with values as checks with regards to the current role of the user and then the necessary view is displayed.
            ViewBag.roleEmployee = employee;
            ViewBag.roleFarmer = farmer;

            List<SelectListItem> roles = new()
            {
                new SelectListItem { Value = "Farmer", Text = "Farmer" },
                new SelectListItem { Value = "Employee", Text = "Employee" }
                
            };
            ViewBag.Role = roles;

            return View();

        }//end of create() Method.

    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int UsersId, string UsersFirstname, string usersSurname, string UsersPassword, string UsersRole)
        {//start of Create() Method after button is clicked.

            if (checkRole.Equals("Farmer"))
            {
                ViewBag.Message1 = "Access Denied";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.roleEmployee = employee;
            ViewBag.roleFarmer = farmer;

            User u = new User(UsersId, UsersFirstname, usersSurname, UsersPassword,UsersRole);
            if (ModelState.IsValid)
            {
                _context.Add(u);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("Index", "Users");

        }//end of Create() Method after button is clicked

        public static int cred = 0;
        public IActionResult Login()
        {//start of Login() Method

            Refresh();

            return View();

        }//end of Login() Method.


        [HttpPost]
        public IActionResult Login(string UsersFirstname, string UsersPassword)
        {//start of Login() method After button is clicked and values are entered

            try
            {//start of try statement
                Refresh();

                cred = 1;
                //UsersPassword = Utility.hashPassword(UserPassword); // hashes password

                List<User> loginCheck = new List<User>();
                loginCheck = _context.Users.Where(x => x.UsersFirstname.Equals(UsersFirstname) && x.UsersPassword.Equals(UsersPassword)).ToList();


                if (loginCheck[0].UsersPassword.Equals(UsersPassword) && loginCheck[0].UsersFirstname.Equals(UsersFirstname))
                {
                    cred = 1;
                    check = 1;

                    ViewBag.Check = check;
                    

                    //user list is isolated with the sepecfic user based on login
                    userList = _context.Users.Where(x => x.UsersPassword.Equals(UsersPassword) && x.UsersFirstname.Equals(UsersFirstname)).ToList();

                    ViewBag.message = "Successfully Logged In.";
                    usernameId = userList[0].UsersId;
                    checkRole = userList[0].UsersRole;

                    //lists are loaded with data based on userID
                    productList = _context.Products.Where(x => x.UsersId == (usernameId)).ToList();
                   

                
                    Products product;
                    Users user;

                  
                    //the foreach below populate the ListHandler with unique userdata
                    foreach (Product item in productList)
                    {
                        product = new Products(item.ProductType, item.ProductPrice, item.ProductDaterange);
                        ListHandler.pList.Add(product);
                    }

              
                    //sets the user data in the ListHandler Class.
                    user = new Users(userList[0].UsersId, userList[0].UsersFirstname, userList[0].UsersSurname, userList[0].UsersPassword, userList[0].UsersRole);
                    ListHandler.uList.Add(user);

                    //If statements created below to check which user is logged in.
                    if (userList[0].UsersRole.Equals("Farmer"))
                    {
                        farmer = 1;
                    }
                    if (userList[0].UsersRole.Equals("Employee"))
                    {
                        employee = 1;
                    }




                    return RedirectToAction("Index", "Home");
                }


            }//end of try statement
            catch (Exception)
            {
                cred++;
                if (cred > 1)
                {
                    ModelState.Clear();
                    ViewBag.Message = "Wrong Credentials";
                }
            }
            return View();

        }//end of Create() Method 2.


        //refreshes/resets certain variables.
        public void Refresh()
        {//start of refresh() method

            usernameId = 0;
            check = 0;
            ViewBag.Check = check;
            farmer = 0;
            employee = 0;
            ViewBag.roleFarmer = farmer;
            ViewBag.roleEmployee = employee;
        }//end of refresh() method

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {//start of Delete() Method

            //If the user tries to access any features of the website without logging in.
            if (usernameId == 0)
            {
                return RedirectToAction("Login", "Users");
            }
            //Checks whether if the user is a farmer and if true then the user is not allowed access to this page/feature
            if (checkRole.Equals("Farmer"))
            {
                ViewBag.Message1 = "Access Denied";
                return RedirectToAction("Index", "Home");
            }

            //These variables are created/initialized with values as checks with regards to the current role of the user and then the necessary view is displayed.
            ViewBag.roleEmployee = employee;
            ViewBag.roleFarmer = farmer;

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UsersId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);

        }//end of Delete() Method

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {//start of DeleteConfirmed() Method

            try
            {

                //If the user tries to access any features of the website without logging in.
                if (usernameId == 0)
                {
                    return RedirectToAction("Login", "Users");
                }
                //Checks whether if the user is a farmer and if true then the user is not allowed access to this page/feature
                if (checkRole.Equals("Farmer"))
                {
                    ViewBag.Message1 = "Access Denied";
                    return RedirectToAction("Index", "Home");
                }

                //These variables are created/initialized with values as checks with regards to the current role of the user and then the necessary view is displayed.
                ViewBag.roleEmployee = employee;
                ViewBag.roleFarmer = farmer;

                var user = await _context.Users.FindAsync(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.MessageDelete = "Please Delete all of the products of this user first!!";
                var user = await _context.Users.FirstOrDefaultAsync(m => m.UsersId == id);
                return View(user);
            }
            
           

        }//end of DeleteConfirmed() Method

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UsersId == id);
        }

       

        //// GET: Users/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(m => m.UsersId == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// GET: Users/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (usernameId == 0)
        //    {
        //        return RedirectToAction("Login", "Users");
        //    }
        //    if (checkRole.Equals("Farmer"))
        //    {
        //        ViewBag.Message1 = "Access Denied";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    ViewBag.roleEmployee = employee;
        //    ViewBag.roleFarmer = farmer;

        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UsersId,UsersFirstname,UsersSurname,UsersPassword,UsersRole")] User user)
        //{

        //    if (usernameId == 0)
        //    {
        //        return RedirectToAction("Login", "Users");
        //    }
        //    if (checkRole.Equals("Farmer"))
        //    {
        //        ViewBag.Message1 = "Access Denied";
        //        return RedirectToAction("Index", "Home");
        //    }

        //    ViewBag.roleEmployee = employee;
        //    ViewBag.roleFarmer = farmer;

        //    if (id != user.UsersId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.UsersId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

    }//end of class

}//end of namespace
