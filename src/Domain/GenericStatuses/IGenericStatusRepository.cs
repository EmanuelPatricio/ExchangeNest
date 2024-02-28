namespace Domain.GenericStatuses;
public interface IGenericStatusRepository
{
    Task<List<GenericStatus>> GetAll();
}
