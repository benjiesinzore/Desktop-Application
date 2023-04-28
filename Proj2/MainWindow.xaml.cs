using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace Proj2
{
    public partial class MainWindow : Window
    {

        private decimal cpuPrice = 0;

        private decimal totalPrice = 0;

        private DispatcherTimer timer;

        string getName = "";

        bool isItChecked = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        // This method is called when any of the radio buttons in the computerType stack panel are checked
        private void ComputerType_Checked(object sender, RoutedEventArgs e)
        {

            RadioButton selected = sender as RadioButton;

            // Uncheck all the checkboxes
            UncheckAllCheckBoxes();
            decimal value = decimal.Parse(selected?.Tag.ToString() ?? "0");

            totalPrice = value;
            isItChecked = true;

            UpdateTotalPrice();

            // Display the image corresponding to the selected radio button
            switch (value)
            {
                case 2000:
                    
                    UpdateTotalPrice();

                    computerImage.Source = new BitmapImage(new Uri(@"D:\Proj2\Proj2\Proj2\images\gaming.jpg", UriKind.Absolute));

                    break;
                case 1500:

                    computerImage.Source = new BitmapImage(new Uri(@"D:\Proj2\Proj2\Proj2\images\workstation.jpg", UriKind.Absolute));
                    break;
                case 1000:

                    computerImage.Source = new BitmapImage(new Uri(@"D:\Proj2\Proj2\Proj2\images\desktop.jpg", UriKind.Absolute));
                    break;
                case 800:

                    computerImage.Source = new BitmapImage(new Uri(@"D:\Proj2\Proj2\Proj2\images\smallform.jpg", UriKind.Absolute));
                    break;
                default:
                    computerImage.Source = null;
                    break;
            }



        }

        // This method is called when the selection in the cpuCombo ComboBox is changed
        private void CpuCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selected = cpuCombo.SelectedItem as ComboBoxItem;
            try
            {
                if (selected != null)
                {
                    int price = int.Parse(selected.Tag?.ToString() ?? "0");
                    totalPrice -= cpuPrice;
                    totalPrice += price;
                    cpuPrice = price;
                    UpdateTotalPrice();
                }
            } catch (Exception)
            {
                
            }
        }


        // This method is called when any of the optionalItem checkboxes are checked
        private void OptionalItem_Checked(object sender, RoutedEventArgs e)
        {
            
            CheckBox selected = sender as CheckBox;
            decimal value = decimal.Parse(selected?.Tag.ToString() ?? "0");
            totalPrice += value;

            UpdateTotalPrice();
        }

        // This method is called when any of the optionalItem checkboxes are unchecked
        private void OptionalItem_Unchecked(object sender, RoutedEventArgs e)
        {
            
            CheckBox selected = sender as CheckBox;
            decimal value = decimal.Parse(selected?.Tag.ToString() ?? "0");
            totalPrice -= value;

            UpdateTotalPrice();
        }

        // This method updates the totalPriceLabel to display the current total price
        private void UpdateTotalPrice()
        {
            if (totalPriceLabel != null)
            {
                totalPriceLabel.Content = $"Total Price: ${totalPrice}";
            }
            
        }

        // This method unchecks all the checkboxes
        private void UncheckAllCheckBoxes()
        {
            
            optionalItem1.IsChecked = false;
            optionalItem2.IsChecked = false;
            optionalItem3.IsChecked = false;
        }

        // This method is called when the NameTextBox loses focus
        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                nameErrorTextBlock.Visibility = Visibility.Visible;
                nameTextBox.Background = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                nameErrorTextBlock.Visibility = Visibility.Collapsed;
                nameTextBox.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void MakePurchase_Click(object sender, RoutedEventArgs e)
        {
            if (!isItChecked)
            {
                MessageBox.Show("Please select a base computer type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } 

            else if (string.IsNullOrEmpty(nameTextBox.Text))
            {
                nameErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                nameErrorTextBlock.Visibility = Visibility.Collapsed;


                getName = nameTextBox.Text;
                ShowTextBlockForOneMinute(nameTextBox.Text);

                myButtonConfirm.Visibility = Visibility.Visible;
                myButton.Visibility = Visibility.Collapsed;
            }
        }



        // This method is will display TextBlock for one minute
        private void ShowTextBlockForOneMinute(string text)
        {

            myTextBlock.Text = "Hello, " + text + ". You are about to make a purchase worth: " + $"Total Price: ${totalPrice}";
            myTextBlock.Visibility = Visibility.Visible;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(120);
            timer.Tick += (s, e) =>
            {
                
                myTextBlock.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }


        private void Confirm_Purchase(object sender, RoutedEventArgs e)
        {

            ShowConfirmDialog();

        }



        public void ShowConfirmDialog()
        {
            MessageBoxResult result = MessageBox.Show("Thank you for your order " + getName + "!" + "\n Total Price: $" + totalPrice, "Purchase Summery", MessageBoxButton.OK, MessageBoxImage.Information);
            // Check if the "OK" button was clicked
            if (result == MessageBoxResult.OK)
            {
                // Call your method here
                ResetPage();
            }
        }


        private void ResetPage()
        {

            isItChecked = false;
            // Reset radio buttons
            var computerType = FindName("computerType") as StackPanel;
            if (computerType != null)
            {
                foreach (RadioButton radioButton in computerType.Children)
                {
                    radioButton.IsChecked = false;
                }
            }

            // Reset image
            computerImage.Source = null;

            // Reset combo box
            cpuCombo.SelectedIndex = 0;

            // Reset check boxes
            optionalItem1.IsChecked = false;
            optionalItem2.IsChecked = false;
            optionalItem3.IsChecked = false;

            // Reset total price label
            totalPriceLabel.Content = "Total Price: $0";

            // Reset name text box and error message
            nameTextBox.Text = "";
            nameErrorTextBlock.Visibility = Visibility.Collapsed;

            // Hide confirmation message and button
            myTextBlock.Visibility = Visibility.Collapsed;
            myButtonConfirm.Visibility = Visibility.Collapsed;

            // Show purchase button
            myButton.Visibility = Visibility.Visible;
        }


    }



}
