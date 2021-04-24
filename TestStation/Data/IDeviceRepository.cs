using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStation.Models;

namespace TestStation.Data
{
    public interface IDeviceRepository
    {
        TOSADevice GetTOSADevice(int id);
        ROSADevice GetROSADevice(int id);
        void SaveTOSADevice(TOSADevice tosa);
        void SaveROSADevice(ROSADevice tosa);
        void SaveTOSAOutput(TOSAOutput result);
        void SaveROSAOutput(ROSAOutput result);
    }

    public class SqLiteBaseRepository
    {
        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\DataFile.sqlite"; }
        }

        public static SQLiteConnection DataFileConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }
    }

}
