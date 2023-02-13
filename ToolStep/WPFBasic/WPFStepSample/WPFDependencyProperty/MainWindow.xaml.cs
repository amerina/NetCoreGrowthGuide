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

namespace WPFStepSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // tell MainWindow.xaml class where the dataContext is
            this.DataContext = new ViewModel();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            txtText.SetText = $"{DateTime.Now} {nameof(btnTest)} Clicked.";
        }

        #region CustomerControl1
        /// <summary>
        /// https://www.codeproject.com/Articles/140620/WPF-Tutorial-Dependency-Property
        /// </summary>
        public static readonly DependencyProperty IsValuePassedProperty = DependencyProperty.RegisterAttached("IsValuePassed", typeof(bool), typeof(MainWindow),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(IsValuePassed_Changed)));

        public static void SetIsValuePassed(DependencyObject obj, bool value)
        {
            obj.SetValue(IsValuePassedProperty, value);
        }
        public static bool GetIsValuePassed(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsValuePassedProperty);
        }

        public static void IsValuePassed_Changed(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MessageBox.Show("Value passed");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.SetIsValuePassed(this, !(bool)this.GetValue(IsValuePassedProperty));
        }
        #endregion

    }
}
