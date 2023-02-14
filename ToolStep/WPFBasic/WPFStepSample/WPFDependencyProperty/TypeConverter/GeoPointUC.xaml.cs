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

namespace WPFStepSample.TypeConverter
{
    /// <summary>
    /// GeoPointUC.xaml 的交互逻辑
    /// 
    /// <converter:GeoPoint x:Name="cGeoPoint" GeoPointValue="60.5,20.5" />
    /// </summary>
    public partial class GeoPointUC : UserControl
    {
        public GeoPointItem GeoPointValue
        {
            get { return GetValue(GeoPointValueProperty) as GeoPointItem; }
            set { SetValue(GeoPointValueProperty, value); }
        }

        public static readonly DependencyProperty GeoPointValueProperty =
            DependencyProperty.Register("GeoPointValue", typeof(GeoPointItem), typeof(GeoPointUC), new PropertyMetadata(new GeoPointItem(0.0,0.0)));


        public GeoPointUC()
        {
            InitializeComponent();
        }

        private void txtlat_TextChanged(object sender, TextChangedEventArgs e)
        {
            GeoPointItem item = this.GeoPointValue;

            item.Latitude = Convert.ToDouble(txtlat.Text);
            this.GeoPointValue = item;
        }

        private void txtlon_TextChanged(object sender, TextChangedEventArgs e)
        {
            GeoPointItem item = this.GeoPointValue;

            item.Longitude = Convert.ToDouble(txtlon.Text);
            this.GeoPointValue = item;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GeoPointItem item = this.GeoPointValue;
            this.txtlat.Text = item.Latitude.ToString();
            this.txtlon.Text = item.Longitude.ToString();

        }
    }
}
