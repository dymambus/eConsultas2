﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBiz.Models
{
    public class Photograph
    {
        public int Id { get; set; } // Identificador único da foto
        public byte[] ImageData { get; set; } // Dados binários da imagem
        
    }
}
