using Finance_Management.Data;
using Finance_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Finance_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {

        private readonly DataContext _dataContext;
        public CategoriesController(DataContext dataContext)
        {
            _dataContext = dataContext;   
        }
        [HttpGet]
        public Task<List<Category>> GetCategories()
        {
            var categories = _dataContext.categories.ToListAsync();
            
            return categories;
        }
    }
}
