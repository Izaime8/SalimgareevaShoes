using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        Products currentProduct = new Products();
        public AddEditWindow(Products selectedProduct)
        {
            InitializeComponent();


            CategoryCB.ItemsSource = SalimgarevaShoesEntities.GetContext().Categories.ToList();
            CategoryCB.SelectedIndex = 0;
            ManufacturersCB.ItemsSource = SalimgarevaShoesEntities.GetContext().Manufacturers.ToList();
            ManufacturersCB.SelectedIndex = 0;
            SupplersCB.ItemsSource = SalimgarevaShoesEntities.GetContext().Supplers.ToList();
            SupplersCB.SelectedIndex = 0;


            if (selectedProduct != null)
            {
                currentProduct = selectedProduct;
                CategoryCB.SelectedItem = currentProduct.Categories;
                ManufacturersCB.SelectedItem = currentProduct.Manufacturers;
                SupplersCB.SelectedItem = currentProduct.Supplers;
            }
            DataContext = currentProduct;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentProduct.ProductArticle))
                errors.AppendLine("Введите артикул");
            
            if (currentProduct.ProductArticle.Length > 6)
                errors.AppendLine("Артикул должен состоять из 6 или менее символов");

            if (string.IsNullOrWhiteSpace(currentProduct.ProductName))
                errors.AppendLine("Введите название");
            if (string.IsNullOrWhiteSpace(currentProduct.ProductDescription))
                errors.AppendLine("Введите описание");

            if (currentProduct.ProductCost <= 0)
                errors.AppendLine("Цена не может быть меньше или равна нулю");
            if (string.IsNullOrWhiteSpace(currentProduct.ProductUnitMeasure))
                errors.AppendLine("Введите единицу измерения");
            if (currentProduct.ProductQuantityInStock < 0)
                errors.AppendLine("Количество на складе не может быть отрицательной");
            if (currentProduct.ProductCurrentDiscount < 0)
                errors.AppendLine("Скидка не может быть отрицательной");



            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (currentProduct.ProductID == 0)
            {
                SalimgarevaShoesEntities.GetContext().Products.Add(currentProduct);
            }

            try
            {
                SalimgarevaShoesEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                currentProduct.ProductImage = openFileDialog.FileName.ToString();
                ProductImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

            /*
            private void ChangeImage_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Изображения (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

                    if (openFileDialog.ShowDialog() == true)
                    {
                        // Папка проекта (на уровень выше bin\Debug)
                        string projectDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));
                        string importFolder = System.IO.Path.Combine(projectDirectory, "import");

                        if (!System.IO.Directory.Exists(importFolder))
                            System.IO.Directory.CreateDirectory(importFolder);

                        // Удаляем старое изображение, если есть
                        if (!string.IsNullOrEmpty(currentProduct.ProductImage))
                        {
                            string oldImagePath = System.IO.Path.Combine(projectDirectory, currentProduct.ProductImage);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                try
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Не удалось удалить старое изображение: {ex.Message}");
                                }
                            }
                        }

                        // Загружаем выбранное изображение
                        BitmapImage originalImage = new BitmapImage();
                        originalImage.BeginInit();
                        originalImage.UriSource = new Uri(openFileDialog.FileName);
                        originalImage.CacheOption = BitmapCacheOption.OnLoad;
                        originalImage.EndInit();

                        // Масштабируем до 300x200 пикселей
                        TransformedBitmap resizedImage = new TransformedBitmap(originalImage,
                            new ScaleTransform(300.0 / originalImage.PixelWidth, 200.0 / originalImage.PixelHeight));

                        // Уникальное имя файла
                        string newFileName = Guid.NewGuid().ToString() + ".png";
                        string destinationPath = System.IO.Path.Combine(importFolder, newFileName);

                        // Сохраняем масштабированное изображение в проект
                        using (var fileStream = new System.IO.FileStream(destinationPath, System.IO.FileMode.Create))
                        {
                            PngBitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(resizedImage));
                            encoder.Save(fileStream);
                        }

                        // Сохраняем путь относительно проекта (для БД)
                        currentProduct.ProductImage = System.IO.Path.Combine("import/", newFileName);

                        // Загружаем изображение для отображения
                        BitmapImage previewImage = new BitmapImage();
                        previewImage.BeginInit();
                        previewImage.UriSource = new Uri(destinationPath, UriKind.Absolute);
                        previewImage.CacheOption = BitmapCacheOption.OnLoad;
                        previewImage.EndInit();

                        ProductImage.Source = previewImage;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при изменении изображения: " + ex.Message);
                }
            }


            private void ChangeImage_Click(object sender, RoutedEventArgs e)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.gif;*.bmp)|*.png;*.jpeg;*.jpg;*.gif;*.bmp|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == true)
                {
                    string projectDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..")); 
                    string importFolder = System.IO.Path.Combine(projectDirectory, "import");

                    if (!System.IO.Directory.Exists(importFolder))
                        System.IO.Directory.CreateDirectory(importFolder);

                    string originalFileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    string destinationPath = System.IO.Path.Combine(importFolder, originalFileName);

                    System.IO.File.Copy(openFileDialog.FileName, destinationPath, overwrite: true);

                    currentProduct.ProductImage = System.IO.Path.Combine("import", originalFileName);

                    ProductImage.Source = new BitmapImage(new Uri(destinationPath));
                }
            }
            */
        }
}
