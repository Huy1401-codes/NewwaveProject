namespace BusinessLogicLayer.DTOs
{
    public class ClassStatisticsDto
    {
        public int TotalStudents { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public double PassRate { get; set; }
        public double FailRate { get; set; }

        public double AverageClassScore { get; set; }
        public double MaxScore { get; set; }
        public double MinScore { get; set; }

        public Dictionary<int, int> ScoreHistogram { get; set; }
            = new Dictionary<int, int>();
    }

}
