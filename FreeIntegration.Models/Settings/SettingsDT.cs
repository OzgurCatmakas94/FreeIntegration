using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreeIntegration.Models.Settings
{
    public class SettingsDT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string APIKey { get; set; }
        public string APISecret { get; set; }
        
    }
}
