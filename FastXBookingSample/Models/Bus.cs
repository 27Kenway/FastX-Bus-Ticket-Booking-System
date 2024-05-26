using System;
using System.Collections.Generic;

namespace FastXBookingSample.Models
{
    public partial class Bus
    {
        public Bus()
        {
            BoardingPoints = new HashSet<BoardingPoint>();
            Bookings = new HashSet<Booking>();
            BusAmenities = new HashSet<BusAmenity>();
            BusDepartures = new HashSet<BusDeparture>();
            BusSeats = new HashSet<BusSeat>();
            DroppingPoints = new HashSet<DroppingPoint>();
            Routes = new HashSet<Route>();
        }

        public int BusId { get; set; }
        public string BusName { get; set; } = null!;
        public string BusNumber { get; set; } = null!;
        public string BusType { get; set; } = null!;
        public int NoOfSeats { get; set; }
        public string Origin { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Fare { get; set; }
        public int? BusOperator { get; set; }

        public virtual ICollection<BoardingPoint> BoardingPoints { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<BusAmenity> BusAmenities { get; set; }
        public virtual ICollection<BusDeparture> BusDepartures { get; set; }
        public virtual ICollection<BusSeat> BusSeats { get; set; }
        public virtual ICollection<DroppingPoint> DroppingPoints { get; set; }
        public virtual ICollection<Route> Routes { get; set; }
    }
}
