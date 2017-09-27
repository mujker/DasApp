using System;
using System.Collections.Generic;

namespace DasApp.Socket
{
    /// <summary>
    /// 监控点
    /// </summary>
    public class JKD
    {
        public List<JKCSZ> LJ = null;
        public string JKD_ID { get; set; }

        public string this[string jkcsname]
        {
            get
            {
                string rtstr = "";
                foreach (JKCSZ jkz in LJ)
                {
                    if (jkz.JKCS == jkcsname)
                    {
                        rtstr = jkz.JKZ;
                        return rtstr;
                    }
                }
                return rtstr;
            }
        }
    }

    /// <summary>
    /// 监控点时间
    /// </summary>
    public class JKCSZ
    {
        /// <summary>
        /// 监控参数
        /// </summary>
        public string JKCS = "????";

        /// <summary>
        /// 监控值
        /// </summary>
        public string JKZ = "????";

        /// <summary>
        /// 监控时间
        /// </summary>
        public string JKSJ = "????";
    }

    public class GetHDCvalue
    {
        public static JKD GetValues(string jkdm)
        {
            try
            {
                JKD reJkd = new JKD {LJ = new List<JKCSZ>()};
                if (string.IsNullOrEmpty(jkdm)) return reJkd;
                string[] Bigsplits = jkdm.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
                //监控点ID
                reJkd.JKD_ID = Bigsplits[1];
                //监控点内容
                string JKDContent = Bigsplits[2];
                //每条监控点
                string[] EveryJKDS = JKDContent.Split(new string[] {"@"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in EveryJKDS)
                {
                    string[] jkcszs = item
                        .Split(new string[] {"&&"}, StringSplitOptions.RemoveEmptyEntries);
                    //监控参数ID
                    string jkcsid = jkcszs[0];
                    string jkcsvalue = jkcszs[1];
                    string jksj = jkcszs[2];
                    JKCSZ mycsz = new JKCSZ
                    {
                        JKCS = jkcsid,
                        JKZ = jkcsvalue,
                        JKSJ = jksj
                    };
                    reJkd.LJ.Add(mycsz);
                }
                return reJkd;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}