namespace edurate.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using edurate.Web.Infrastructure;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<edurate.Web.Infrastructure.EdurateDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        

        protected override void Seed(edurate.Web.Infrastructure.EdurateDb context)
        {
            
            context.Categories.AddOrUpdate(c => c.Name,
                new Category() { Name = "Computer Science" },
                new Category() { Name = "Mathematics" },
                new Category() { Name = "Humanities" },
                new Category() { Name = "Medicine" },
                new Category() { Name = "Law" },
                new Category() { Name = "Finance" },
                new Category() { Name = "Physics" }
                );

            SeedMembership();
        }
        private void SeedMembership()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                "Users", "UserId", "Email", autoCreateTables: true);

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            #region Adding Roles
            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }
            if (!roles.RoleExists("Instructor"))
            {
                roles.CreateRole("Instructor");
            }
            if (!roles.RoleExists("Company"))
            {
                roles.CreateRole("Company");
            }
            if (!roles.RoleExists("User"))
            {
                roles.CreateRole("User");
            }
            #endregion

            #region Adding Users
            if (membership.GetUser("zoya.k@zerek.kz", false) == null)
            {
                membership.CreateUserAndAccount("zoya.k@zerek.kz", "1234567");
            }

            if (membership.GetUser("zhanymgul.k@zerek.kz", false) == null)
            {
                membership.CreateUserAndAccount("zhanymgul.k@zerek.kz", "1234567");
            }
            if (membership.GetUser("igibek.k@zerek.kz", false) == null)
            {
                membership.CreateUserAndAccount("igibek.k@zerek.kz", "1234567");
            }
            #endregion

            #region Asigning User's Role
            if (!roles.GetRolesForUser("igibek.k@zerek.kz").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "igibek.k@zerek.kz" }, new[] { "Admin" });
            }
            if (!roles.GetRolesForUser("igibek.k@zerek.kz").Contains("Instructor"))
            {
                roles.AddUsersToRoles(new[] { "igibek.k@zerek.kz" }, new[] { "Instructor" });
            }
            
            if (!roles.GetRolesForUser("zoya.k@zerek.kz").Contains("Instructor"))
            {
                roles.AddUsersToRoles(new[] { "zoya.k@zerek.kz" }, new[] { "Instructor" });
            }

            if (!roles.GetRolesForUser("zhanymgul.k@zerek.kz").Contains("Instructor"))
            {
                roles.AddUsersToRoles(new[] { "zhanymgul.k@zerek.kz" }, new[] { "Instructor" });
            }
            #endregion

        }
    }
}
