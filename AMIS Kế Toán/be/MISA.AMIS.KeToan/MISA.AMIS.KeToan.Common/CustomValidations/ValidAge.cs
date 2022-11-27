using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.CustomValidations
{
    public class ValidAge : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            //var employee = (Employee)validationContext.ObjectInstance;

            if (value != null) 
            {
                var age = DateTime.Today.Year - Convert.ToDateTime(value).Year;
                return (age <= 50 && age >= 18);
            }
            return true;
        }
    }
}
