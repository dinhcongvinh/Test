using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NM.Models
{
    public class InforCustommer
    {
         [DataType(DataType.Date)]
        public string Custommer { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public string Date { get; set; }
        public string Note { get; set; }
    }
}