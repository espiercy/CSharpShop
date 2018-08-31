using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//needs accept input for the total number of students
//for each student, needs to save a name, three grades (English, Math, Computer)
//calculate the total from the three grades
//show the student names/grades by grade in descending order...
//The book suggests using a multidimensional array...that's dumb. Very dumb
namespace SGCH1CodeChallenges
{
    class SimpleRepCard
    {
        private SimpleStudent[] studArray;
        public SimpleRepCard()
        {
            int studArraySize = Program.ValidEntryEnforcer("Enter the number of students: ");
            studArray = new SimpleStudent[studArraySize];
            for(int i = 0; i < studArraySize; i++)
            {
                studArray[i] = new SimpleStudent();
            }
        }

        public void PrintStudentReport()
        {
            for(int i = 300; i > 0; --i)
            {
                foreach(SimpleStudent student in studArray)
                {
                    if(student.TotalScore == i)
                    {
                        student.PrintStudent();
                    }
                }
            }
        }
    }
}
