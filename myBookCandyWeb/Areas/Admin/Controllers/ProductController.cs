using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myBookCandy.DataAccess.Repository.IRepository;
using myBookCandy.Models;
using myBookCandy.Models.ViewModels;
using System.Collections.Generic;

namespace myBookCandyWeb.Areas.Admin.Controllers
{
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		// This is for Img File Upload
		private readonly IWebHostEnvironment _hostEnvironment;

		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
            // This is for Img File Upload
            _hostEnvironment = hostEnvironment;
		}


		public IActionResult Index()
		{
            // If Not Using DataTables:
            IEnumerable<Product> ProductList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return View(ProductList);
		}


		// UPSERT -- GET
		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new()
			{
				Product = new(),
				CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
				CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
			};

			if(id==null || id == 0)
			{
				// Create Product
				
                return View(productVM);
            }
			else
			{
                // Update Product
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                
				if (productVM.Product == null)
                {
                    return NotFound();
                }

                return View(productVM);
            }
		}

        // UPSERT -- POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product Created successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product Updated successfully";
                }

                return RedirectToAction("Index");
            }

            return View(obj);
        }



        // DELETE -- GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            
            // Update Product
            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (productVM.Product == null)
            {
                return NotFound();
            }

            return View(productVM);
        }


        // DELETE -- POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["error"] = "~ Product Deleted successfully ~";
            return RedirectToAction("Index");
        }
    }
}
