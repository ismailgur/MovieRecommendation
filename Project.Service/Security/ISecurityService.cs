using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Service.Security
{
    public interface ISecurityService
    {
        string GetSha256Hash(string input);
    }

}
