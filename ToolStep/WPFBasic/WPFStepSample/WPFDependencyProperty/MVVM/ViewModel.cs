using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFStepSample
{
    /// <summary>
    /// https://www.c-sharpcorner.com/article/what-is-a-dependency-properties-in-wpf-explained-with-an-example/
    ///依赖Prism.WPF实现MVVM模式
    /// </summary>
    public class ViewModel : BindableBase
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set => SetProperty(ref _userName, value);
        }
    }
}
