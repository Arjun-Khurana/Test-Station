using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStation.Models;
using System.Data.SQLite;
using Dapper;
using System.IO;

namespace TestStation.Data
{
    public class DeviceRepository : SqLiteBaseRepository, IDeviceRepository
    {
        public DeviceRepository()
        {
            if (!File.Exists(DbFile))
            {
                CreateDatabase();
            }

        }

        private static void CreateDatabase()
        {
            using (var cnn = DataFileConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @"create table TOSADevice
                (
                    id                  integer primary key autoincrement,
                    Part_Number         varchar(255) not null,
                    I_Continuity        double not null,
                    V_Continuity_Min    double not null,
                    V_Continuity_Max    double not null,
                    I_Continuity_Tol    double not null,
                    I_Start             double not null,
                    I_Step              double not null,
                    I_Stop              double not null,
                    P_OP                double not null,
                    I_OP_Min            double not null,
                    I_OP_Max            double not null,
                    Wiggle_Time         double not null,
                    VBR_Test            double not null,
                    V_OP_Min            double not null,
                    V_OP_Max            double not null,
                    RS_Min              double not null,
                    RS_Max              double not null,
                    SE_Min              double not null,
                    SE_Max              double not null,
                    Ith_Min             double not null,
                    Ith_Max             double not null,
                    Pwiggle_Max         double not null,
                    POPCT_Min           double not null,
                    IBM_Min             double not null,
                    IBM_Max             double not null,
                    IBM_Tracking_Min    double not null,
                    IBM_Tracking_Max    double not null,
                    IBR_Max             double not null
                )");
                cnn.Execute(
                    @"create table ROSADevice
                (
                    id                  integer primary key autoincrement,
                    Part_Number         varchar(255) not null,
                    V_Test              double not null,
                    RESP_Min            double not null,
                    ICC_Max             double not null,
                    I_Wiggle_Max        double not null,
                    Wiggle_Time         double not null,
                    RSSSI_VPD           double not null
                )");
                cnn.Execute(
                    @"create table TOSAOutput
                (
                    id                  integer primary key autoincrement,
                    Part_Number         varchar(255) not null,
                    Job_Number          varchar(255) not null,
                    Unit_Number         varchar(255) not null,
                    Operator            varchar(255) not null,
                    Timestamp           datetime not null,
                    Repeat_Number       integer not null,
                    I_OP                double not null,
                    P_OP                double not null,
                    VBR_Test            double not null,
                    P_Total             double not null,
                    V_OP                double not null,
                    RS                  double not null,
                    SE                  double not null,
                    Ith                 double not null,
                    POPCT               double not null,
                    POPCT_Min           double not null,
                    Pwiggle             double not null,
                    IBM                 double not null,
                    IBM_Tracking        double not null,
                    IBR                 double not null,
                    V_OP_Pass           boolean not null,
                    I_OP_Pass           boolean not null,
                    RS_Pass             boolean not null,
                    SE_Pass             boolean not null,
                    Ith_Pass            boolean not null,
                    Pwiggle_Pass        boolean not null,
                    POPCT_Min_Pass      boolean not null,
                    IBM_Pass            boolean not null,
                    IBM_Tracking_Pass   boolean not null,
                    IBR_Pass            boolean not null,
                    Result              boolean not null
                )");
                cnn.Execute(
                    @"create table ROSAOutput
                (
                    id                  integer primary key autoincrement,
                    Part_Number         varchar(255) not null,
                    Job_Number          varchar(255) not null,
                    Unit_Number         varchar(255) not null,
                    Operator            varchar(255) not null,
                    Timestamp           datetime not null,
                    Repeat_Number       integer not null,
                    V_Test              double not null,
                    P_Laser             double not null,
                    I_TIA               double not null,
                    I_PD                double not null,
                    Responsivity        double not null,
                    I_Dark              double not null,
                    I_TIA_Pass          boolean not null,
                    RESP_Pass           boolean not null,
                    Wiggle_pass         boolean not null,
                    Result              boolean not null
                )");

                cnn.Execute(
                    @"insert into TOSADevice
                (                  
                    Part_Number,         
                    I_Continuity,        
                    V_Continuity_Min,    
                    V_Continuity_Max,    
                    I_Continuity_Tol,    
                    I_Start,             
                    I_Step,              
                    I_Stop,              
                    P_OP,                
                    I_OP_Min,            
                    I_OP_Max,            
                    Wiggle_Time,         
                    VBR_Test,            
                    V_OP_Min,            
                    V_OP_Max,            
                    RS_Min,              
                    RS_Max,              
                    SE_Min,              
                    SE_Max,              
                    Ith_Min,             
                    Ith_Max,             
                    Pwiggle_Max,         
                    POPCT_Min,           
                    IBM_Min,             
                    IBM_Max,             
                    IBM_Tracking_Min,    
                    IBM_Tracking_Max,    
                    IBR_Max             
                ) values (
                    '12345',         
                    1,        
                    1,    
                    3,    
                    .1,    
                    .1,             
                    .1,              
                    12,              
                    7,                
                    5,            
                    10,                          
                    10,         
                    -5,            
                    1.8,            
                    2,            
                    40,              
                    100,              
                    .1,              
                    .6,              
                    .3,             
                    2,             
                    1,         
                    .65,           
                    .1,             
                    .5,             
                    .2,    
                    .8,    
                    100             
                )
                ");
            }
        }

        public ROSADevice GetROSADevice(int id)
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = DataFileConnection())
            {
                cnn.Open();
                ROSADevice result = cnn.Query<ROSADevice>(
                    @"select * from ROSADevice where id = @id", new { id }).FirstOrDefault();
                return result;
            }
        }

        public TOSADevice GetTOSADevice(int id)
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = DataFileConnection())
            {
                cnn.Open();
                TOSADevice result = cnn.Query<TOSADevice>(
                    @"select * from TOSADevice where id = @id", new { id }).FirstOrDefault();
                return result;
            }
        }

