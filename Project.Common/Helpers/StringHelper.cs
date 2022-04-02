using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Helpers
{
    public static class StringHelper
    {
        public static string AlwaysConvert(object str)
        {
            try
            {
                return str.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
