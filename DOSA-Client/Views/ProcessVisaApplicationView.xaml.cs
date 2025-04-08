using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DOSA_Client.ViewModels;

namespace DOSA_Client.Views
{
    /// <summary>
    /// Interaction logic for ProcessVisaApplicationView.xaml
    /// </summary>
    public partial class ProcessVisaApplicationView : UserControl
    {
        public ProcessVisaApplicationView()
        {
            InitializeComponent();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ProcessVisaApplicationViewModel vm)
            {
                vm.Load((bool)e.NewValue);
            }
        }
    }
}
