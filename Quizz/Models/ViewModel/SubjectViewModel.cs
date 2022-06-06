using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.Models.ViewModel
{
    public class SubjectViewModel
    {
        public int SubjectId { get; set; }
        public string Thumbnail { get; set; }
        public string SubjectName { get; set; }
        public int LevelId { get; set; }
        public string LevelThumbnail { get; set; }
        public string LevelName { get; set; }
        public int TotalQues { get; set; }
    }
}