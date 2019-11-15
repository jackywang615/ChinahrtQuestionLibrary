using System;
using System.Collections.Generic;

namespace ChinahrtQuestionLibrary.Models
{
    public class Question
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public virtual List<Answer> Answers { get; set; } = new List<Answer>();
    }

    public class Answer
    {
        public Guid Id { get; set; }

        public string Result { get; set; }

        public bool IsRight { get; set; }
    }
}
