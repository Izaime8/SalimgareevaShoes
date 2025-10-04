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
    /// Логика взаимодействия для ShoesPage.xaml
    /// </summary>
    public partial class ShoesPage : Page
    {
        public ShoesPage()
        {
            InitializeComponent();

            UpdateProduct();
        }

        void UpdateProduct()
        {
            var currentProducts = SalimgarevaShoesEntities.GetContext().Products.ToList();
            ShoesListView.ItemsSource = currentProducts;

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
