using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace WPFStepSample.MarkupExtension
{
    /// <summary>
    /// 使用:
    /// <ListBox ItemsSource="{local:Reflection {x:Type Grid}, IncludeMethods=true, IncludeFields=true, IncludeEvents=true}"  MaxHeight="200" Grid.Row="3" Grid.ColumnSpan="2" />
    /// </summary>
    public class ReflectionExtension : System.Windows.Markup.MarkupExtension
    {
        public Type CurrentType { get; set; }
        public bool IncludeMethods { get; set; }
        public bool IncludeFields { get; set; }
        public bool IncludeEvents { get; set; }

        public ReflectionExtension(Type currentType)
        {
            this.CurrentType = currentType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.CurrentType == null)
                throw new ArgumentException("Type argument is not specified");

            ObservableCollection<string> collection = new ObservableCollection<string>();
            foreach (PropertyInfo p in this.CurrentType.GetProperties())
                collection.Add(string.Format("Property : {0}", p.Name));

            if (this.IncludeMethods)
                foreach (MethodInfo m in this.CurrentType.GetMethods())
                    collection.Add(string.Format("Method : {0} with {1} argument(s)", m.Name, m.GetParameters().Count()));
            if (this.IncludeFields)
                foreach (FieldInfo f in this.CurrentType.GetFields())
                    collection.Add(string.Format("Field : {0}", f.Name));
            if (this.IncludeEvents)
                foreach (EventInfo e in this.CurrentType.GetEvents())
                    collection.Add(string.Format("Events : {0}", e.Name));

            return collection;
        }

    }
}
