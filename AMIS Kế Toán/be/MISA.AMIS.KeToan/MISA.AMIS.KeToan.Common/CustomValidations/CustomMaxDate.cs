using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.CustomValidations
{
    public class CustomMaxDate : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value != null) 
            {
                return Convert.ToDateTime(value) <= DateTime.Now;
            }
            return true;
        }
    }
}
