using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class UsersRoles
    {
        public Roles roles { set; get; }
        public Users users { set; get; }
        public Delegaciones delegaciones { set; get; }
    }
}