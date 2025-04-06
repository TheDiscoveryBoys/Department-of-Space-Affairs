using System.Windows;
using System.Windows.Controls;
using DOSA_Client.ViewModels;
namespace DOSA_Client.Views
{
    public partial class ContainerView : UserControl
    {
        public ContainerView()
        {
            InitializeComponent();
            this.DataContext = new ContainerViewModel();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ContainerViewModel vm)
            {
                vm.OnVisibilityChanged((bool)e.NewValue);
            }
        }

    }


}
