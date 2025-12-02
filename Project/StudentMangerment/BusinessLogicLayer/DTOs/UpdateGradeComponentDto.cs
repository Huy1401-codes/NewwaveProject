namespace BusinessLogicLayer.DTOs
{
    public class UpdateGradeComponentDto
    {
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Hệ số 
        /// </summary>
        public double Weight { get; set; }
    }

}
