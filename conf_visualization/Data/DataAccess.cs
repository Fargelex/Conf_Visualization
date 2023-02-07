using conf_visualization.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace conf_visualization.Data
{
    internal class DataAccess
    {
      //  Random rnd = new Random();
        string DataBaseName = "database.db";

        int[] conferenceId = new int[] { 540, 424, 422, 80, 57, 25, 350, 150, 20, 10 };
        string[] conferenceName = new string[] { "Селектор 1", "Селектор 2", "Селектор 3", "Селектор 4", "Селектор 5", "Селектор 6", "Селектор 7", "Селектор 8", "Селектор 9", "Селектор 10" };
        int[] participantsCount = new int[] { 30,30,44,80,57,25,35,15,20,10 };
        int[] conferenceDuration = new int[] { 360, 360, 180, 60, 240, 90, 100, 35, 20, 30 };

        bool[] isAcive = new bool[] { true, false };

   //     DateTime lowEndDate = new DateTime(1943, 1, 1);
     //   int daysFromLowDate;

        public DataAccess()
        {
        //    daysFromLowDate = (DateTime.Today - lowEndDate).Days;
        }

        //Подключение к Базе Данных
        public List<ConferenceModel> GetConferences()
        {
            List<ConferenceModel> ConferenceList = new List<ConferenceModel>();

            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=database.db; Version=3;"))
            {
                Connect.Open();

                SQLiteCommand comm = new SQLiteCommand("Select * From ConferenceTable", Connect);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        ConferenceModel outputConference = new ConferenceModel();

                        outputConference.ConferenceId = Convert.ToInt32(read.GetValue(read.GetOrdinal("ConferenceId")));
                        outputConference.ConferenceName = read.GetValue(read.GetOrdinal("ConferenceName")).ToString();
                        outputConference.ConferenceDuration = Convert.ToInt32(read.GetValue(read.GetOrdinal("ConferenceDuration")));
                        outputConference.ParticipantsCount = Convert.ToInt32(read.GetValue(read.GetOrdinal("ParticipantsCount")));
                        outputConference.IsAcive = Convert.ToBoolean(read.GetValue(read.GetOrdinal("IsAcive")));
                        ConferenceList.Add(outputConference);
                    }
                    Connect.Close(); // закрыть соединение
                }
                return ConferenceList;
            }
        }


        public List<ConferencePlanModel> GetConferencePlanSeries(int ConferenceId)
        {
            List<ConferencePlanModel> ConferencePlanSeriesList = new List<ConferencePlanModel>();

            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=database.db; Version=3;"))
            {
                Connect.Open();

                SQLiteCommand comm = new SQLiteCommand("Select * From ConferencePlanTable WHERE ConferenceId="+ ConferenceId.ToString(), Connect);
                using (SQLiteDataReader read = comm.ExecuteReader())
                {
                    while (read.Read())
                    {
                        ConferencePlanModel outputConferencePlan = new ConferencePlanModel();

                        outputConferencePlan.ConferenceId = Convert.ToInt32(read.GetValue(read.GetOrdinal("ConferenceId")));
                        outputConferencePlan.PeriodicType = read.GetValue(read.GetOrdinal("PeriodicType")).ToString();
                        outputConferencePlan.PeriodicValue= read.GetValue(read.GetOrdinal("PeriodicValue")).ToString();
                        outputConferencePlan.ConferenceBeginPeriod = read.GetValue(read.GetOrdinal("ConferenceBeginPeriod")).ToString();
                        outputConferencePlan.ConferenceEndPeriod = read.GetValue(read.GetOrdinal("ConferenceEndPeriod")).ToString();
                        outputConferencePlan.ConferenceStartTime = read.GetValue(read.GetOrdinal("ConferenceStartTime")).ToString();
                        outputConferencePlan.ConferenceStopTime = read.GetValue(read.GetOrdinal("ConferenceStopTime")).ToString();
                        ConferencePlanSeriesList.Add(outputConferencePlan);
                    }
                    Connect.Close(); // закрыть соединение
                }
                return ConferencePlanSeriesList;
            }
        }



        public bool sendUpdateToDataBase(string sqlComand)
        {
            bool error = false;
            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=" + DataBaseName + "; Version=3;"))
            {
                Connect.Open();
                SQLiteCommand Insert_Command = new SQLiteCommand(sqlComand, Connect);
                try
                {
                    Insert_Command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (e.Message.ToString().Contains("UNIQUE constraint failed: ConferenceTable.ConferenceId"))
                    {
                        MessageBox.Show("Селектор с таким ID уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    error = true;

                    throw;
                }
                
                Connect.Close();
                return error;
            }
        }


        // Заполнение DataGrid случайными данными

        //public List<ConferenceModel> GetConferences(int total = 100)
        //{
        //    List<ConferenceModel> output = new List<ConferenceModel>();
        //    for (int i = 0; i < total; i++)
        //    {

        //        output.Add(GetConference(i + 1));
        //    }
        //    return output;
        //}
        //private ConferenceModel GetConference(int id)
        //{
        //    ConferenceModel output = new ConferenceModel();

        //    output.ConferenceId = id;
        //    output.ConferenceName = GetRandomItem(conferenceName);
        //    output.ConferenceDuration = GetRandomItem(conferenceDuration);
        //    output.ParticipantsCount = GetRandomItem(participantsCount);
        //    output.IsAcive = GetRandomItem(isAcive);

        //    return output;
        //}



    }
}
