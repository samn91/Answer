using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Answer.Models
{
    public class QuestionModels
    {
        [Key]
        public int QuestionId { get; set; }
        public virtual UserProfile User { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Title { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 5)]
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int Views { get; set; }
        public virtual MajorModels Major { get; set; }
        public int Rate { get; set; }
        public virtual UserProfile ReferredUser { get; set; }
        public virtual List<AnswerModels> Answers { get; set; }

        public QuestionModels()
        {
            //User = new UserProfile();
            //Major = new MajorModels();
            Answers = new List<AnswerModels>();
        }

    }
}