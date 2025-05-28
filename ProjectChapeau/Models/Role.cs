namespace ProjectChapeau.Models
{
    public class Role
    {
        public int roleId { get; set; }
        public string roleName { get; set; }

        public Role(int roleId, string roleName)
        {
            this.roleId = roleId;
            this.roleName = roleName;
        }

        public Role()
        {
        }
    }
}
