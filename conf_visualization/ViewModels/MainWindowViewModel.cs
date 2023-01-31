using conf_visualization.Data;
using conf_visualization.Infrastructure.Commands;
using conf_visualization.Models;
using conf_visualization.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace conf_visualization.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        //    public BindableCollection<ConferenceModel> People { get; set; }
        public ObservableCollection<ConferenceModel> Conferences { get; set; } = new ObservableCollection<ConferenceModel>();
        public Collection<ConferenceModel> Conferences_before_edit = new Collection<ConferenceModel>();
        public Dictionary<int, ConferenceModel> Conferences_before_edit_dictionary = new Dictionary<int, ConferenceModel>();
        public Dictionary<int, ConferenceModel> Conferences_after_edit_dictionary = new Dictionary<int, ConferenceModel>();



        #region Заголовок окна
        private string _TitleAddEditConferenceWindow = "asdasd";

        /// <summary>Заголовок окна</summary>

        public string TitleAddEditConferenceWindow
        {
            get => _TitleAddEditConferenceWindow; // { return _Title; }  
            set => Set(ref _TitleAddEditConferenceWindow, value);
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
            List<string> conferenceItemChangedValueLlist = new List<string>();
            List<string> conferenceItemNewValueLlist = new List<string>();
            //    string sqlUpdateCommand = "UPDATE ConferenceTable SET ConferenceName = '{0}', ParticipantsCount = {1}, ConferenceDuration = {2} WHERE ConferenceId = {3};";


            for (int i = 0; i < Conferences.Count; i++)
            {
                if (Conferences[i].ChangedValue)
                {
                    conferenceItemChangedValueLlist.Add(String.Format("UPDATE ConferenceTable SET ConferenceName = '{0}', ParticipantsCount = {1}, ConferenceDuration = {2} WHERE ConferenceId = {3};", Conferences[i].ConferenceName, Conferences[i].ParticipantsCount, Conferences[i].ConferenceDuration, Conferences[i].ConferenceId));
                }
                if (Conferences[i].NewValue)
                {
                    conferenceItemNewValueLlist.Add(String.Format("INSERT INTO ConferenceTable (ConferenceId, ConferenceName, ParticipantsCount, ConferenceDuration, IsAcive) VALUES ( {0},'{1}',{2},{3},'{4}' );", Conferences[i].ConferenceId, Conferences[i].ConferenceName, Conferences[i].ParticipantsCount, Conferences[i].ConferenceDuration, Conferences[i].IsAcive.ToString()));
                    Conferences[i].NewValue = false;
                }
            }


            MessageBox.Show("");

        }
        #endregion




        #endregion


        //private DataGridColumn _getNameColumn;

        //public DataGridColumn GetNameColumn
        //{
        //    get { return _getNameColumn; }
        //    set
        //    {
        //        if (_getNameColumn != value)
        //            _getNameColumn = value;
        //        if (_getNameColumn != null)
        //        {
        //           // Debug.Print(_getNameColumn.DisplayIndex.ToString());
        //            MessageBox.Show(_getNameColumn.DisplayIndex.ToString());
        //        }
        //        RaisePropertyChanged("GetNameColumn");
        //    }
        //}

        private ConferenceModel _ConferenceModel;

        public ConferenceModel CurretConference
        {
            get => _ConferenceModel;
            set => Set(ref _ConferenceModel, value);
        }


        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            AddNewConferenceToDataBase = new LambdaCommand(OnAddNewConferenceToDataBaseExecuted, CanAddNewConferenceToDataBaseExecute);
            #endregion

            

            var svc = new DataAccess();
            foreach (var conf in svc.GetConferences())
            {
                conf.ChangedValue = false;
                this.Conferences.Add(conf);
                this.Conferences_before_edit_dictionary.Add(conf.ConferenceId, conf);
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
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems[0] is ConferenceModel newConf)
                    {
                        newConf.NewValue = true;
                        newConf.ConferenceId = 1000;
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    MessageBox.Show($"Replace");
                    break;
            }
        }
    }

    
}
