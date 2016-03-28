using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
	{
        public override IQueryable<Product> All()
        {
            return base.All().Where(p => p.IsDelete == false);
        }

        //�p�n���o���㪺all�A���L�o
        public IQueryable<Product> All(bool trueAll)
        {
            if (trueAll)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }


        public Product Find(int id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public override void Delete(Product entity)
        {
            entity.IsDelete = true;
        }
    }

	public  interface IProductRepository : IRepository<Product>
	{

	}
}