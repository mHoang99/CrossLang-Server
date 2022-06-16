using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossLang.ApplicationCore.Entities
{
    [TableName("exercise_question_mapping")]
    class ExerciseQuestionMapping : BaseEntity
    {
        [DBColumn]
        [DisplayName("Bài tập")]
        public long? ExerciseID { get; set; }

        [DBColumn]
        [DisplayName("Câu hỏi")]
        public long? QuestionID { get; set; }
    }
}
