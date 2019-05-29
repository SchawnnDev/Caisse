using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseLibrary.test
{
    public struct Question
    {
        public string Chap;
        public string Content;
        public string Answer;

        public Question(string chap, string content, string answer)
        {
            Chap = chap;
            Content = content;
            Answer = answer;
        }
    }
}
