using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Answer.Models
{
    public class AnswerModels
    {
        [Key]
        public int AnswerId { get; set; }
        public virtual QuestionModels Question { get; set; }
        public virtual UserProfile User { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string AnswerText { get; set; }
        public DateTime Date { get; set; }
        public virtual List<RateModels> Rate { get; set; }

        public AnswerModels() {
            Rate = new List<RateModels>();
        }
    }
}