using System;
namespace AuthSystemBackend.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string TypeOfUser  { get; set; }
        public string State { get; set; }
        public string City  { get; set; }
        public string Password { get; set; }
    }
}
