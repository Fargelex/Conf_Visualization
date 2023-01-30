using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conf_visualization_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            loadConferencesFromFIle();
            Console.ReadKey();
        }



        public static void loadConferencesFromFIle()
        {

            //for (int i = 0; i < total; i++)
            //{

            //    ConferenceList.Add(GetConference(i + 1));
            //}




            string sqlcomand = "INSERT INTO ConferenceTable (ConferenceId, ParticipantsCount,ConferenceDuration) VALUES";
            string valuesForcomand = "";
            File.ReadAllLines("123.txt");
            foreach (var item in File.ReadAllLines("123.txt"))
            {
                string[] splt = item.Split('\t');
                valuesForcomand += String.Format(" ( {0},{1},{2} ), ", splt[0], splt[1], splt[2]);
            }
            valuesForcomand = valuesForcomand.Remove(valuesForcomand.Length - 2, 2);
            valuesForcomand += ';';
            sqlcomand += valuesForcomand;

            using (SQLiteConnection Connect = new SQLiteConnection(@"Data Source=database.db; Version=3;"))
            {
                Connect.Open();

                SQLiteCommand Insert_Command = new SQLiteCommand(sqlcomand, Connect);
                Insert_Command.ExecuteNonQuery();


                Connect.Close();
            }


        }
    }
}
