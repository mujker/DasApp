using System;
using System.Collections.Generic;

namespace DasApp.Socket
{
    /// <summary>
    ///     监控点
    /// </summary>
    public class JKD
    {
        public List<JKCSZ> LJ;
        public string JKD_ID { get; set; }

        public string this[string jkcsname]
        {
            get
            {
                var rtstr = "";
                foreach (var jkz in LJ)
                    if (jkz.JKCS == jkcsname)
                    {
                        rtstr = jkz.JKZ;
                        return rtstr;
                    }
                return rtstr;
            }
        }
    }

    /// <summary>
    ///     监控点时间
    /// </summary>
    public class JKCSZ
    {
        /// <summary>
        ///     监控参数
        /// </summary>
        public string JKCS = "????";

        /// <summary>
        ///     监控时间
        /// </summary>
        public string JKSJ = "????";

        /// <summary>
        ///     监控值
        /// </summary>
        public string JKZ = "????";
    }

    public class GetHDCvalue
    {
        public static JKD GetValues(string jkdm)
        {
            try
            {
                var reJkd = new JKD {LJ = new List<JKCSZ>()};
                if (string.IsNullOrEmpty(jkdm)) return reJkd;
                var Bigsplits = jkdm.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                //监控点ID
                reJkd.JKD_ID = Bigsplits[1];
                //监控点内容
                var JKDContent = Bigsplits[2];
                //每条监控点
                var EveryJKDS = JKDContent.Split(new[] {"@"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in EveryJKDS)
                {
                    var jkcszs = item
                        .Split(new[] {"&&"}, StringSplitOptions.RemoveEmptyEntries);
                    //监控参数ID
                    var jkcsid = jkcszs[0];
                    var jkcsvalue = jkcszs[1];
                    var jksj = jkcszs[2];
                    var mycsz = new JKCSZ
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