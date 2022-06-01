namespace M13.InterviewProject.Models.Implementation
{
    public struct SpellerErrors : ISpellCheckError
    {
        public int Code { get; set; }
        public int Pos { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public int len { get; set; }
        public string Word { get; set; }
        public string[] s { get; set; }
    }
}
