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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace conf_visualization.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        //    public BindableCollection<ConferenceModel> People { get; set; }
        public ObservableCollection<ConferenceModel> Conferences { get; set; } = new ObservableCollection<ConferenceModel>();
        public ObservableCollection<ConferencePlanModel> ConferencesPlanSeries { get; set; } = new ObservableCollection<ConferencePlanModel>();

        public Collection<ConferenceModel> Conferences_before_edit = new Collection<ConferenceModel>();
        public Dictionary<int, ConferenceModel> Conferences_before_edit_dictionary = new Dictionary<int, ConferenceModel>();
        public Dictionary<int, ConferenceModel> Conferences_after_edit_dictionary = new Dictionary<int, ConferenceModel>();



        #region Команды

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


        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object parameter) => true;

        private void OnCloseApplicationCommandExecuted(object parameter)
        {
            Application.Current.Shutdown();
        }
        #endregion


        #region reloadFromDataBaseCommand
        public ICommand reloadFromDataBaseCommand { get; }

        private bool CanReloadFromDataBaseCommandExecute(object parameter) => true;

        private void OnReloadFromDataBaseCommandExecuted(object parameter)
        {
            Conferences.Clear();
            Conferences.CollectionChanged -= Conferences_CollectionChanged;
            Conferences_before_edit_dictionary.Clear();
            var svc = new DataAccess();
            foreach (var conf in svc.GetConferences())
            {
                conf.ChangedValue = false;
                this.Conferences.Add(conf);
                this.Conferences_before_edit_dictionary.Add(conf.ConferenceId, conf);
            }
            Conferences.CollectionChanged += Conferences_CollectionChanged;
        }
        #endregion


        #region SendEditConferenceToDataBaseCommand
        public ICommand SendEditConferenceToDataBaseCommand { get; }

        private bool CanSendEditConferenceToDataBaseCommandExecute(object parameter) => true;

        private void OnSendEditConferenceToDataBaseCommandExecuted(object parameter)
        {

            var svc = new DataAccess();
            if (((ConferenceModel)parameter).ChangedValue)
            {
                string sqlCommand = "";
                if (((ConferenceModel)parameter).NewValue)
                {
                    sqlCommand = String.Format("INSERT INTO ConferenceTable (ConferenceId, ConferenceName, ParticipantsCount, ConferenceDuration, IsAcive) VALUES ( {0},'{1}',{2},{3},'{4}' );", ((ConferenceModel)parameter).ConferenceId, ((ConferenceModel)parameter).ConferenceName, ((ConferenceModel)parameter).ParticipantsCount, ((ConferenceModel)parameter).ConferenceDuration, ((ConferenceModel)parameter).IsAcive.ToString());
                    if (!svc.sendUpdateToDataBase(sqlCommand)) // если не удалось выполнить запрос к БД
                    {
                        ((ConferenceModel)parameter).ConferenceIdColorBrush = Brushes.Red;
                        ((ConferenceModel)parameter).ConferenceName = "не удалось выполнить запрос к БД";
                        //     MessageBox.Show(Conferences.Last().ConferenceId+" "+ Conferences.Last().ConferenceName);
                        //     Conferences.Remove(Conferences[10]);
                        //   GetConferencesToDataGrid();
                    }
                    else
                    {
                        ((ConferenceModel)parameter).NewValue = false;
                        ((ConferenceModel)parameter).hasError = false;
                        //при добавлении новой записи в DataGrid по умолчанию уникальный ID = 0, 
                        //он присваивается в базе данных при добавлении новой записи, делаем запрос в БД для получения этого нового значения из БД
                        ((ConferenceModel)parameter).ID = svc.getConferencetableID(((ConferenceModel)parameter).ConferenceId);
                    }
                }
                else
                {
                    sqlCommand = String.Format("UPDATE ConferenceTable SET ConferenceId = {0}, ConferenceName = '{1}', ParticipantsCount = {2}, ConferenceDuration = {3}, IsAcive = '{4}' WHERE id = {5};", ((ConferenceModel)parameter).ConferenceId, ((ConferenceModel)parameter).ConferenceName, ((ConferenceModel)parameter).ParticipantsCount, ((ConferenceModel)parameter).ConferenceDuration, ((ConferenceModel)parameter).IsAcive.ToString(), ((ConferenceModel)parameter).ID);
                    if (!svc.sendUpdateToDataBase(sqlCommand))
                    {
                        GetConferencesToDataGrid();
                    }
                    else
                    {
                        ((ConferenceModel)parameter).hasError = false;
                        ((ConferenceModel)parameter).ChangedValue = false;
                    }
                }
                //если отправка в БД завершилась с ошибкой то обновлеям DataGrid данными из БД
                // т.к. изменения в DataGrid останутся

            }
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


            if (conferenceItemChangedValueLlist.Count > 0)
            {
                var svc = new DataAccess();
                //   svc.sendUpdateToDataBase(conferenceItemChangedValueLlist);
            }


            //   MessageBox.Show("");

        }
        #endregion

        #region DeleteConferenceFromDataBase
        public ICommand DeleteConferenceFromDataBase { get; }

        private bool CanDeleteConferenceFromDataBaseExecute(object parameter) => true;

        private void OnDeleteConferenceFromDataBaseExecuted(object parameter)
        {
            string msgBody = String.Format("[{0}] {1}\nУчастников: {2}\nПродолжительность: {3}", ((ConferenceModel)parameter).ConferenceId, ((ConferenceModel)parameter).ConferenceName, ((ConferenceModel)parameter).ParticipantsCount, ((ConferenceModel)parameter).ConferenceDuration);
            MessageBoxResult result = MessageBox.Show(msgBody, "Удалить селектор?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var svc = new DataAccess();
               string answer = svc.deleteFromDataBase("DELETE FROM ConferenceTable WHERE id = "+ ((ConferenceModel)parameter).ID);
                if (answer != "ok")
                {
                    ((ConferenceModel)parameter).ConferenceNameColorBrush = Brushes.Red;
                    ((ConferenceModel)parameter).ConferenceNameToolTip = answer;
                }
                else
                {
                    Conferences.Remove(((ConferenceModel)parameter));
                }
                //  DELETE FROM table_name WHERE condition;
            }
        }
        #endregion

        #endregion






        public IList<string> PeriodicTypesList
        {
            get { return arrElementsCombo; }
        }
        private readonly string[] arrElementsCombo =
        {
            "Еженедельно", "Ежемесячно", "Ежедневно"
        };


        private ConferenceModel _ConferenceModel;
        public ConferenceModel CurretConference
        {
            get { return _ConferenceModel; }

            set
            {
                if (value != null)
                {
                    Set(ref _ConferenceModel, value);
                    dosmth(value.ConferenceId);
                }
            }
        }

        private ConferenceModel _editedConferenceModel;
        public ConferenceModel EditedConferenceModel
        {
            get { return _editedConferenceModel; }
            set
            {
                if (value != null)
                {
                    Set(ref _editedConferenceModel, value);
                }
            }
        }



        private void dosmth(int confID)
        {
            var svc = new DataAccess();
            if (svc.GetConferencePlanSeries(confID).Count == 0)
            {
                this.ConferencesPlanSeries.Clear();
            }
            foreach (var conf in svc.GetConferencePlanSeries(confID))
            {
                //   conf.ChangedValue = false;
                this.ConferencesPlanSeries.Clear();
                this.ConferencesPlanSeries.Add(conf);
                // this.Conferences_before_edit_dictionary.Add(conf.ConferenceId, conf);
            }
        }


        public void GetConferencesToDataGrid()
        {
            Conferences.CollectionChanged -= Conferences_CollectionChanged;
            Conferences.Clear();
            this.Conferences_before_edit_dictionary.Clear();
            var svc = new DataAccess();
            foreach (var conf in svc.GetConferences())
            {
                conf.ChangedValue = false;
                this.Conferences.Add(conf);
                //        this.Conferences_before_edit_dictionary.Add(conf.ConferenceId, conf);
            }
            Conferences.CollectionChanged += Conferences_CollectionChanged;
        }

        public MainWindowViewModel()
        {
            #region Команды
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            AddNewConferenceToDataBase = new LambdaCommand(OnAddNewConferenceToDataBaseExecuted, CanAddNewConferenceToDataBaseExecute);
            reloadFromDataBaseCommand = new LambdaCommand(OnReloadFromDataBaseCommandExecuted, CanReloadFromDataBaseCommandExecute);
            SendEditConferenceToDataBaseCommand = new LambdaCommand(OnSendEditConferenceToDataBaseCommandExecuted, CanSendEditConferenceToDataBaseCommandExecute);
            DeleteConferenceFromDataBase = new LambdaCommand(OnDeleteConferenceFromDataBaseExecuted, CanDeleteConferenceFromDataBaseExecute);
            #endregion
            GetConferencesToDataGrid();
            CurretConference = Conferences.FirstOrDefault();





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
