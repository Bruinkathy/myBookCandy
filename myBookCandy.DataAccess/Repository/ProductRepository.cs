using myBookCandy.DataAccess.Data;
using myBookCandy.DataAccess.Repository.IRepository;
using myBookCandy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myBookCandy.DataAccess.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private ApplicationDbContext _db;

		public ProductRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Product obj)
		{
			// Updates all properties in Product
			//_db.Products.Update(obj);

			// Updates only properties Edited
			var objFromDb = _db.Products.FirstOrDefault(u=>u.Id==obj.Id);
			if (objFromDb != null)
			{
				objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
				objFromDb.ListPrice = obj.ListPrice;
				objFromDb.Price50 = obj.Price50;
				objFromDb.Price100 = obj.Price100;
				objFromDb.Description = obj.Description;
				objFromDb.CategoryId = obj.CategoryId;
				objFromDb.CoverTypeId = obj.CoverTypeId;
				objFromDb.Author = obj.Author;
				if(objFromDb.ImageUrl != null)
				{
					objFromDb.ImageUrl = obj.ImageUrl;
				}
			}
		}
	}
}
