using conf_visualization.Infrastructure.Commands;
using conf_visualization.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace conf_visualization.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Заголовок окна
        private string _Title;

        /// <summary>Заголовок окна</summary>

        public string Title
        {
            get => _Title; // { return _Title; }  
            set => Set(ref _Title, value);
            //set 
            //{ 
            //   // _Title = value;
            //    Set(ref _Title, value);
            //}
        }
        #endregion

        #region Команды

        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object parameter) => true;

        private void OnCloseApplicationCommandExecuted(object parameter)
        {
            Application.Current.Shutdown();
        } 
        #endregion


        #endregion


        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            #endregion

        }

    }


    

    internal class testClass : ViewModel
    {
        private string _a;
        private string _b;
        private string _c;
        //private var data = new List<testClass>
        //                   {
        //                       new testClass("1", "2", "3"),
        //                       new testClass("4", "5", "6"),
        //                       new testClass("7", "8", "9")
        //                   };

        public testClass(string a, string b, string c)
        {
            A = a;
            B = b;
            C = c;
        }

        public string A 
        {
            get => _a; // { return _Title; }  
            set => Set(ref _a, value);
        }

        public string B
        {
            get => _b; // { return _Title; }  
            set => Set(ref _b, value);
        }

        public string C
        {
            get => _c; // { return _Title; }  
            set => Set(ref _c, value);
        }
    }

   

}
