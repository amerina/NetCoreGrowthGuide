using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// https://www.codeproject.com/Articles/140620/WPF-Tutorial-Dependency-Property
    /// https://learn.microsoft.com/zh-cn/dotnet/desktop/wpf/advanced/properties-wpf?view=netframeworkdesktop-4.8
    /// CustomerControl1.xaml 的交互逻辑
    /// </summary>
    public partial class CustomerControl1 : UserControl
    {
        public CustomerControl1()
        {
            InitializeComponent();

            //for each instance, the collection will reset and hence you will see a unique collection for each UserControl created.
            SetValue(ObserverPropertyKey, new ObservableCollection<Button>());
        }

        public static FrameworkPropertyMetadata propertyMetadata = new FrameworkPropertyMetadata("Comes as Default",
                                                                                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                                                new PropertyChangedCallback(DP_PropertyChanged),
                                                                                                new CoerceValueCallback(DP_CoerceValue),
                                                                                                false,
                                                                                                UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty DPProperty = DependencyProperty.Register("DP", typeof(string), typeof(CustomerControl1), propertyMetadata, new ValidateValueCallback(DP_Validate));
        private static void DP_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.

            //Observer.Add(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object DP_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency property value is reevaluated. The return value is the 
            //latest value set to the dependency property
            //Observer.Add(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool DP_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //Observer.Add(string.Format("DataValidation is Fired : Value {0}", Value));


            return true;
        }

        public string DP
        {
            get
            {
                return this.GetValue(DPProperty) as string;
            }
            set
            {
                this.SetValue(DPProperty, value);
            }
        }

        /// <summary>
        /// Note on CollectionType Dependency Property
        /// 当希望将DependencyObject的集合保存到一个集合中时，将使用CollectionType依赖项属性。
        /// 如果您希望创建一个Dependency对象的集合，并希望对象具有自己的默认值，
        /// 则需要将该值分配给依赖项集合的每个单独项，而不是使用元数据定义定义。
        /// </summary>
        public static readonly DependencyPropertyKey ObserverPropertyKey = DependencyProperty.RegisterReadOnly("Observer", typeof(ObservableCollection<Button>), 
                                                                            typeof(CustomerControl1),
                                                                            new FrameworkPropertyMetadata(new ObservableCollection<Button>()));
        public static readonly DependencyProperty ObserverProperty = ObserverPropertyKey.DependencyProperty;

        public ObservableCollection<Button> Observer
        {
            get
            {
                return (ObservableCollection<Button>)GetValue(ObserverProperty);
            }
        }
    }
}
