using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Model.DB
{
    public class User : IdentityUser
    {
        public double UserRating { get; set; }
        public int DoneTaskNumber { get; set; }
    }
}
