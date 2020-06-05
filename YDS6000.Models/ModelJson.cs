using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace YDS6000.Models
{
    #region 拉合闸策略
    [DataContract]
    public class JsonSiModel
    {
        private time _d01 = new time();
        private time _d02 = new time();
        private time _d03 = new time();
        private time _d04 = new time();
        private time _d05 = new time();
        private time _d06 = new time();
        private time _d07 = new time();
        private time _d08 = new time();

        [DataMember]
        public time d01 { get { return _d01; } set { _d01 = value; } }
        [DataMember]
        public time d02 { get { return _d02; } set { _d02 = value; } }
        [DataMember]
        public time d03 { get { return _d03; } set { _d03 = value; } }
        [DataMember]
        public time d04 { get { return _d04; } set { _d04 = value; } }
        [DataMember]
        public time d05 { get { return _d05; } set { _d05 = value; } }
        [DataMember]
        public time d06 { get { return _d06; } set { _d06 = value; } }
        [DataMember]
        public time d07 { get { return _d07; } set { _d07 = value; } }
        [DataMember]
        public time d08 { get { return _d08; } set { _d08 = value; } }

        public class time
        {
            private Value _t01 = new Value();
            private Value _t02 = new Value();
            private Value _t03 = new Value();
            private Value _t04 = new Value();
            private Value _t05 = new Value();
            private Value _t06 = new Value();
            private Value _t07 = new Value();
            private Value _t08 = new Value();
            private Value _t09 = new Value();
            private Value _t10 = new Value();
            private Value _t11 = new Value();
            private Value _t12 = new Value();
            private Value _t13 = new Value();
            private Value _t14 = new Value();

            public Value t01 { get { return _t01; } set { _t01 = value; } }
            public Value t02 { get { return _t02; } set { _t02 = value; } }
            public Value t03 { get { return _t03; } set { _t03 = value; } }
            public Value t04 { get { return _t04; } set { _t04 = value; } }
            public Value t05 { get { return _t05; } set { _t05 = value; } }
            public Value t06 { get { return _t06; } set { _t06 = value; } }
            public Value t07 { get { return _t07; } set { _t07 = value; } }
            public Value t08 { get { return _t08; } set { _t08 = value; } }

            public string label { get; set; }
            //public Value t09 { get { return _t09; } set { _t09 = value; } }
            //public Value t10 { get { return _t10; } set { _t10 = value; } }
            //public Value t11 { get { return _t11; } set { _t11 = value; } }
            //public Value t12 { get { return _t12; } set { _t12 = value; } }
            //public Value t13 { get { return _t13; } set { _t13 = value; } }
            //public Value t14 { get { return _t14; } set { _t14 = value; } }
        }

        public class Value
        {
            private string _hm = "00:00";
            private string _sr = "0000";
            public string hm
            {
                get { return _hm; }
                set { _hm = value; }
            }
            public string sr
            {
                get { return _sr; }
                set { _sr = value; }
            }
        }
    }

    [DataContract]
    public class JsonSiMdModel
    {
        private Value _md01 = new Value();
        private Value _md02 = new Value();
        private Value _md03 = new Value();
        private Value _md04 = new Value();
        private Value _md05 = new Value();
        private Value _md06 = new Value();
        private Value _md07 = new Value();

        [DataMember]
        public Value md01 { get { return _md01; } set { _md01 = value; } }
        [DataMember]
        public Value md02 { get { return _md02; } set { _md02 = value; } }
        [DataMember]
        public Value md03 { get { return _md03; } set { _md03 = value; } }
        [DataMember]
        public Value md04 { get { return _md04; } set { _md04 = value; } }
        [DataMember]
        public Value md05 { get { return _md05; } set { _md05 = value; } }
        [DataMember]
        public Value md06 { get { return _md06; } set { _md06 = value; } }
        [DataMember]
        public Value md07 { get { return _md07; } set { _md07 = value; } }

        public class Value
        {
            private string _md = "0000";
            private string _si = "00";

            public string md
            {
                get { return _md; }
                set { _md = value; }
            }

            public string si
            {
                get { return _si; }
                set { _si = value; }
            }
        }
    }

    [DataContract]
    public class JsonSiWkModel
    {
        private string _si01 = "00";
        private string _si02 = "00";
        private string _si03 = "00";
        private string _si04 = "00";
        private string _si05 = "00";
        private string _si06 = "00";
        private string _si07 = "00";

        [DataMember]
        public string si01 { get { return _si01; } set { _si01 = value; } }
        [DataMember]
        public string si02 { get { return _si02; } set { _si02 = value; } }
        [DataMember]
        public string si03 { get { return _si03; } set { _si03 = value; } }
        [DataMember]
        public string si04 { get { return _si04; } set { _si04 = value; } }
        [DataMember]
        public string si05 { get { return _si05; } set { _si05 = value; } }
        [DataMember]
        public string si06 { get { return _si06; } set { _si06 = value; } }
        [DataMember]
        public string si07 { get { return _si07; } set { _si07 = value; } }

        //[DataMember]
        //public int di01 { get; set; }
        //[DataMember]
        //public int di02 { get; set; }
        //[DataMember]
        //public int di03 { get; set; }
        //[DataMember]
        //public int di04 { get; set; }
        //[DataMember]
        //public int di05 { get; set; }
        //[DataMember]
        //public int di06 { get; set; }
        //[DataMember]
        //public int di07 { get; set; }
    }

    [DataContract]
    public class JsonSiTsModel
    {
        private Value _ts01 = new Value();
        private Value _ts02 = new Value();
        private Value _ts03 = new Value();
        private Value _ts04 = new Value();
        private Value _ts05 = new Value();
        private Value _ts06 = new Value();
        private Value _ts07 = new Value();

        [DataMember]
        public Value ts01 { get { return _ts01; } set { _ts01 = value; } }
        [DataMember]
        public Value ts02 { get { return _ts02; } set { _ts02 = value; } }
        [DataMember]
        public Value ts03 { get { return _ts03; } set { _ts03 = value; } }
        [DataMember]
        public Value ts04 { get { return _ts04; } set { _ts04 = value; } }
        [DataMember]
        public Value ts05 { get { return _ts05; } set { _ts05 = value; } }
        [DataMember]
        public Value ts06 { get { return _ts06; } set { _ts06 = value; } }
        [DataMember]
        public Value ts07 { get { return _ts07; } set { _ts07 = value; } }

        public class Value
        {
            private string _dt = "00000000";
            private string _si = "00";

            public string dt
            {
                get { return _dt; }
                set { _dt = value; }
            }

            public string si
            {
                get { return _si; }
                set { _si = value; }
            }
        }
    }
    #endregion
}
