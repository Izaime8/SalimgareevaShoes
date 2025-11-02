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

namespace SalimgareevaShoes
{
    /// <summary>
    /// Логика взаимодействия для ViewOrdersPage.xaml
    /// </summary>
    public partial class ViewOrdersPage : Page
    {
        public ViewOrdersPage()
        {
            InitializeComponent();
            OrdersListView.ItemsSource = SalimgarevaShoesEntities.GetContext().Orders.ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SalimgarevaShoesEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(c => c.Reload());
            OrdersListView.ItemsSource = SalimgarevaShoesEntities.GetContext().Orders.ToList();
        }
    }
}
