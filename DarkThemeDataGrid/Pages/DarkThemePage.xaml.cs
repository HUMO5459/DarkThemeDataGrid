using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DarkThemeDataGrid.Models;

namespace DarkThemeDataGrid.Pages;

public partial class DarkThemePage : Page
{
    public ObservableCollection<Order> Orders { get; set; }

    public DarkThemePage()
    {
        InitializeComponent();
        Orders = GenerateRandomOrders(20); // 20 ta random data
        dataGrid.ItemsSource = Orders;
    }
    
    private ObservableCollection<Order> GenerateRandomOrders(int count)
    {
        var random = new Random();
        var customerNames = new[] { "Franklin", "Martin", "George", "Zachary", "James", "Thomas", "Rutherford", "Abraham", "Grover" };
        var countries = new[] { "Argentina", "Brazil", "USA", "Germany", "Japan", "Canada", "UK" };
        var imageFiles = new[]
        {
        "Images/img1.png",
        "Images/img2.png",
        "Images/img3.png",
        "pImages/img4.png",
        "Images/img5.png"
    };

        var orders = new ObservableCollection<Order>();

        for (int i = 0; i < count; i++)
        {
            orders.Add(new Order
            {
                OrderId = 10000 + i,
                CustomerId = 11000 + i,
                CustomerName = customerNames[random.Next(customerNames.Length)],
                ShipCountry = countries[random.Next(countries.Length)],
                ShippingDate = RandomDate(random, new DateTime(2000, 1, 1), DateTime.Now),
                DeliveryStatus = random.Next(2) == 0,
                ImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imageFiles[random.Next(imageFiles.Length)])  // 🖼 Rasm yo‘li
            });
        }

        return orders;
    }


    private DateTime RandomDate(Random random, DateTime start, DateTime end)
    {
        var range = (end - start).Days;
        return start.AddDays(random.Next(range));
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var order = button?.DataContext as Order;
        if (order != null)
        {
            MessageBox.Show($"Editing Order ID: {order.OrderId}\nCustomer: {order.CustomerName}", "Edit Order");
        }
    }

    private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        var image = sender as Image;
        if (image?.DataContext is Order order && !string.IsNullOrEmpty(order.ImagePath))
        {
            var viewer = new ImageViewerWindow(order.ImagePath);
            viewer.ShowDialog(); // Modal
        }
    }
    
    
}