using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH1CodeChallenges
{
    class SimpleStudent
    {
        public string Name { get; set; }
        public int MathScore { get; set; }
        public int EngScore { get; set; }
        public int CompScore { get; set; }
        public int TotalScore { get; set; }

        public SimpleStudent()
        {
            Console.Write("Enter the student's name: ");
            Name = Console.ReadLine();
            MathScore = Program.ValidEntryEnforcer("Enter the student's Math Score: ");
            EngScore = Program.ValidEntryEnforcer("Enter the student's English Score: ");
            CompScore = Program.ValidEntryEnforcer("Enter the student's Computer Score: ");
            TotalScore = MathScore + EngScore + CompScore;
        }

        public void PrintStudent()
        {
            Console.Write("Student Name: {0}  Math Score: {1}  English Score: {2}  Computer Score: {3}  Total: {4}\n", Name, MathScore,
                            EngScore, CompScore, TotalScore);
        }

    }
}
