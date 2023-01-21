using conf_visualization.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
