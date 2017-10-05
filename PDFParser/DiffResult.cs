using System;
namespace PDFParser
{
    /// <summary>
    /// Structure aggregating all Webgrader output per test.
    /// </summary>
    public struct DiffResult
    {
        /// <summary>
        /// Gets a value indicating whether the diff is empty.
        /// </summary>
        /// <value><c>true</c> if the diff is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty { get; private set; }
        /// <summary>
        /// Gets the section number. What part of the assignment this test
        /// corresponds to.
        /// </summary>
        /// <value>The section number.</value>
        public int SectionNumber { get; private set; }
        /// <summary>
        /// What number of test this is in a given section.
        /// </summary>
        /// <value>The test number.</value>
        public int TestNumber { get; private set; }
        /// <summary>
        /// The diff.
        /// </summary>
        /// <value>The diff.</value>
        public string Diff { get; private set; }
        /// <summary>
        /// The user's output.
        /// </summary>
        /// <value>The submission.</value>
        public string Submission { get; private set; }
        /// <summary>
        /// The WebGrader's "correct" output.
        /// </summary>
        /// <value>The solution.</value>
        public string Solution { get; private set; }
        /// <summary>
        /// The output of stderr. If there is an error, the diff will be empty,
        /// but that test should still be considered failed.
        /// </summary>
        /// <value>The error.</value>
        public string Error { get; private set; }
        /// <summary>
        /// The original maze input.
        /// </summary>
        /// <value>The maze.</value>
        public string Maze { get; private set; }
        /// <summary>
        /// The moves inputted to the player(s).
        /// </summary>
        /// <value>The moves.</value>
        public string Moves { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFParser.DiffResult"/> struct.
        /// </summary>
        /// <param name="isEmpty">If set to <c>true</c> is empty.</param>
        /// <param name="sectionNumber">Section number.</param>
        /// <param name="testNumber">Test number.</param>
        /// <param name="diff">Diff.</param>
        /// <param name="submission">Submission.</param>
        /// <param name="solution">Solution.</param>
        /// <param name="error">Error.</param>
        /// <param name="maze">Maze.</param>
        /// <param name="moves">Moves.</param>
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
