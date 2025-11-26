namespace BusinessLogicLayer.DTOs
{
    public class UpdateGradeComponentDto
    {
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Hệ số (0 - 1)
        /// </summary>
        public double Weight { get; set; }
    }

}
