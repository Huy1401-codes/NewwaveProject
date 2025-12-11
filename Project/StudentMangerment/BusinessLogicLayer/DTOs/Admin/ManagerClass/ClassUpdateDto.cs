using BusinessLogicLayer.Enums.Admin;

namespace BusinessLogicLayer.DTOs.Admin.ManagerClass
{
    public class ClassUpdateDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public ClassStatus IsStatus { get; set; }

        public int SubjectId { get; set; }
        public int SemesterId { get; set; }
        public int TeacherId { get; set; }

        public List<int> StudentIds { get; set; } = new();
    }

}
