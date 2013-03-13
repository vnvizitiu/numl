﻿using System;
using numl.Model;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

namespace numl.Tests.DataTests
{
    [TestFixture]
    public class ComplexConversionTests
    {
        private Descriptor Generate()
        {
            Descriptor d = new Descriptor();

            d.Features = new Property[]
            {
                new Property { Name = "Age" },
                new Property { Name = "Height" },
                new DateTimeProperty(DateTimeFeature.DayOfWeek | DateTimeFeature.Month) { Name = "BirthDate"  },
                new Property { Name = "Weight" },
                new Property { Name = "Good" },
                
            };

            return d;
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Dates()
        {
            var d = Generate();

            var o = new 
            { 
                Age = 23, 
                Height = 6.21d, 
                Weight = 220m, 
                Good = false, 
                BirthDate = new DateTime(2012, 11, 7, 2, 3, 4) 
            };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* DATE */
                                        11, 3,
                                        /* END DATE */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(4, d.Features[3].Start);
            Assert.AreEqual(5, d.Features[4].Start);

        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Dates_2()
        {
            Descriptor d = Generate();

            d.Features[2] = new DateTimeProperty(DateTimeFeature.Year | DateTimeFeature.Month | DateTimeFeature.Hour) { Name = "BirthDate" };

            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, BirthDate = new DateTime(2012, 11, 7, 2, 3, 4) };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* DATE */
                                        2012, 11, 2,
                                        /* END DATE */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(5, d.Features[3].Start);
            Assert.AreEqual(6, d.Features[4].Start);
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Dates_3()
        {
            Descriptor d = Generate();

            d.Features[2] = new DateTimeProperty(DatePortion.Date) { Name = "BirthDate" };

            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, BirthDate = new DateTime(2012, 11, 7, 2, 3, 4) };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* DATE */
                                        2012, 11, 7,
                                        /* END DATE */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(5, d.Features[3].Start);
            Assert.AreEqual(6, d.Features[4].Start);
            // proper length
            Assert.AreEqual(3, d.Features[2].Length);
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Dates_4()
        {
            Descriptor d = Generate();

            d.Features[2] = new DateTimeProperty(DatePortion.Time) { Name = "BirthDate" };

            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, BirthDate = new DateTime(2012, 11, 7, 2, 3, 4) };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* DATE */
                                        2, 3,
                                        /* END DATE */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(4, d.Features[3].Start);
            Assert.AreEqual(5, d.Features[4].Start);
            // proper length
            Assert.AreEqual(2, d.Features[2].Length);
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Dates_5()
        {
            Descriptor d = Generate();

            d.Features[2] = new DateTimeProperty(DatePortion.Date | DatePortion.TimeExtended) { Name = "BirthDate" };

            DateTime date = new DateTime(2012, 11, 7, 2, 3, 4, 234);
            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, BirthDate = date };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* DATE */
                                        2012, 11, 7, 4, 234,
                                        /* END DATE */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(7, d.Features[3].Start);
            Assert.AreEqual(8, d.Features[4].Start);
            // proper length
            Assert.AreEqual(5, d.Features[2].Length);
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Dates_6()
        {
            Descriptor d = Generate();

            d.Features[2] = new DateTimeProperty(DatePortion.DateExtended | DatePortion.TimeExtended) { Name = "BirthDate" };

            DateTime date = new DateTime(2012, 11, 7, 2, 3, 4, 234);
            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, BirthDate = date };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* DATE */
                                        date.DayOfYear, (int)date.DayOfWeek, 4, 234,
                                        /* END DATE */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(6, d.Features[3].Start);
            Assert.AreEqual(7, d.Features[4].Start);
            // proper length
            Assert.AreEqual(4, d.Features[2].Length);
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Enumerable_Short()
        {
            Descriptor d = Generate();

            d.Features[2] = new EnumerableProperty(10) { Name = "Stuff" };

            var array = new int[] { 1, 2, 3, 4, 5, 6, 7 };
            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, Stuff = array };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* ARRAY */
                                        1, 2, 3, 4, 5, 6, 7, 0, 0, 0,
                                        /* END ARRAY */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(12, d.Features[3].Start);
            Assert.AreEqual(13, d.Features[4].Start);
            // proper length
            Assert.AreEqual(10, d.Features[2].Length);
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Enumerable_Exact()
        {
            Descriptor d = Generate();

            d.Features[2] = new EnumerableProperty(10) { Name = "Stuff" };

            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, Stuff = array };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* ARRAY */
                                        1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                                        /* END ARRAY */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(12, d.Features[3].Start);
            Assert.AreEqual(13, d.Features[4].Start);
            // proper length
            Assert.AreEqual(10, d.Features[2].Length);
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Enumerable_Long()
        {
            Descriptor d = Generate();

            d.Features[2] = new EnumerableProperty(10) { Name = "Stuff" };

            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, Stuff = array };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* ARRAY */
                                        1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                                        /* END ARRAY */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(12, d.Features[3].Start);
            Assert.AreEqual(13, d.Features[4].Start);
            // proper length
            Assert.AreEqual(10, d.Features[2].Length);
        }

        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Enumerable_Short_By_One()
        {
            Descriptor d = Generate();

            d.Features[2] = new EnumerableProperty(10) { Name = "Stuff" };

            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, Stuff = array };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* ARRAY */
                                        1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
                                        /* END ARRAY */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(12, d.Features[3].Start);
            Assert.AreEqual(13, d.Features[4].Start);
            // proper length
            Assert.AreEqual(10, d.Features[2].Length);
        }


        [Test]
        public void Test_Vector_Conversion_Simple_Numbers_And_Enumerable_Long_By_One()
        {
            Descriptor d = Generate();

            d.Features[2] = new EnumerableProperty(10) { Name = "Stuff" };

            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var o = new { Age = 23, Height = 6.21d, Weight = 220m, Good = false, Stuff = array };

            // array generated by descriptor ordering
            var truths = new double[] { 23, 6.21,  
                                        /* ARRAY */
                                        1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                                        /* END ARRAY */
                                        220, -1 };
            var actual = d.Convert(o);
            Assert.AreEqual(truths, actual);
            // offset test
            Assert.AreEqual(2, d.Features[2].Start);
            Assert.AreEqual(12, d.Features[3].Start);
            Assert.AreEqual(13, d.Features[4].Start);
            // proper length
            Assert.AreEqual(10, d.Features[2].Length);
        }
    }
}
