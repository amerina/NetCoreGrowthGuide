using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFSample
{
    /// <summary>
    /// 大部分过程代码都在模型中
    /// UI只是声明性的XAML，它通过绑定与模型同步。这就是MVVM的本质。
    /// </summary>
    public class Model : INotifyPropertyChanged
    {
        public ICommand AddCommand { get; private set; }
        public Model()
        {
            AddCommand = new AddNameCommand(this);
        }
        public string CurrentName
        {
            get { return mCurrentName; }
            set
            {
                if (value == mCurrentName)
                    return;
                mCurrentName = value;
                OnPropertyChanged();
            }
        }
        string mCurrentName;

        public ObservableCollection<string> AddedNames
        { get; } = new ObservableCollection<string>();


        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        class AddNameCommand : ICommand
        {
            Model parent;

            public AddNameCommand(Model parent)
            {
                this.parent = parent;
                parent.PropertyChanged += delegate { CanExecuteChanged?.Invoke(this, EventArgs.Empty); };
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) { return !string.IsNullOrEmpty(parent.CurrentName); }

            public void Execute(object parameter)
            {
                parent.AddedNames.Add(parent.CurrentName); ;
                parent.CurrentName = null;
            }
        }
    }
}
