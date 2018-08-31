using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH3CodeChallenges
{
    class Student
    {
        public int Marks { get; set; }

        //shows operator + overload method
        public static Student operator +(Student s1, Student s2)
        {
            Student s = new Student();
            s.Marks = s1.Marks + s2.Marks;
            return s;
        }
    }
}
