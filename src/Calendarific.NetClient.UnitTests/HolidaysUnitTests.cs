using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calendarific.NetClient.UnitTests
{
    [TestClass]
    public class HolidaysUnitTests
    {
        [Ignore]
        [TestMethod]
        public async Task ShouldGetHolidays()
        {
            var sut = new CalendarificApi(new CalendarificConfig { ApiKey = "" });
            var holidays = await sut.HolidaysAsync(2020, "IE");
            Assert.IsNotNull(holidays);
        }
    }
}
