using FastXBookingSample.Models;

namespace FastXBookingSample.Interface
{
    public interface IBusSeatRepository
    {
        List<BusSeat> GetSeatsByBusId(int busid,DateTime deptId);
        void AddSeatByBusId(int busid, int seats,int deptId);
        void DeleteSeatsByBusId(int busid);
    }
}
