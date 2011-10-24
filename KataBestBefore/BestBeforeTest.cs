using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace KataBestBefore
{
    [TestFixture]
    public class BestBeforeTest
    {
        [Test]
        public void EmptyDateIsInvalid()
        {
            string tag = "";

            Assert.AreEqual(string.Empty, BestBefore.Translate(tag));
        }

        [Test]
        public void SameNumberTest()
        {
            string tag = "03/03/03";

            Assert.AreEqual("2003/03/03", BestBefore.Translate(tag));
        }

        [Test]
        public void DecreasingNumbersTest()
        {
            string tag = "12/10/03";

            Assert.AreEqual("2003/10/12", BestBefore.Translate(tag));
        }

        [Test]
        public void IncreasingNumbersTest()
        {
            string tag = "03/10/12";

            Assert.AreEqual("2003/10/12", BestBefore.Translate(tag));
        }

        [Test]
        public void RandomTagTest()
        {
            string tag = "12/22/11";

            Assert.AreEqual("2011/12/22", BestBefore.Translate(tag));
        }

        [Test]
        public void TestWithYearOnlyNumber()
        {
            string tag = "01/01/45";

            Assert.AreEqual("2045/01/01", BestBefore.Translate(tag));
        }

        [Test]
        public void TestWithYearOrDayOnlyNumber()
        {
            string tag = "01/01/30";

            Assert.AreEqual("2001/01/30", BestBefore.Translate(tag));
        }

        [Test]
        public void TestWithAYearOnlyNumberAndADayOnlyNumber()
        {
            string tag = "01/45/31";

            Assert.AreEqual("2045/01/31", BestBefore.Translate(tag));
        }
    }

    public static class BestBefore
    {
        public static string Translate(string tag)
        {
            var separator = '/';
            if (tag  == string.Empty)
                return "";
            var splitNumbers = tag.Split(separator);
            var orderedNumbers = splitNumbers.OrderBy(n => int.Parse(n))
                                             .ToList();
            List<string> rightOrderNumbers = OrderNumbersYearwise(orderedNumbers);

            return "20" + rightOrderNumbers[0] + separator + rightOrderNumbers[1] + separator + rightOrderNumbers[2];
        }

        private static List<string> OrderNumbersYearwise(List<string> numbers)
        {
            var yearOrderedNumbers = new List<string>();
            foreach (var number in numbers)
            {
                if (int.Parse(number) > 31) {
                    yearOrderedNumbers.Add(number);
                }
            }
            yearOrderedNumbers.AddRange(numbers.Where(n => int.Parse(n) <= 31));

            var monthOrderedNumbers = new List<string>();

            if (numbers.Any((n => int.Parse(n) > 31)))
            {
                foreach (var number in yearOrderedNumbers)
                {
                    if (int.Parse(number) > 12)
                        monthOrderedNumbers.Add(number);
                }
            }

            monthOrderedNumbers.AddRange(numbers.Where(n => int.Parse(n) <= 12));

            return monthOrderedNumbers;
        }
    }
}