        public List<TOSADevice> GetAllTosaDevices()
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = DataFileConnection())
            {
                cnn.Open();
                List<TOSADevice> result = cnn.Query<TOSADevice>(@"select * from TOSADevice").ToList();
                return result;
            }
        }

        public List<ROSADevice> GetAllRosaDevices()
        {
            if (!File.Exists(DbFile)) return null;

            using (var cnn = DataFileConnection())
            {
                cnn.Open();
                List<ROSADevice> result = cnn.Query<ROSADevice>(@"select * from ROSADevice").ToList();
                return result;
            }
        }

        public void SaveROSADevice(ROSADevice rosa)
        {
            if (!File.Exists(DbFile)) return;

            using (var cnn = DataFileConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @"insert into ROSADevice
                    (
                        Part_Number,
	                    V_Test,
	                    RESP_Min,
	                    ICC_Max,
	                    I_Wiggle_Max,
	                    Wiggle_Time,
	                    RSSSI_VPD
                    )
                    values
                    (
                        @Part_Number,
	                    @V_Test,
	                    @RESP_Min,
	                    @ICC_Max,
	                    @I_Wiggle_Max,
	                    @Wiggle_Time,
	                    @RSSSI_VPD
                    )",
                    new
                    {
                        Part_Number = rosa.Part_Number,
                        V_Test = rosa.V_Test,
                        RESP_Min = rosa.RESP_Min,
                        ICC_Max = rosa.ICC_Max,
                        I_Wiggle_Max = rosa.I_Wiggle_Max,
                        Wiggle_Time = rosa.Wiggle_Time,
                        RSSSI_VPD = rosa.RSSSI_VPD
                    }
                );
            }
        }

        public void SaveROSAOutput(ROSAOutput result)
        {
            throw new NotImplementedException();
        }

        public void SaveTOSADevice(TOSADevice tosa)
        {
            if (!File.Exists(DbFile)) return;
            
            using (var cnn = DataFileConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @"insert into TOSADevice
                    (
                        Part_Number,
	                    I_Continuity,
	                    V_Continuity_Min,
	                    V_Continuity_Max,
	                    I_Continuity_Tol,
	                    I_Start,
	                    I_Step,
	                    I_Stop,
	                    P_OP,
	                    I_OP_Min,
	                    I_OP_Max,
	                    Wiggle_Time,
	                    VBR_Test,
	                    V_OP_Min,
	                    V_OP_Max,
	                    RS_Min,
	                    RS_Max,
	                    SE_Min,
	                    SE_Max,
	                    Ith_Min,
	                    Ith_Max,
	                    Pwiggle_Max,
	                    POPCT_Min,
	                    IBM_Min,
	                    IBM_Max,
	                    IBM_Tracking_Min,
	                    IBM_Tracking_Max,
	                    IBR_Max
                    )
                    values
                    (
                        @Part_Number,
	                    @I_Continuity,
	                    @V_Continuity_Min,
	                    @V_Continuity_Max,
	                    @I_Continuity_Tol,
	                    @I_Start,
	                    @I_Step,
	                    @I_Stop,
	                    @P_OP,
	                    @I_OP_Min,
	                    @I_OP_Max,
	                    @Wiggle_Time,
	                    @VBR_Test,
	                    @V_OP_Min,
	                    @V_OP_Max,
	                    @RS_Min,
	                    @RS_Max,
	                    @SE_Min,
	                    @SE_Max,
	                    @Ith_Min,
	                    @Ith_Max,
	                    @Pwiggle_Max,
	                    @POPCT_Min,
	                    @IBM_Min,
	                    @IBM_Max,
	                    @IBM_Tracking_Min,
	                    @IBM_Tracking_Max,
	                    @IBR_Max
                    )",
                    new
                    {
                        Part_Number = tosa.Part_Number,
                        I_Continuity = tosa.I_Continuity,
                        V_Continuity_Min = tosa.V_Continuity_Min,
                        V_Continuity_Max = tosa.V_Continuity_Max,
                        I_Continuity_Tol = tosa.I_Continuity_Tol,
                        I_Start = tosa.I_Start,
                        I_Step = tosa.I_Step,
                        I_Stop = tosa.I_Stop,
                        P_OP = tosa.P_OP,
                        I_OP_Min = tosa.I_OP_Min,
                        I_OP_Max = tosa.I_OP_Max,
                        Wiggle_Time = tosa.Wiggle_Time,
                        VBR_Test = tosa.VBR_Test,
                        V_OP_Min = tosa.V_OP_Min,
                        V_OP_Max = tosa.V_OP_Max,
                        RS_Min = tosa.RS_Min,
                        RS_Max = tosa.RS_Max,
                        SE_Min = tosa.SE_Min,
                        SE_Max = tosa.SE_Max,
                        Ith_Min = tosa.Ith_Min,
                        Ith_Max = tosa.Ith_Max,
                        Pwiggle_Max = tosa.Pwiggle_Max,
                        POPCT_Min = tosa.POPCT_Min,
                        IBM_Min = tosa.IBM_Min,
                        IBM_Max = tosa.IBM_Max,
                        IBM_Tracking_Min = tosa.IBM_Tracking_Min,
                        IBM_Tracking_Max = tosa.IBM_Tracking_Max,
                        IBR_Max = tosa.IBR_Max
                    }
                );
            }
            
        }

        public void SaveTOSAOutput(TOSAOutput result)
        {
            throw new NotImplementedException();
        }
    }
}
