using System;
using System.Collections.Generic;

namespace FastXBookingSample.Models
{
    public partial class Booking
    {
        public Booking()
        {
            BookingHistories = new HashSet<BookingHistory>();
            Seats = new HashSet<Seat>();
        }

        public int BookingId { get; set; }
        public int? UserId { get; set; }
        public int? BusId { get; set; }
        public int? BoardingId { get; set; }
        public int? DroppingId { get; set; }
        public int? DeptId { get; set; }

        public virtual Bus? Bus { get; set; }
        public virtual BusDeparture? Dept { get; set; }
        public virtual DroppingPoint? Dropping { get; set; }
        public virtual BoardingPoint? Boarding { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<BookingHistory> BookingHistories { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
