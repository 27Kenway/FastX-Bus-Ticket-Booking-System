using FastXBookingSample.Models;

namespace FastXBookingSample.Interface
{
    public interface IBusDepartureRepository
    {
        BusDeparture AddDepartureDate(BusDeparture busDeparture);
    }
}
