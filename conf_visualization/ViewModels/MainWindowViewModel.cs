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
                    string answer = svc.sendUpdateToDataBase(sqlCommand);
                    if (answer != "ok") // если не удалось выполнить запрос к БД
                    {
                        string rus_answer = "";
                        if (answer.Contains("ConferenceId"))
                        {
                            rus_answer = "Такой ID уже существует";
                            ((ConferenceModel)parameter).ConferenceIdColorBrush = Brushes.Red;
                            ((ConferenceModel)parameter).hasError = true;
                            ((ConferenceModel)parameter).ConferenceIdToolTip = rus_answer;
                            MessageBox.Show(rus_answer, "Ошибка");
                        }                        
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
                    string answer = svc.sendUpdateToDataBase(sqlCommand);
                    if (answer != "ok")
                    {
                        MessageBox.Show(answer, "Ошибка");
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


        #region SendEditConferencePlanToDataBaseCommand
        public ICommand SendEditConferencePlanToDataBaseCommand { get; }

        private bool CanSendEditConferencePlanToDataBaseCommandExecute(object parameter) => true;

        private void OnSendEditConferencePlanToDataBaseCommandExecuted(object parameter)
        {
            var svc = new DataAccess();
            ConferencePlanModel ConfPlan = ((ConferencePlanModel)parameter);
            if (ConfPlan.ChangedValue)
            {
                string sqlCommand = "";
                if (ConfPlan.NewValue)
                {
                    sqlCommand = String.Format("INSERT INTO ConferencePlanTable " +
                        "(ConferenceId, ConferenceBeginPeriod, ConferenceEndPeriod, PeriodicType, PeriodicValue,ConferenceStartTime,ConferenceStopTime) " +
                        "VALUES ( {0},'{1}','{2}','{3}','{4}','{5}','{6}' );",
                        CurretConference.ConferenceId, ConfPlan.ConferenceBeginPeriod, ConfPlan.ConferenceEndPeriod, ConfPlan.PeriodicType,
                        ConfPlan.PeriodicValue, ConfPlan.ConferenceStartTime, ConfPlan.ConferenceStopTime);
                    string answer = svc.sendUpdateToDataBase(sqlCommand);
                    if (answer != "ok") // если не удалось выполнить запрос к БД
                    {
                        string rus_answer = "";
                        ConfPlan.hasError = true;
                        ConfPlan.ErrorToolTip = answer;
                        MessageBox.Show(rus_answer, "Ошибка");
                    }
                    else
                    {
                        ConfPlan.NewValue = false;
                        ConfPlan.ChangedValue = false;
                        ConfPlan.hasError = false;
                        LoadConferencesPlan(CurretConference.ConferenceId);
                        //      LoadConferencesPlan(CurretConference.ConferenceId);
                        //при добавлении новой записи в DataGrid по умолчанию уникальный ID = 0, 
                        //он присваивается в базе данных при добавлении новой записи, делаем запрос в БД для получения этого нового значения из БД
                        //     ((ConferencePlanModel)parameter).ID = svc.getConferencetableID(((ConferenceModel)parameter).ConferenceId);
                    }
                }
                else
                {
                  //  sqlCommand = String.Format("UPDATE ConferenceTable SET ConferenceId = {0}, ConferenceName = '{1}', ParticipantsCount = {2}, ConferenceDuration = {3}, IsAcive = '{4}' WHERE id = {5};", ((ConferenceModel)parameter).ConferenceId, ((ConferenceModel)parameter).ConferenceName, ((ConferenceModel)parameter).ParticipantsCount, ((ConferenceModel)parameter).ConferenceDuration, ((ConferenceModel)parameter).IsAcive.ToString(), ((ConferenceModel)parameter).ID);

                    sqlCommand = String.Format("UPDATE ConferencePlanTable SET " +
                       "ConferenceId = {0}, ConferenceBeginPeriod = '{1}', ConferenceEndPeriod = '{2}', PeriodicType = '{3}', " +
                       "PeriodicValue = '{4}', ConferenceStartTime = '{5}', ConferenceStopTime = '{6}' WHERE ConferencePlanId = {7};",
                       CurretConference.ConferenceId, ConfPlan.ConferenceBeginPeriod, ConfPlan.ConferenceEndPeriod, ConfPlan.PeriodicType,
                       ConfPlan.PeriodicValue, ConfPlan.ConferenceStartTime, ConfPlan.ConferenceStopTime, ConfPlan.ConferencePlanId);
                    string answer = svc.sendUpdateToDataBase(sqlCommand);
                    if (answer != "ok")
                    {
                        MessageBox.Show(answer, "Ошибка");
                        //      GetConferencesToDataGrid();
                    }
                    else
                    {
                        ((ConferencePlanModel)parameter).hasError = false;
                        ((ConferencePlanModel)parameter).ChangedValue = false;
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

        //private void OnAddNewConferenceToDataBaseExecuted(object parameter)
        //{
        //    List<string> conferenceItemChangedValueLlist = new List<string>();
        //    List<string> conferenceItemNewValueLlist = new List<string>();
        //    //    string sqlUpdateCommand = "UPDATE ConferenceTable SET ConferenceName = '{0}', ParticipantsCount = {1}, ConferenceDuration = {2} WHERE ConferenceId = {3};";


        //    for (int i = 0; i < Conferences.Count; i++)
        //    {
        //        if (Conferences[i].ChangedValue)
        //        {
        //            conferenceItemChangedValueLlist.Add(String.Format("UPDATE ConferenceTable SET ConferenceName = '{0}', ParticipantsCount = {1}, ConferenceDuration = {2} WHERE ConferenceId = {3};", Conferences[i].ConferenceName, Conferences[i].ParticipantsCount, Conferences[i].ConferenceDuration, Conferences[i].ConferenceId));
        //            Conferences[i].ChangedValue = false;
        //        }
        //        if (Conferences[i].NewValue)
        //        {
        //            conferenceItemNewValueLlist.Add(String.Format("INSERT INTO ConferenceTable (ConferenceId, ConferenceName, ParticipantsCount, ConferenceDuration, IsAcive) VALUES ( {0},'{1}',{2},{3},'{4}' );", Conferences[i].ConferenceId, Conferences[i].ConferenceName, Conferences[i].ParticipantsCount, Conferences[i].ConferenceDuration, Conferences[i].IsAcive.ToString()));
        //            Conferences[i].NewValue = false;
        //        }
        //    }



        //}
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
                string answer = svc.deleteFromDataBase("DELETE FROM ConferenceTable WHERE ConferenceId = " + ((ConferenceModel)parameter).ConferenceId);
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
        #region DeleteConferencePlanFromDataBase
        public ICommand DeleteConferencePlanFromDataBase { get; }

        private bool CanDeleteConferencePlanFromDataBaseExecute(object parameter) => true;

        private void OnDeleteConferencePlanFromDataBaseExecuted(object parameter)
        {
            ConferencePlanModel confPlan = ((ConferencePlanModel)parameter);
            string msgBody = String.Format("{0},{1}\nНачало: {2}\nКонец: {3}", confPlan.PeriodicType, confPlan.PeriodicValue, confPlan.ConferenceStartTime, confPlan.ConferenceStopTime);
            MessageBoxResult result = MessageBox.Show(msgBody, "Удалить план селектора?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var svc = new DataAccess();
                string answer = svc.deleteFromDataBase("DELETE FROM ConferencePlanTable WHERE ConferencePlanId = " + confPlan.ConferencePlanId);
                if (answer != "ok")
                {
                  //  ((ConferenceModel)parameter).ConferenceNameToolTip = answer;
                }
                else
                {
                    ConferencesPlanSeries.Remove(confPlan);
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
            "Еженедельно", "Ежедневно"
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
                    LoadConferencesPlan(value.ConferenceId);
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



        private void LoadConferencesPlan(int confID) // загружает план конференций относящийся к конкретному селектору
        {
            var svc = new DataAccess();
            ConferencesPlanSeries.CollectionChanged -= ConferencesPlanSeries_CollectionChanged;
            ConferencesPlanSeries.Clear();
            if (svc.GetConferencePlanSeries(confID).Count > 0)
            {
                foreach (var conf in svc.GetConferencePlanSeries(confID))
                {
                    this.ConferencesPlanSeries.Add(conf);
                    // this.Conferences_before_edit_dictionary.Add(conf.ConferenceId, conf);
                }
                
            }
            ConferencesPlanSeries.CollectionChanged += ConferencesPlanSeries_CollectionChanged;
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
            //  AddNewConferenceToDataBase = new LambdaCommand(OnAddNewConferenceToDataBaseExecuted, CanAddNewConferenceToDataBaseExecute);
            reloadFromDataBaseCommand = new LambdaCommand(OnReloadFromDataBaseCommandExecuted, CanReloadFromDataBaseCommandExecute);
            SendEditConferenceToDataBaseCommand = new LambdaCommand(OnSendEditConferenceToDataBaseCommandExecuted, CanSendEditConferenceToDataBaseCommandExecute);
            DeleteConferenceFromDataBase = new LambdaCommand(OnDeleteConferenceFromDataBaseExecuted, CanDeleteConferenceFromDataBaseExecute);
            SendEditConferencePlanToDataBaseCommand = new LambdaCommand(OnSendEditConferencePlanToDataBaseCommandExecuted, CanSendEditConferencePlanToDataBaseCommandExecute);
            DeleteConferencePlanFromDataBase = new LambdaCommand(OnDeleteConferencePlanFromDataBaseExecuted, CanDeleteConferencePlanFromDataBaseExecute);
            #endregion
            GetConferencesToDataGrid();
            CurretConference = Conferences.FirstOrDefault();

        }

        private void Conferences_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems[0] is ConferenceModel newConf)
                {
                    newConf.NewValue = true;
                    newConf.ConferenceId = 1000;
                }
            }
            // throw new NotImplementedException();

            //switch (e.Action)
            //{
            //    case NotifyCollectionChangedAction.Remove: // если удаление
            //        if (e.OldItems?[0] is ConferenceModel oldPerson)
            //            MessageBox.Show($"Удален объект: {oldPerson.ConferenceName}");
            //        break;
            //    case NotifyCollectionChangedAction.Add:
            //        if (e.NewItems[0] is ConferenceModel newConf)
            //        {
            //            newConf.NewValue = true;
            //            newConf.ConferenceId = 1000;
            //        }
            //        break;
            //    case NotifyCollectionChangedAction.Replace:
            //        MessageBox.Show($"Replace");
            //        break;
            //}

        }

        private void ConferencesPlanSeries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems[0] is ConferencePlanModel newConf)
                {
                    newConf.NewValue = true;
                 //   newConf.ConferencePlanId = 1000;
                }
            }
        }
    }


}
