using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Microsoft.AspNet.Identity.EntityFramework;

namespace eLifeApi.Models
{
    [Serializable]
    public class Person
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }


        public Person(int _id, string _name, string _email)
        {
            Id = _id;
            Name = _name;
            Email = _email;
        }
    }
}