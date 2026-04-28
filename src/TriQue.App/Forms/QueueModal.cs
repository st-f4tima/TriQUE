using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trique.Forms
{
    public partial class QueueModal : Form
    {
        private string _route;

        public QueueModal(string route)
        {
            InitializeComponent();
            _route = route;
            this.Text = route;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}