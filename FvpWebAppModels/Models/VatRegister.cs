﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FvpWebAppModels.Models
{
    [Table("VatRegisters")]
    public class VatRegister
    {
        [Key]
        public int VatRegisterId { get; set; }
        public int TargetDocumentSettingsId { get; set; }
        public decimal VatValue { get; set; }
        public int ErpVatRegisterId { get; set; }
    }
}