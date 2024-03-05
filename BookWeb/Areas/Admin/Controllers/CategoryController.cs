using Microsoft.AspNetCore.Mvc;
using Book.DataAccess.Data;
using Book.Models;
using Book.DataAccess.Repository.IRepository;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)//get data from post category input and assign to obj
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }
            //if (obj.Name != null && obj.Name.ToLower()=="test")
            //{
            //	ModelState.AddModelError("", "Test is invalid value.");// in "" is empty so wen the name == test is call the statement but not has the feild (like name : Name Test is an...)in head
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);//allow the change of data
                _unitOfWork.Save();//go to the database and create a new category
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");//when the list update, it must go to the view() and again and again, so this line  make it go back to the list and show it
            }
            return View();
        }

        public IActionResult Edit(int? id)// id can be a null value
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)//get data from post category input and assign to obj
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)// id can be a null value
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)//get data from post category input and assign to obj
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
