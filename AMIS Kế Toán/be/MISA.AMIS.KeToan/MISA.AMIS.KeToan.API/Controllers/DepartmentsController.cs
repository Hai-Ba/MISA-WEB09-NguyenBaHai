using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.Common.Entities;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [ApiController]
    public class DepartmentsController : BasesController<Department>
    {
        public DepartmentsController(IBaseBL<Department> baseBL) : base(baseBL)
        {
        }
    }
}
