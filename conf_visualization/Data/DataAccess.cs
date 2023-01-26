﻿using conf_visualization.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conf_visualization.Data
{
    internal class DataAccess
    {
        Random rnd = new Random();

        int[] conferenceId = new int[] { 540, 424, 422, 80, 57, 25, 350, 150, 20, 10 };
        string[] conferenceName = new string[] { "Селектор 1", "Селектор 2", "Селектор 3", "Селектор 4", "Селектор 5", "Селектор 6", "Селектор 7", "Селектор 8", "Селектор 9", "Селектор 10" };
        int[] participantsCount = new int[] { 30,30,44,80,57,25,35,15,20,10 };
        int[] conferenceDuration = new int[] { 360, 360, 180, 60, 240, 90, 100, 35, 20, 30 };    

    bool[] isAcive = new bool[] { true, false };

        DateTime lowEndDate = new DateTime(1943, 1, 1);
        int daysFromLowDate;

        public DataAccess()
        {
            daysFromLowDate = (DateTime.Today - lowEndDate).Days;
        }

        //Подключение к Базе Данных
        //public List<ConferenceModel> GetConferences()
        //{
        //    List<ConferenceModel> ConferenceList = new List<ConferenceModel>();

        //    //for (int i = 0; i < total; i++)
        //    //{

        //    //    ConferenceList.Add(GetConference(i + 1));
        //    //}

        //    using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=database.db; Version=3;"))
        //    {
        //        Connect.Open();

        //        SQLiteCommand comm = new SQLiteCommand("Select * From ConferenceTable", Connect);
        //        using (SQLiteDataReader read = comm.ExecuteReader())
        //        {
        //            while (read.Read())
        //            {
        //                ConferenceModel outputConference = new ConferenceModel();

        //                outputConference.ConferenceId = Convert.ToInt32(read.GetValue(read.GetOrdinal("ConferenceId")));
        //                outputConference.ConferenceName = read.GetValue(read.GetOrdinal("ConferenceName")).ToString();
        //                outputConference.ConferenceDuration = Convert.ToInt32(read.GetValue(read.GetOrdinal("ConferenceDuration")));
        //                outputConference.ParticipantsCount = Convert.ToInt32(read.GetValue(read.GetOrdinal("ParticipantsCount")));
        //                outputConference.IsAcive = Convert.ToBoolean(read.GetValue(read.GetOrdinal("IsAcive")));
        //                ConferenceList.Add(outputConference);
        //            }
        //            Connect.Close(); // закрыть соединение
        //                             //      add_to_main_log("info\tновая БД с именем " + data_base_filename + " создана. Полей для экспорта: ");
        //        }
        //        return ConferenceList;
        //    }
        //}

        //Заполняет таблицы случайными данными
        public List<ConferenceModel> GetConferences(int total = 100)
        {
            List<ConferenceModel> output = new List<ConferenceModel>();

            for (int i = 0; i < total; i++)
            {

                output.Add(GetConference(i + 1));
            }

            return output;
        }
        private ConferenceModel GetConference(int id)
        {
            ConferenceModel output = new ConferenceModel();

            output.ConferenceId = id;
            output.ConferenceName = GetRandomItem(conferenceName);
            output.ConferenceDuration = GetRandomItem(conferenceDuration);
            output.ParticipantsCount = GetRandomItem(participantsCount);
            output.IsAcive = GetRandomItem(isAcive);
          //  output.DateOfBirth = GetRandomDate();
         //   output.Age = GetAgeInYears(output.DateOfBirth);
            //output.AccountBalance = ((decimal)rnd.Next(1, 1000000) / 100);

            //int addressCount = rnd.Next(1, 5);

            //for (int i = 0; i < addressCount; i++)
            //{
            //    output.Addresses.Add(GetAddress(((id - 1) * 5) + i + 1));
            //}

            return output;
        }

        private T GetRandomItem<T>(T[] data)
        {
            return data[rnd.Next(0, data.Length)];
        }

        private DateTime GetRandomDate()
        {
            return lowEndDate.AddDays(rnd.Next(daysFromLowDate));
        }

        private int GetAgeInYears(DateTime birthday)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
}
