namespace Christmas2016.Service.Repositories
{
    using AForge.Imaging;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBlobRepository
    {
        Task<List<Blob>> GetAsync();
    }
}