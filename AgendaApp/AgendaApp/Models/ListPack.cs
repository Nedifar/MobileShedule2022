using System;
using System.Collections.Generic;
using System.Text;

namespace AgendaApp.Models
{
    public class ListPack
    {
        public List<string> Groups { get; set; } = new List<string>();
        public List<string> Cabinets { get; set; } = new List<string>();
        public List<string> Teachers { get; set; } = new List<string>();
    }
}
