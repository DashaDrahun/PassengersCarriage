using System.Collections.Generic;
using Lab06.MVC.Carriage.DAL.Interfaces;

namespace Lab06.MVC.Carriage.BL.Interfaces
{
    public interface IWrapMapper<M, E> 
        where M: IModel
        where E: IEntity
    {
        M MapModel(E sourceEntity);
        IEnumerable<M> MapCollectionModels(IEnumerable<E> sourceEntities);
        E MapEntity(M sourceModel);
    }
}
