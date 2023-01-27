using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pytania.Models
{
    public class QuestionsTemplate
    {
#nullable enable
        public int? Index { get; set; }
#nullable disable
        public string Question { get; set; }
#nullable enable
        public string? Answer { get; set; }
#nullable disable
    }
}
