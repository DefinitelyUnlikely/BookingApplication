using System.Threading.Tasks;
using BookingApplication.Interfaces;

namespace BookingApplication.Services;

public abstract class EfService<T, TAddRequest, TEditRequest>
    : IService<T, TAddRequest, TEditRequest>
    where T : class
    where TAddRequest : IRequest
    where TEditRequest : IRequest
{
    protected readonly IRepository<T> repository;

    public EfService(IRepository<T> repository)
    {
        this.repository = repository;
    }

    public abstract Task<T> CreateFromRequestAsync(TAddRequest request);

    /*  Adding from request must be abstract since there is no smooth way to make this generic for any model i think
    
        Possible to remove generic type <TAddRequest> and have IRequest as a parameter instead
        but that needs a type check in each service:

        public abstract Task<T> AddFromRequestAsync(IRequest request){
            if (request is RegisterRequest registerRequest)
            {
                ....
            }
        }
    */

    public abstract Task<T> EditFromRequestAsync(TEditRequest request);


    /*  Same here as with AddFromRequest*/

    public async Task DeleteAsync(Guid entityId)
    {
        var entity = await GetByIdAsync(entityId);

        if (entity == null)
        {
            return;
        }

        // await repository.DeleteAsync(entity);
        await repository.DeleteAsync(entity);
    }

    public async Task DeleteAsync(T entityToRemove)
    {
        await repository.DeleteAsync(entityToRemove);
        //await repository.DeleteAsync(entityToRemove);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task EditAsync(T entityToUpdate)
    {
        await repository.EditAsync(entityToUpdate);
    }

    public async Task<T?> GetByIdAsync(Guid entityId)
    {
        return await repository.GetByIdAsync(Guid.Parse(entityId.ToString()!));
    }
}
