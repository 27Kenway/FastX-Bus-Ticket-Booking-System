using FastXBookingSample.Interface;
using FastXBookingSample.Models;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastXBookingTest
{
    [TestFixture]
    public class BusSeatRepoTest
    {
        private Mock<IBusSeatRepository> _busSeat;

        [SetUp]
        public void Setup()
        {
            _busSeat = new Mock<IBusSeatRepository>();
        }


      


        [Test]
        public async Task TestForGetAllBoardingbyBusId()
        {
            _busSeat.Setup(x => x.GetSeatsByBusId(It.IsAny<int>(), It.IsAny<DateTime>())).Returns(new List<BusSeat>()
            {
                new BusSeat()
                {
                   IsBooked = true,
                   DepartureId=2,
                   SeatNo = 1,
                },
                 new BusSeat()
                {
                   IsBooked = false,
                   SeatNo = 2,
                   DepartureId=3,
                },
            });
            List<BusSeat> output = await Task.Run(() => _busSeat.Object.GetSeatsByBusId(2,new DateTime()));
            Assert.AreEqual(output[0].IsBooked, true);
            Assert.AreEqual(output[0].SeatNo, 1);
            Assert.AreEqual(output[1].IsBooked, false);
            Assert.AreEqual(output[1].SeatNo, 2);
        }



    }
}
