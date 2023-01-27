using conf_visualization.Data;
using conf_visualization.Infrastructure.Commands;
using conf_visualization.Models;
using conf_visualization.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace conf_visualization.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
    //    public BindableCollection<ConferenceModel> People { get; set; }
    public ObservableCollection<ConferenceModel> Conferences { get; set; } = new ObservableCollection<ConferenceModel>();
        public Collection<ConferenceModel> Conferences_before_edit = new Collection<ConferenceModel>();



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
        #region AddNewConferenceToDataBase
        public ICommand AddNewConferenceToDataBase { get; }

        private bool CanAddNewConferenceToDataBaseExecute(object parameter) => true;

        private void OnAddNewConferenceToDataBaseExecuted(object parameter)
        {
            MessageBox.Show("проверка команды");
        }
        #endregion



        #endregion


        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            AddNewConferenceToDataBase = new LambdaCommand(OnAddNewConferenceToDataBaseExecuted, CanAddNewConferenceToDataBaseExecute);
            #endregion


            var svc = new DataAccess();
            foreach (var conf in svc.GetConferences())
            {
                this.Conferences.Add(conf);
                this.Conferences_before_edit.Add(conf);
            }

            Conferences.CollectionChanged += Conferences_CollectionChanged;
        }

        private void Conferences_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           // throw new NotImplementedException();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove: // если удаление
                    if (e.OldItems?[0] is ConferenceModel oldPerson)
                        MessageBox.Show($"Удален объект: {oldPerson.ConferenceName}");
                    break;
            }
        }
    }

    
}
