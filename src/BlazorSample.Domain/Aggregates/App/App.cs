using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorSample.Domain.Infra;

namespace BlazorSample.Domain.Aggregates.App
{
    public class Application : BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<UIResource> UIResources { get; set; }
        public ICollection<APIResource> APIResources { get; set; }
    }

    public class Role : BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<UIResourceRole> UIResourceRoles { get; set; }
        public ICollection<APIResourceRole> APIResourceRoles { get; set; }
        public ICollection<User> Users { get; set; }
        public int ApplicationId { get; set; }
    }

    public class UIResource : BaseEntity<int>
    {
        public string Name { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public ICollection<UIResourceRole> UIResourceRoles { get; set; }
    }

    public class APIResource : BaseEntity<int>
    {
        public string Name { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public ICollection<APIResourceRole> APIResourceRoles { get; set; }
    }

    public class User : BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<Role> Roles { get; set; }
    }

    public class UIResourceRole : BaseEntity<int>
    {
        public int UIResourceId { get; set; }
        public UIResource UIResource { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class APIResourceRole : BaseEntity<int>
    {
        public int APIResourceId { get; set; }
        public APIResource APIResource { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
