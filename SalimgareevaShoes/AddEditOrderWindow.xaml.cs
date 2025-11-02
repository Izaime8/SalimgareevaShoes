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
using System.Windows.Shapes;

namespace SalimgareevaShoes
{
    /// <summary>
    /// Логика взаимодействия для AddEditOrderWindow.xaml
    /// </summary>
    public partial class AddEditOrderWindow : Window
    {
        Orders currentOrder = new Orders();
        public AddEditOrderWindow(Orders selectedOrder)
        {
            InitializeComponent();

            DeliveryDateDP.SelectedDate = DateTime.Today;
            PickUpPointsCB.ItemsSource = SalimgarevaShoesEntities.GetContext().PickUpPoints.ToList();
            UsersCB.ItemsSource = SalimgarevaShoesEntities.GetContext().Users.ToList();
            StatusCB.ItemsSource = SalimgarevaShoesEntities.GetContext().OrderStatus.ToList();
            ProductsCB.ItemsSource = SalimgarevaShoesEntities.GetContext().Products.ToList();

            if (selectedOrder != null)
            {
                currentOrder = selectedOrder;

                DeliveryDateDP.SelectedDate = currentOrder.OrderDelivertDate;
                PickUpPointsCB.SelectedItem = currentOrder.PickUpPoints;
                UsersCB.SelectedItem = currentOrder.Users;
                StatusCB.SelectedItem = currentOrder.OrderStatus;
            }
            else
            {
                ShoesListView.Visibility = Visibility.Collapsed;
                AddProductsButton.Visibility = Visibility.Collapsed;
                ProductsCB.Visibility = Visibility.Collapsed;
                ProductQuantityInOrder.Visibility = Visibility.Collapsed;
                AddProductsButton.Visibility = Visibility.Collapsed;
                ProductsInOrderTB.Visibility = Visibility.Collapsed;
                ProductQuantityInOrderTB.Visibility = Visibility.Collapsed;
                ProductInOrderTB.Visibility = Visibility.Collapsed;
            }


            UpdateProducts();
            DataContext = currentOrder;
        }

        void UpdateProducts()
        {

            if (SalimgarevaShoesEntities.GetContext().OrderProduct.Where(o => o.OrderID == currentOrder.OrderID).FirstOrDefault() != null)
            {
                ShoesListView.ItemsSource = SalimgarevaShoesEntities.GetContext().OrderProduct.Where(o => o.OrderID == currentOrder.OrderID).ToList();
                ProductsInOrderTB.Text = "Продукты в заказе ";
                ShoesListView.Visibility = Visibility.Visible;


            }
            else
            {
                ProductsInOrderTB.Text = "Продукты в заказе отсутствуют";
                ShoesListView.Visibility = Visibility.Collapsed;
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrder.OrderCode <= 0)
            {
                MessageBox.Show("Введите положительный код");
                return;
            }
            if (SalimgarevaShoesEntities.GetContext().Orders.Where(o => o.OrderCode == currentOrder.OrderCode && o.OrderID != currentOrder.OrderID).FirstOrDefault() != null)
            {
                MessageBox.Show("Такой артикул заказа уже существует");
                return;
            }

            currentOrder.OrderDate = DateTime.Today;
            currentOrder.OrderDelivertDate = Convert.ToDateTime( DeliveryDateDP.SelectedDate);
            currentOrder.UserID = ((Users)UsersCB.SelectedItem).UserID;
            currentOrder.PickUpPointID = ((PickUpPoints)PickUpPointsCB.SelectedItem).PickUpPointID;
            currentOrder.OrderStatusID = ((OrderStatus)StatusCB.SelectedItem).OrderStatusID;


            if (currentOrder.OrderID == 0)
                SalimgarevaShoesEntities.GetContext().Orders.Add(currentOrder);
            try
            {
                SalimgarevaShoesEntities.GetContext().SaveChanges();
                MessageBox.Show("Заказ сохранен");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void AddProductsButton_Click(object sender, RoutedEventArgs e)
        {
            Products product = ((Products)ProductsCB.SelectedItem);
            if (SalimgarevaShoesEntities.GetContext().OrderProduct.Where(o => o.OrderID == currentOrder.OrderID && o.ProductID == product.ProductID).FirstOrDefault() != null)
            {
                MessageBox.Show("Товар уже есть в заказе");
                return;
            }
            if (string.IsNullOrWhiteSpace(ProductQuantityInOrder.Text))
            {
                MessageBox.Show("Введите количество товара в заказе");
                return;
            }


            OrderProduct newOrderProduct = new OrderProduct();

            newOrderProduct.ProductID = product.ProductID;
            newOrderProduct.OrderID = currentOrder.OrderID;
            newOrderProduct.ProductQuantity = Convert.ToInt32(ProductQuantityInOrder.Text);
            SalimgarevaShoesEntities.GetContext().OrderProduct.Add(newOrderProduct);

            try
            {
                SalimgarevaShoesEntities.GetContext().SaveChanges();
                MessageBox.Show("Продукт добавлен в заказ");
                SalimgarevaShoesEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(q => q.Reload());
                UpdateProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void ProductQuantityInOrder_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pqio = "";
            foreach (char c in ProductQuantityInOrder.Text)
            {
                if ("0123456789,".Contains(c))
                    pqio = pqio + c.ToString();
            }
            ProductQuantityInOrder.Text = pqio;


        }

        private void DeleteOrderProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить продукт из заказа?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SalimgarevaShoesEntities.GetContext().OrderProduct.Remove((sender as Button).DataContext as OrderProduct);
                try
                {
                    SalimgarevaShoesEntities.GetContext().SaveChanges();
                    MessageBox.Show("Товар удален из заказа");
                    SalimgarevaShoesEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(q => q.Reload());
                    UpdateProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
