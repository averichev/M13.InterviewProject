namespace M13.InterviewProject.Models.Implementation
{
    public struct SpellerErrors : ISpellCheckError
    {
        public int Code { get; set; }
        public int Pos { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int Len { get; set; }
        public string Word { get; set; }
        public string[] S { get; set; }
    }
}
