using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ServiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        

        public void Update(Service service)
        {
            var serviceFromDb = _db.Service.Find(service.Id);

            serviceFromDb.Name = service.Name;
            serviceFromDb.Price = service.Price;
            serviceFromDb.CategoryId = service.CategoryId;
            serviceFromDb.FrequencyId = service.FrequencyId;
            serviceFromDb.LongDesc = service.LongDesc;
            serviceFromDb.ImageUrl = service.ImageUrl;

            _db.SaveChanges();
        }
    }
}
