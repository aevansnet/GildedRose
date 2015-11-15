using Xunit;
using GildedRose.Console;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Tests
{

    //    - Once the sell by date has passed, Quality degrades twice as fast
    //- The Quality of an item is never negative
    //- "Aged Brie" actually increases in Quality the older it gets
    //- The Quality of an item is never more than 50
    //- "Sulfuras", being a legendary item, never has to be sold or decreases
    //in Quality
    //- "Backstage passes", like aged brie, increases in Quality as it's SellIn 
    //value approaches; Quality increases by 2 when there are 10 days or less
    //and by 3 when there are 5 days or less but Quality drops to 0 after the
    //concert

    public class TestAssemblyTests
    {
        Program _p;
        public TestAssemblyTests()
        {
            _p = new Program();
        }

        [Fact]
        public void TestTheTruth()
        {
            Assert.True(true);
        }

        [Fact]
        public void AgedBrieTestIncreasesInQuality()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 };
            UpdateQuality(item);
            Assert.True(item.Quality == 1 && item.SellIn == 1);
        }

        [Fact]
        public void AgedBrieTestNoMoreThan50()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 2, Quality = 48 };
            UpdateQuality(item);
            UpdateQuality(item);
            UpdateQuality(item);
            Assert.True(item.Quality == 50);
        }

        [Fact]
        public void SulfurasTestUnchanged()
        {
            var item = new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 };
            UpdateQuality(item);
            Assert.True(item.Quality == 80 && item.SellIn == 0);
        }

        [Fact]
        public void BackstagePassTestDegradationBands()
        {
            bool result = true;
            var item =  new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 15,
                    Quality = 20
                };
            UpdateQuality(item);
            result = item.Quality == 21;
            UpdateQuality(item); // 22
            UpdateQuality(item); // 23
            UpdateQuality(item); // 24
            UpdateQuality(item); // 25
            UpdateQuality(item); // 27
            UpdateQuality(item); // 29
            UpdateQuality(item); // 31
            result = item.Quality == 31;
            UpdateQuality(item); // 33
            UpdateQuality(item); // 35
            UpdateQuality(item); // 38
            UpdateQuality(item); // 41
            UpdateQuality(item); // 44
            UpdateQuality(item); // 47
            UpdateQuality(item); // 50
            result = item.Quality == 50;
            UpdateQuality(item); // 0
            result = item.Quality == 0;
            Assert.True(result);
        }

        [Fact]
        public void ConjuredTestDoubleDegradation()
        {
            var item = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 7 };
            UpdateQuality(item);
            Assert.True(item.Quality == 5 && item.SellIn == 2);
            UpdateQuality(item);
            Assert.True(item.Quality == 3 && item.SellIn == 1);
            UpdateQuality(item);
            Assert.True(item.Quality == 1 && item.SellIn == 0);
            UpdateQuality(item);
            Assert.True(item.Quality == 0 && item.SellIn == -1);
        }

        void UpdateQuality(Item i)
        {
            _p.UpdateQuality(new List<Item>() { i });
        }
    }
}