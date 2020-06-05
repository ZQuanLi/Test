using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YDS6000.Models.Tables
{
    public class ExpTimingModels
    {
        private string T00 = null;
        private string D01 = null;
        private string D02 = null;
        private string D03 = null;
        private string D04 = null;
        private string D05 = null;
        private string D06 = null;
        private string D07 = null;
        private string D08 = null;

        public string t00
        {
            get
            {
                return T00;
            }

            set
            {
                T00 = value;
            }
        }

        public string d01
        {
            get
            {
                return D01;
            }

            set
            {
                D01 = value;
            }
        }

        public string d02
        {
            get
            {
                return D02;
            }

            set
            {
                D02 = value;
            }
        }

        public string d03
        {
            get
            {
                return D03;
            }

            set
            {
                D03 = value;
            }
        }

        public string d04
        {
            get
            {
                return D04;
            }

            set
            {
                D04 = value;
            }
        }

        public string d05
        {
            get
            {
                return D05;
            }

            set
            {
                D05 = value;
            }
        }

        public string d06
        {
            get
            {
                return D06;
            }

            set
            {
                D06 = value;
            }
        }

        public string d07
        {
            get
            {
                return D07;
            }

            set
            {
                D07 = value;
            }
        }

        public string d08
        {
            get
            {
                return D08;
            }

            set
            {
                D08 = value;
            }
        }
    }

    
    public class DataModels
    {
        private string data = null;

        /// <summary>
        /// Json格式的数据,转格式var data = JSON.stringify(this.rows);
        /// </summary>
        public string Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }
    }

}
