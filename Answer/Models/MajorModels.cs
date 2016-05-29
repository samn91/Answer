using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Answer.Models
{
    public class MajorModels
    {
        [Key]
        public int MajorId { get; set; }
        public string MajorType { get; set; }

        public MajorModels()
        {
        }
        public MajorModels(string s)
        {
            MajorType = s;
        }
        public MajorModels(int id,string s) 
        {
            MajorId = id;
            MajorType = s;
        }

    }
}