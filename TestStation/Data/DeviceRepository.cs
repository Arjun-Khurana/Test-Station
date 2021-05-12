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
                    I_Start             double not null, 
                    I_Step              double not null,
                    I_Stop              double not null,
                    I_Test              double not null,
                    I_Test_Tol          double not null,
                    P_Test_OB_Min       double not null,
                    P_Test_OB_Max       double not null,
                    V_Test_Min          double not null,
                    V_Test_Max          double not null,
                    VBR_Test            double not null,
                    IBM_Min             double not null,
                    IBM_Max             double not null,
                    P_Test_FC_Min       double not null,
                    P_Test_FC_Max       double not null,
                    I_OP_Min            double not null,
                    I_OP_Max            double not null,
                    P_BM_Test           double not null,
                    POPCT_Min           double not null,
                    IBM_Tracking_Min    double not null,
                    IBM_Tracking_Max    double not null,
                    RS_Min              double not null,
                    RS_Max              double not null,
                    SE_Min              double not null,
                    SE_Max              double not null,
                    Ith_Min             double not null,
                    Ith_Max             double not null,
                    Wiggle_Time         double not null,
                    Pwiggle_Max         double not null,
                    IBR_Max             double not null,
                    POPCT_Wiggle_Min    double not null
                )");
                cnn.Execute(
                    @"create table ROSADevice
                (
                    id                  integer primary key autoincrement,
                    Part_Number         varchar(255) not null,
                    V_Test              double not null,
                    RESP_Min            double not null,
                    I_TIA_Max             double not null,
                    PD_Wiggle_Max        double not null,
                    Wiggle_Time         double not null,
                    RSSI_VPD            varchar(255) not null
                )");
                cnn.Execute(
                    @"create table TOSAOutput
                (
                    id                      integer primary key autoincrement,
                    Part_Number             varchar(255) not null,
                    Job_Number              varchar(255) not null,
                    Unit_Number             integer not null,
                    Operator                varchar(255) not null,
                    Timestamp               datetime not null,
                    Repeat_Number           integer not null,
                    I_Test                  double not null,
                    P_Test_OB               double not null,
                    P_OB_Pass               boolean not null,
                    V_Test                  double not null,
                    V_Test_Pass             boolean not null,
                    IBM_Test_OB             double not null,
                    IBM_Pass                boolean not null,
                    VBR_Test                double not null,
                    IBR                     double not null,
                    IBR_Pass                boolean not null,
                    P_Test_FC               double not null,
                    P_Test_FC_Pass          boolean not null,
                    RS                      double not null,
                    RS_Pass                 boolean not null,
                    SE                      double not null,
                    SE_Pass                 boolean not null,
                    Ith                     double not null,
                    Ith_Pass                boolean not null,
                    POPCT                   double not null,
                    POPCT_Pass              boolean not null,
                    I_BM_Slope              double not null,
                    I_BM_P_BM_Test          double not null,
                    I_BM_P_BM_Test_Pass     boolean not null,
                    IBM_Track               double not null,
                    I_BM_Track_Pass         boolean not null,
                    POPCT_Wiggle_Min        double not null,
                    POPCT_Wiggle_Min_Pass   boolean not null,
                    Wiggle_dB               double not null,
                    Wiggle_dB_Pass          boolean not null,
                    Result                  boolean not null
                )");
                cnn.Execute(
                    @"create table ROSAOutput
                (
                    id                  integer primary key autoincrement,
                    Part_Number         varchar(255) not null,
                    Job_Number          varchar(255) not null,
                    Unit_Number         integer not null,
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
                    I_Start,
                    I_Step,
                    I_Stop,
                    I_Test,
                    I_Test_Tol,
                    P_Test_OB_Min,
                    P_Test_OB_Max,
                    V_Test_Min,
                    V_Test_Max,
                    VBR_Test,
                    IBM_Min,
                    IBM_Max,
                    P_Test_FC_Min,
                    P_Test_FC_Max,
                    I_OP_Min,
                    I_OP_Max,
                    P_BM_Test,
                    POPCT_Min,
                    IBM_Tracking_Min,
                    IBM_Tracking_Max,
                    RS_Min,
                    RS_Max,
                    SE_Min,
                    SE_Max,
                    Ith_Min,
                    Ith_Max,
                    Wiggle_Time,
                    Pwiggle_Max,
                    IBR_Max,
                    POPCT_Wiggle_Min
                ) values (
                    'HFE4192-911',         
                    2,
                    .5,
                    10,
                    7,
                    .1,
                    .3,
                    1,
                    1,
                    2,
                    -5,
                    .1,
                    1,
                    .1,
                    .7,
                    6,
                    8,
                    .4,
                    .7,
                    .8,
                    1.2,
                    25,
                    50,
                    .05,
                    .15,
                    .5,
                    2,
                    5,
                    1,
                    10,
                    .55
                )
                ");
                cnn.Execute(
                    @"insert into ROSADevice
                (
                    Part_Number,         
                    V_Test,              
                    RESP_Min,            
                    I_TIA_Max,           
                    PD_Wiggle_Max,        
                    Wiggle_Time,         
                    RSSI_VPD            
                ) values (
                    'VPD-4G',
                    3.3,
                    0.4,
                    35,
                    1,
                    5,
                    'VPD'
                )
                "
                );
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
	                    I_TIA_Max,
	                    PD_Wiggle_Max,
	                    Wiggle_Time,
	                    RSSI_VPD
                    )
                    values
                    (
                        @Part_Number,
	                    @V_Test,
	                    @RESP_Min,
	                    @I_TIA_Max,
	                    @PD_Wiggle_Max,
	                    @Wiggle_Time,
	                    @RSSI_VPD
                    )",
                    new
                    {
                        Part_Number = rosa.Part_Number,
                        V_Test = rosa.V_Test,
                        RESP_Min = rosa.RESP_Min,
                        I_TIA_Max = rosa.I_TIA_Max,
                        PD_Wiggle_Max = rosa.PD_Wiggle_Max,
                        Wiggle_Time = rosa.Wiggle_Time,
                        RSSI_VPD = rosa.RSSI_VPD
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
                    I_Start,
                    I_Step,
                    I_Stop,
                    I_Test,
                    P_Test_OB_Min,
                    P_Test_OB_Max,
                    V_Test_Min,
                    V_Test_Max,
                    VBR_Test,
                    IBM_Min,
                    IBM_Max,
                    P_Test_FC_Min,
                    P_Test_FC_Max,
                    I_OP_Min,
                    I_OP_Max,
                    P_BM_Test,
                    POPCT_Min,
                    IBM_Tracking_Min,
                    IBM_Tracking_Max,
                    RS_Min,
                    RS_Max,
                    SE_Min,
                    SE_Max,
                    Ith_Min,
                    Ith_Max,
                    Wiggle_Time,
                    Pwiggle_Max,
                    IBR_Max,
                    POPCT_Wiggle_Min,
                ) values (
                    @Part_Number,
                    @I_Start,
                    @I_Step,
                    @I_Stop,
                    @I_Test,
                    @P_Test_OB_Min,
                    @P_Test_OB_Max,
                    @V_Test_Min,
                    @V_Test_Max,
                    @VBR_Test,
                    @IBM_Min,
                    @IBM_Max,
                    @P_Test_FC_Min,
                    @P_Test_FC_Max,
                    @I_OP_Min,
                    @I_OP_Max,
                    @P_BM_Test,
                    @POPCT_Min,
                    @IBM_Tracking_Min,
                    @IBM_Tracking_Max,
                    @RS_Min,
                    @RS_Max,
                    @SE_Min,
                    @SE_Max,
                    @Ith_Min,
                    @Ith_Max,
                    @Wiggle_Time,
                    @Pwiggle_Max,
                    @IBR_Max,
                    @POPCT_Wiggle_Min
                )
                ",
                    new
                    {
                        Part_Number = tosa.Part_Number,
                        I_Start = tosa.I_Start,
                        I_Step = tosa.I_Step,
                        I_Stop = tosa.I_Stop,
                        I_Test = tosa.I_Test,
                        P_Test_OB_Min = tosa.P_Test_OB_Min,
                        P_Test_OB_Max = tosa.P_Test_OB_Max,
                        V_Test_Min = tosa.V_Test_Min,
                        V_Test_Max = tosa.V_Test_Max,
                        VBR_Test = tosa.VBR_Test,
                        IBM_Min = tosa.IBM_Min,
                        IBM_Max = tosa.IBM_Max,
                        P_Test_FC_Min = tosa.P_Test_FC_Min,
                        P_Test_FC_Max = tosa.P_Test_FC_Max,
                        I_OP_Min = tosa.I_OP_Min,
                        I_OP_Max = tosa.I_OP_Max,
                        P_BM_Test = tosa.P_BM_Test,
                        POPCT_Min = tosa.POPCT_Min,
                        IBM_Tracking_Min = tosa.IBM_Tracking_Min,
                        IBM_Tracking_Max = tosa.IBM_Tracking_Max,
                        RS_Min = tosa.RS_Min,
                        RS_Max = tosa.RS_Max,
                        SE_Min = tosa.SE_Min,
                        SE_Max = tosa.SE_Max,
                        Ith_Min = tosa.Ith_Min,
                        Ith_Max = tosa.Ith_Max,
                        Wiggle_Time = tosa.Wiggle_Time,
                        Pwiggle_Max = tosa.Pwiggle_Max,
                        IBR_Max = tosa.IBR_Max,
                        POPCT_Wiggle_Min = tosa.POPCT_Wiggle_Min
                    }
                );
            }
            
        }

        public void SaveTOSAOutput(TOSAOutput output)
        {
            if (!File.Exists(DbFile)) return;

            using (var cnn = DataFileConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @" insert into TOSAOutput 
                (                      
                    Part_Number,             
                    Job_Number,            
                    Unit_Number,             
                    Operator,                
                    Timestamp,               
                    Repeat_Number,           
                    I_Test,                  
                    P_Test_OB,               
                    P_OB_Pass,              
                    V_Test,                  
                    V_Test_Pass,             
                    IBM_Test_OB,             
                    IBM_Pass,                
                    VBR_Test,                
                    IBR,                    
                    IBR_Pass,                
                    P_Test_FC,               
                    P_Test_FC_Pass,          
                    RS,                      
                    RS_Pass,                 
                    SE,                      
                    SE_Pass,                 
                    Ith,                     
                    Ith_Pass,                
                    POPCT,                   
                    POPCT_Pass,              
                    I_BM_Slope,              
                    I_BM_P_BM_Test,          
                    I_BM_P_BM_Test_Pass,     
                    IBM_Track,               
                    I_BM_Track_Pass,         
                    POPCT_Wiggle_Min,        
                    POPCT_Wiggle_Min_Pass,   
                    Wiggle_dB,               
                    Wiggle_dB_Pass,          
                    Result
                ) values (                    
                    @Part_Number,             
                    @Job_Number,            
                    @Unit_Number,             
                    @Operator,                
                    @Timestamp,               
                    @Repeat_Number,           
                    @I_Test,                  
                    @P_Test_OB,               
                    @P_OB_Pass,              
                    @V_Test,                  
                    @V_Test_Pass,             
                    @IBM_Test_OB,             
                    @IBM_Pass,                
                    @VBR_Test,                
                    @IBR,                    
                    @IBR_Pass,                
                    @P_Test_FC,               
                    @P_Test_FC_Pass,          
                    @RS,                      
                    @RS_Pass,                 
                    @SE,                      
                    @SE_Pass,                 
                    @Ith,                     
                    @Ith_Pass,                
                    @POPCT,                   
                    @POPCT_Pass,              
                    @I_BM_Slope,              
                    @I_BM_P_BM_Test,          
                    @I_BM_P_BM_Test_Pass,     
                    @IBM_Track,               
                    @I_BM_Track_Pass,         
                    @POPCT_Wiggle_Min,        
                    @POPCT_Wiggle_Min_Pass,   
                    @Wiggle_dB,               
                    @Wiggle_dB_Pass,          
                    @Result
                )
                ",
                    new
                    {
                        Part_Number = output.Part_Number,
                        Job_Number = output.Job_Number,
                        Unit_Number = output.Unit_Number,
                        Operator = output.Operator,
                        Timestamp = output.Timestamp,
                        Repeat_Number = output.Repeat_Number,
                        I_Test = output.I_Test,
                        P_Test_OB = output.P_Test_OB,
                        P_OB_Pass = output.P_OB_Pass,
                        V_Test = output.V_Test,
                        V_Test_Pass = output.V_Test_Pass,
                        IBM_Test_OB = output.IBM_Test_OB,
                        IBM_Pass = output.IBM_Pass,
                        VBR_Test = output.VBR_Test,
                        IBR = output.IBR,
                        IBR_Pass = output.IBR_Pass,
                        P_Test_FC = output.P_Test_FC,
                        P_Test_FC_Pass = output.P_Test_FC_Pass,
                        RS = output.RS,
                        RS_Pass = output.RS_Pass,
                        SE = output.SE,
                        SE_Pass = output.SE_Pass,
                        Ith = output.Ith,
                        Ith_Pass = output.Ith_Pass,
                        POPCT = output.POPCT,
                        POPCT_Pass = output.POPCT_Pass,
                        I_BM_Slope = output.I_BM_Slope,
                        I_BM_P_BM_Test = output.I_BM_P_BM_Test,
                        I_BM_P_BM_Test_Pass = output.I_BM_P_BM_Test_Pass,
                        IBM_Track = output.IBM_Track,
                        I_BM_Track_Pass = output.I_BM_Track_Pass,
                        POPCT_Wiggle_Min = output.POPCT_Wiggle_Min,
                        POPCT_Wiggle_Min_Pass = output.POPCT_Wiggle_Min_Pass,
                        Wiggle_dB = output.Wiggle_dB,
                        Wiggle_dB_Pass = output.Wiggle_dB_Pass,
                        Result = output.Result
                    }
                );
            }

        }

        public int GetMaxTOSAUnitNumber(string jobNumber)
        {
            using (var conn = DataFileConnection())
            {
                conn.Open();

                int? num = conn.Query<int?>(
                    @"select max(Unit_Number) from TOSAOutput where Job_Number = @jobNumber", new { jobNumber }).FirstOrDefault();

                return (int)(num == null ? 0 : num);
            }
        }

        public int GetMaxROSAUnitNumber(string jobNumber)
        {
            using (var conn = DataFileConnection())
            {
                conn.Open();

                int? num = conn.Query<int?>(
                    @"select max(Unit_Number) from ROSAOutput where Job_Number = @jobNumber", new { jobNumber }).FirstOrDefault();

                return (int)(num == null ? 0 : num);
            }
        }

    }
}
