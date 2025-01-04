using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTrack.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string User_Name { get; set; }

        public string UserEmail { get; set; }

        public int UserPhone { get; set; }

        public string UserGender {  get; set; }

        public string Password { get; set; }


        
    }
}
