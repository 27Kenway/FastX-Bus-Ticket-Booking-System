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
    public class BusRepoTest
    {
        private Mock<IBusRepository> _busRepository;

        [SetUp]
        public void Setup()
        {
            _busRepository = new Mock<IBusRepository>();
        }


        [Test]
        public async Task TestForDeleteBus()
        {
            _busRepository.Setup(x => x.DeleteBus(It.IsAny<int>())).Returns("Deleted Successfully");
            string output = await Task.Run(() => _busRepository.Object.DeleteBus(2));
            Assert.AreEqual(output, "Deleted Successfully");
        }


        [Test]
        public async Task TestForGetAll()
        {
            _busRepository.Setup(x => x.GetAll()).Returns(new List<Bus>()
            {
                new Bus()
                {
                   BusName = "Volvo",
                   BusNumber = "TS04TG5478"
                },
               new Bus()
                {
                   BusName = "Super Luxury",
                   BusNumber = "TS08HS2578"
                }
            }
               );
            List<Bus> output = await Task.Run(() => _busRepository.Object.GetAll());
            Assert.AreEqual(output[0].BusName, "Volvo");
            Assert.AreEqual(output[0].BusNumber, "TS04TG5478");
            Assert.AreEqual(output[1].BusName, "Super Luxury");
            Assert.AreEqual(output[1].BusNumber, "TS08HS2578");
        }



        [Test]
        public async Task TestForPatchBus()
        {
            JsonPatchDocument<Bus> jsonpatch = new JsonPatchDocument<Bus>();
            _busRepository.Setup(x => x.PatchBus(It.IsAny<int>(), jsonpatch)).Returns("Updated Successfully");
            string output = await Task.Run(() => _busRepository.Object.PatchBus(It.IsAny<int>(), jsonpatch));
            Assert.AreEqual(output, "Updated Successfully");
        }


        [Test]
        public async Task TestForPostBus()
        {
            Bus bus = new Bus()
            {
                BusName = "Sample"
            };
            _busRepository.Setup(x => x.CreateBus(bus)).Returns(new Bus(){ BusName = "Sample"});
            Bus bus1 = await Task.Run(() => _busRepository.Object.CreateBus(bus));
            Assert.AreEqual(bus.BusName,bus1.BusName);
        }


        [Test]
        public async Task TestForUpdateBoardingPoint()
        {
            Bus bus = new Bus();
            _busRepository.Setup(x => x.UpdateBus(It.IsAny<int>(), bus)).Returns("Updated Successfully");
            string output = await Task.Run(() => _busRepository.Object.UpdateBus(It.IsAny<int>(), bus));
            Assert.AreEqual(output, "Updated Successfully");
        }
    }
}
