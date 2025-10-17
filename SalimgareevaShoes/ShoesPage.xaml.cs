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


            SortCB.SelectedIndex = 0;
            SupplersCB.SelectedIndex = 0;
            UpdateProduct();

        }

        void UpdateProduct()
        {
            var currentProducts = SalimgarevaShoesEntities.GetContext().Products.ToList();

            var searchedProducts = currentProducts;
            foreach (string s in SearchTB.Text.Split())
            {
                searchedProducts = searchedProducts.Where(p => p.ProductArticle.ToLower().Contains(s.ToLower()) ||
                p.ProductName.ToLower().Contains(s.ToLower()) ||
                p.ProductUnitMeasure.ToLower().Contains(s.ToLower()) ||
                p.Supplers.SupplerName.ToLower().Contains(s.ToLower()) ||
                p.Manufacturers.ManufacturerName.ToLower().Contains(s.ToLower()) ||
                p.Categories.CategoryName.ToLower().Contains(s.ToLower()) ||
                p.ProductDescription.ToLower().Contains(s.ToLower())).ToList();
            }

            if (searchedProducts != null)
                currentProducts = searchedProducts;



            if (SupplersCB.SelectedIndex == 1)
            {
                currentProducts = currentProducts.Where(p => p.SupplerID == 1).ToList();
            }
            if (SupplersCB.SelectedIndex == 2)
            {
                currentProducts = currentProducts.Where(p => p.SupplerID == 2).ToList();
            }



            if (SortCB.SelectedIndex == 1)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductQuantityInStock).ToList();
            }
            if (SortCB.SelectedIndex == 2)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductQuantityInStock).ToList();
            }






            ShoesListView.ItemsSource = currentProducts;

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();

        }

       

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();

        }

        private void SupplereCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();

        }
    }
}
