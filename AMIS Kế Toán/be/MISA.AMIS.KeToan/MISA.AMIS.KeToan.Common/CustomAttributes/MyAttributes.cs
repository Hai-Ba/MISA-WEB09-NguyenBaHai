using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MyAttributes : Attribute
    {
        public MyAttributes() { }
        public bool KeyProp { get; set; }
        public bool RequiredProp { get; set; }
        public bool ValidCode { get; set; }
        public bool ValidAge { get; set; }
        public bool ValidDate { get; set; }
        public bool ValidPhone { get; set; }
        public bool ValidEmail { get; set; }
        public bool ValidLengthNumber15 { get; set; }
    }
}
