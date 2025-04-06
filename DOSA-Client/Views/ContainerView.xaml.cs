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
    }
}
