using System;
using System.Collections.Generic;
using System.Text;

namespace ExamApp.Model
{
    class MasterNavigationItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type Target { get; set; }
    }
}
