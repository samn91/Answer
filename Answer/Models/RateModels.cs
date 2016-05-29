using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Answer.Models
{
    public class RateModels
    {
        [Key]
        public int RateId { get; set; }
        public virtual AnswerModels Answer { get; set; }
        public virtual UserProfile User { get; set; }

        public RateModels()
        {
        }
        public RateModels(AnswerModels a, UserProfile u)
        {
            Answer = a;
            User = u;
        }
    }
}