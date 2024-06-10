using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User:BaseEntity
    {
        [StringLength(25)]
        required public string Username { get; set; }
        [StringLength(15)]
        required public string Password { get; set; }
        [StringLength(80)]
        required public string Email { get; set; }
        [StringLength(80)]
        required public string Country { get; set; }
        [StringLength(1000)]
        required public string Biography { get; set; }
        required public DateTime BirthDay { get; set; } 

    }
}
