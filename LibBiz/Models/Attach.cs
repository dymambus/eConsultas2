using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBiz.Models
{
    public class Attach
    {
        public int AttachId { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}