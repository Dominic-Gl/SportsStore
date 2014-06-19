using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.WebUI.Infrastructure.Abstracts
{
    public interface IAuthProvider
    {
        bool Authenticate(string name, string password);
    }
}