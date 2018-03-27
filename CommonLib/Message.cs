using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    // メッセージ
    [Serializable]
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return $@"{{ {nameof(Id)} = {Id}, {nameof(Content)} = ""{Content}"" }}";
        }
    }
}