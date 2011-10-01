using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Diagnostics;

namespace NFileAPI
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.dialog = new OpenFileDialog(){ Multiselect = true };
        }

        OpenFileDialog dialog;

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            dialog.ShowDialog();
        }

        [ScriptableMember]
        public FileList files {
            get {
                List<System.IO.FileInfo> list;
                try
                {
                    list = this.dialog.Files.ToList();
                }
                catch 
                {
                    list = new List<System.IO.FileInfo>(); 
                }
                return new FileList(list);
            }
        }
    }
}
