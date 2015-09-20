using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetMeReadIt.Models
{
    public class LetMePage
    {
        public bool IsParsed { get; set; }
        public ParsedPage ParsedPage { get; set; }
        public Uri NotParsedPage { get; set; }
    }
}
