using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GuitarStore.Models.Data
{
    public class Db :DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }
        public DbSet<CategoryDTO> Categories { get; set; }
        public DbSet<ProductDTO> Products { get; set; }
        public DbSet<UserDTO> Users { get; set; }
        public DbSet<RoleDTO> Roles { get; set; }
        public DbSet<UserRoleDTO> UserRoles { get; set; }

        public System.Data.Entity.DbSet<GuitarStore.Models.ViewModels.Shop.ProductVM> ProductVMs { get; set; }
    }
}