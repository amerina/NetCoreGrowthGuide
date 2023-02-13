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
    /// https://www.tutorialspoint.com/wpf/wpf_dependency_properties.htm
    /// Follow the steps given below to define custom dependency property in C#.
    /// 1.Declare and register your dependency property with system call register.
    /// 2.Provide the setter and getter for the property.
    /// 3.Define a static handler which will handle any changes that occur globally
    /// 4.Define an instance handler which will handle any changes that occur to that particular instance.
    /// UserControl.xaml 的交互逻辑
    /// </summary>
    public partial class CustomerControl : UserControl
    {
        public CustomerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Declare and register your dependency property with system call register.
        /// </summary>
        public static readonly DependencyProperty SetTextProperty = 
            DependencyProperty.Register("SetText", typeof(string), typeof(CustomerControl), new PropertyMetadata("", new PropertyChangedCallback(OnSetTextChanged)));

        /// <summary>
        /// Provide the setter and getter for the property.
        /// </summary>
        public string SetText
        {
            get { return (string)GetValue(SetTextProperty); }
            set { SetValue(SetTextProperty, value); }
        }

        /// <summary>
        /// handle any changes that occur globally
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSetTextChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            CustomerControl control = d as CustomerControl;
            control.OnSetTextChanged(e);
        }

        /// <summary>
        /// handle any changes that occur to that particular instance.
        /// </summary>
        /// <param name="e"></param>
        private void OnSetTextChanged(DependencyPropertyChangedEventArgs e)
        {
           tbTest.Text = e.NewValue.ToString();
        }
    
    }
}
