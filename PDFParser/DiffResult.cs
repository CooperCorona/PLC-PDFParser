using System;
namespace PDFParser
{
    public struct DiffResult
    {
        public bool IsEmpty { get; private set; }
        public int SectionNumber { get; private set; }
        public int TestNumber { get; private set; }
        public string Diff { get; private set; }
        public string Submission { get; private set; }
        public string Solution { get; private set; }
        public string Error { get; private set; }
        public string Maze { get; private set; }
        public string Moves { get; private set; }

        public DiffResult(bool isEmpty, int sectionNumber, int testNumber, string diff, string submission, string solution, string error, string maze, string moves) {
            IsEmpty = isEmpty;
            SectionNumber = sectionNumber;
            TestNumber = testNumber;
            Diff = diff;
            Submission = submission;
            Solution = solution;
            Error = error;
            Maze = maze;
            Moves = moves;
        }

    }
}
