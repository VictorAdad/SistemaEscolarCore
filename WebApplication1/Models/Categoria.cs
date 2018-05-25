﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener de 2 a 50 caracteres.")]
        public string Nombre { get; set; }
        [StringLength(256, ErrorMessage = "La descripción no debe exceder los 256 caracteres.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public Boolean Estado { get; set; }
    }
}
