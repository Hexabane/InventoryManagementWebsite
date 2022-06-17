using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FarmCentralApp.Models;

//ProductsController class where all the product related razor views have all the backend code.
namespace FarmCentralApp.Controllers
{//start of namespace

    public class ProductsController : Controller
    {//start of ProductsController Class.

        private readonly ST10116273_FarmCentralContext _context;
        private static string chosenFarmer = "";

        public ProductsController(ST10116273_FarmCentralContext context)
        {
            _context = context;
        }



        //gets the list of products from the database for the logged in farmer and all the products for the logged in employee
        public async Task<IActionResult> Index(string searchString, DateTime searchDate, DateTime searchDate1, string ButtonSearch, string ButtonDate, string SearchFarmer, string ButtonFarmer)
        {//start of Index() Method

            //These variables are created/initialized with values as checks with regards to the current role of the user and then the necessary view is displayed.
            ViewBag.roleEmployee = UsersController.employee;
            ViewBag.roleFarmer = UsersController.farmer;

            //If the user tries to access any features of the website without logging in.
            if (UsersController.usernameId == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            //If the User role is employee then it eill provide the user with all products provided with the farmer with the feature to sort them accordingly.
            if (UsersController.checkRole.Equals("Employee"))
            {//start of employee if statement

                ViewBag.listFarmer = new SelectList(_context.Users.Where(x => x.UsersRole.Equals("Farmer")).Select(s => s.UsersFirstname));

                //this if statements sorts out the list based on the farmer when the button is clicked.
                if (!string.IsNullOrEmpty(ButtonFarmer))
                {

                    chosenFarmer = SearchFarmer;

                    ViewBag.MsgSelectedFarmer = "Sorted By Farmer : " + chosenFarmer;


                    ViewData["CurrentFarmer"] = SearchFarmer;

                    var ST10116273_FarmCentralContext3 = _context.Products.Include(p => p.Users).Where(x => x.Users.UsersFirstname.Contains(SearchFarmer));
                    return View(await ST10116273_FarmCentralContext3.ToListAsync());


                }
                //this if statement sorts out the list based on the product type when the button is clicked.
                else if (!string.IsNullOrEmpty(ButtonSearch))
                {
                    ViewBag.MsgSelectedFarmer = "Sorted By Farmer : " + chosenFarmer;

                    ViewData["CurrentFilter"] = searchString;
                    var ST10116273_FarmCentralContext1 = _context.Products.Include(p => p.Users).Where(x => chosenFarmer.Equals("") ? x.ProductType.Contains(searchString) : x.Users.UsersFirstname.Equals(chosenFarmer) && x.ProductType.Contains(searchString));
                    return View(await ST10116273_FarmCentralContext1.ToListAsync());


                }
                //this if statement sorts out the list based on the given date range when the button is clicked.
                else if (!string.IsNullOrEmpty(ButtonDate))
                {
                    ViewBag.MsgSelectedFarmer = "Sorted By Farmer : " + chosenFarmer;

                    ViewData["CurrentDate"] = searchDate;
                    ViewData["CurrentDate1"] = searchDate1;
                    var ST10116273_FarmCentralContext2 = _context.Products.Include(p => p.Users).Where(x => chosenFarmer.Equals("") ? x.ProductDaterange >= searchDate && x.ProductDaterange <= searchDate1 : x.Users.UsersFirstname.Equals(chosenFarmer) && x.ProductDaterange >= searchDate && x.ProductDaterange <= searchDate1);
                    return View(await ST10116273_FarmCentralContext2.ToListAsync());


                }

                //returns a normal list if none of the buttons are clicked.
                var ST10116273_FarmCentralContext = _context.Products.Include(p => p.Users).OrderByDescending(p => p.Users);
                return View(await ST10116273_FarmCentralContext.ToListAsync());

            }//end of employee if statement

            //If the User role is farmer then it will provide the user with all products for that specific farmer with the feature to sort them accordingly.
            if (UsersController.checkRole.Equals("Farmer"))
            {//start of farmer if satement

                ViewBag.deleteRemove = 0; //this variable is initialized as a check if the farmer is logged in then it will hide the features which are not available to the farmer.


                //this if statement sorts out the list based on the product type when the button is clicked.
                if (!string.IsNullOrEmpty(ButtonSearch))
                {


                    ViewData["CurrentFilter"] = searchString;
                    var ST10116273_FarmCentralContext1 = _context.Products.Include(p => p.Users).Where(x => x.UsersId == UsersController.usernameId && x.ProductType.Contains(searchString));
                    return View(await ST10116273_FarmCentralContext1.ToListAsync());


                }
                //this if statement sorts out the list based on the given date range when the button is clicked.
                else if (!string.IsNullOrEmpty(ButtonDate))
                {


                    ViewData["CurrentDate"] = searchDate;
                    ViewData["CurrentDate1"] = searchDate1;
                    var ST10116273_FarmCentralContext2 = _context.Products.Include(p => p.Users).Where(x => x.UsersId == UsersController.usernameId && x.ProductDaterange >= searchDate && x.ProductDaterange <= searchDate1);
                    return View(await ST10116273_FarmCentralContext2.ToListAsync());


                }

                //returns a normal list if none of the buttons are clicked.
                var ST10116273_FarmCentralContext = _context.Products.Include(p => p.Users).Where(x => x.UsersId == UsersController.usernameId);
                return View(await ST10116273_FarmCentralContext.ToListAsync());

            }//end of farmer if statement


            return View();

        }//end of Index() Method

        
        //gets the product details
        public async Task<IActionResult> Details(int? id)
        {//start of Details() Method

            if (UsersController.usernameId == 0)
            {
                return RedirectToAction("Login", "Users");
            }
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Users)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }//end of Details Method()


