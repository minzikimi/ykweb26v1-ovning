namespace modell.Requests
{
    public struct CreateGradeRequest
    {
        public int StudentId { get; set; }
        public int CourseInstanceId { get; set; }
        public string Grade { get; set; }
    }
}