using System;
using System.Collections.Generic;

namespace FastXBookingSample.Models
{
    public partial class BusDeparture
    {
        public BusDeparture()
        {
            Bookings = new HashSet<Booking>();
            BusSeats = new HashSet<BusSeat>();
        }

        public int? BusId { get; set; }
        public DateTime? DepartureDate { get; set; }
        public int Id { get; set; }

        public virtual Bus? Bus { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<BusSeat> BusSeats { get; set; }
    }
}