        //Method that allows the farmer to create a product and store the data in the database.
        public IActionResult Create()
        {//start of Create() Method


            //If statement checks if the current user is an employee if not then it will allow the user (farmer) to create the product.
            if (UsersController.checkRole.Equals("Employee"))
            {
                ViewBag.Message1 = "Access Denied";
                return RedirectToAction("Index", "Products");
            }

            //These variables are created/initialized with values as checks with regards to the current role of the user and then the necessary view is displayed.
            ViewBag.roleEmployee = UsersController.employee;
            ViewBag.roleFarmer = UsersController.farmer;


            //If the user tries to access any features of the website without logging in.
            if (UsersController.usernameId == 0)
            {
                return RedirectToAction("Login", "Users");
            }
            ViewData["UsersId"] = new SelectList(UsersController.userList.Select(x => x.UsersId));
            return View();

        }//end of Create() Method

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductType,ProductPrice,ProductDaterange,UsersId")] Product product)
        {//start of Create() Method after button is clicked.

            //If statement checks if the current user is an employee if not then it will allow the user (farmer) to create the product.
            if (UsersController.checkRole.Equals("Employee"))
            {
                ViewBag.Message1 = "Access Denied";
                return RedirectToAction("Index", "Products");
            }

            //These variables are created/initialized with values as checks with regards to the current role of the user and then the necessary view is displayed.
            ViewBag.roleEmployee = UsersController.employee;
            ViewBag.roleFarmer = UsersController.farmer;

            //If the user tries to access any features of the website without logging in.
            if (UsersController.usernameId == 0)
            {
                return RedirectToAction("Login", "Users");
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsersId"] = new SelectList(UsersController.userList.Select(x => x.UsersId));
            return View(product);

        }//end of Create() Method after button is clicked.

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {//start of Edit() Method
           
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (UsersController.checkRole.Equals("Farmer"))
            {
                ViewData["UsersId"] = new SelectList(UsersController.userList.Select(x => x.UsersId));
            }
            else
            {
                ViewData["UsersId"] = new SelectList(_context.Products.Where(x => x.ProductId == id).Select(x => x.UsersId));
            }
            return View(product);

        }//end of Edit() Method

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductType,ProductPrice,ProductDaterange,UsersId")] Product product)
        {//start of Edit() method after the button is clicked
            
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        
            return View(product);

        }//end of Edit() method after the button is clicked

        //Allows the Employee to delete the product
        public async Task<IActionResult> Delete(int? id)
        {//start of Delete() Method

            //If statement checks if the current user is an Farmer if not then it will allow the user (Employee) to delete the product.
            if (UsersController.checkRole.Equals("Farmer"))
            {
                ViewBag.Message1 = "Access Denied";
                return RedirectToAction("Index", "Products");
            }
          
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Users)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);

        }//end of Delete() Method

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

    }//end of ProductsController Class

}//end of namespace
