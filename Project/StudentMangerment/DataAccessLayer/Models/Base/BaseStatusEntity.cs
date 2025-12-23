namespace DataAccessLayer.Models.Base
{
    public abstract class BaseStatusEntity : BaseEntity
    {
        public bool? IsStatus { get; set; } = true;
    }

}
