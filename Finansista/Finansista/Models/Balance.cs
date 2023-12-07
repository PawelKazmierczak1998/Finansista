using Microsoft.AspNetCore.Identity;

namespace Finansista.Models
{
    public class Balance
    {
        public int Id { get; set; }
        public decimal accountBalance { get; set; }
        public string accountName { get; set; }
    }
}
