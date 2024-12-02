﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaNumberDTO
    {
        public int VillaNo { get; set; }

        [Required] public int VillaId { get; set; }
        public string SpecialDetails { get; set; }

        public VillaDTO villa { get; set; }
    }
}