using Microsoft.AspNetCore.Mvc;
using myBookCandy.DataAccess.Repository.IRepository;
using myBookCandy.Models;

namespace myBookCandyWeb.Areas.Admin.Controllers
{
	public class CoverTypeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
			return View(objCoverTypeList);
		}


		// CREATE -- GET
		public IActionResult Create()
		{
			return View();
		}

		// CREATE -- POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(CoverType obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.CoverType.Add(obj);
				_unitOfWork.Save();
                TempData["success"] = "~ Cover Type Created Successfully ~";
				return RedirectToAction("Index");
            }
			return View(obj);
		}


		// EDIT -- GET
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

			if(coverTypeFromDbFirst == null)
			{
				return NotFound();
			}
			return View(coverTypeFromDbFirst);
		}

		// EDIT -- POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(CoverType obj)
		{
			if(ModelState.IsValid)
			{
				_unitOfWork.CoverType.Update(obj);
				_unitOfWork.Save();
                TempData["success"] = "~ Cover Type Updated Successfully ~";
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

            var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

			if(coverTypeFromDbFirst == null)
			{
				return NotFound();
			}
			return View(coverTypeFromDbFirst);

        }

		// DELETE -- POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePOST(int? id)
		{
			var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

			if (obj == null)
			{
				return NotFound();
			}

			_unitOfWork.CoverType.Remove(obj);
			_unitOfWork.Save();
            TempData["success"] = "~ Cover Type Deleted Successfully ~";
			return RedirectToAction("Index");
        }
	}
}
