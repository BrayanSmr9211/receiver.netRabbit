using Newtonsoft.Json;

namespace WebApplication1.Models
{
    
    public class UserModel
    {
        public Int32 Id { get; set; }
        public DateTime datec { get; set; }
        public string gender  { get; set; }
        public string Companyposition { get; set; }
        public string name { get; set; }
        public string Email { get; set; }
        public Int32 Phone { get; set; }
        public Int32 Identification { get; set; }
        public bool active { get; set; }
    }

}
