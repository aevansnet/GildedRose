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
            Assert.Equal(item.Quality, 1);
            Assert.Equal(item.SellIn, 1);
        }

        [Fact]
        public void AgedBrieTestNoMoreThan50()
        {
            var item = new Item { Name = "Aged Brie", SellIn = 2, Quality = 48 };
            UpdateQuality(item);
            UpdateQuality(item);
            UpdateQuality(item);
            Assert.Equal(item.Quality, 50);
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
            var item =  new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 15,
                    Quality = 21
                };
            UpdateQuality(item);
            Assert.Equal(item.Quality, 22);
            UpdateQuality(item); // 23
            UpdateQuality(item); // 24
            UpdateQuality(item); // 25
            UpdateQuality(item); // 26
            UpdateQuality(item); // 28
            UpdateQuality(item); // 30
            UpdateQuality(item); // 32
            Assert.Equal(item.Quality, 32);
            UpdateQuality(item); // 34
            UpdateQuality(item); // 36
            UpdateQuality(item); // 39
            UpdateQuality(item); // 42
            Assert.Equal(item.Quality, 42);
            UpdateQuality(item); // 45
            UpdateQuality(item); // 48
            UpdateQuality(item); // 50
            Assert.Equal(item.Quality, 50); // should be no more than 50!
            UpdateQuality(item); // 0
            Assert.Equal(item.Quality, 0); // quality drops to zero now date has passed
        }

        [Fact]
        public void ConjuredTestDoubleDegradation()
        {
            var item = new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 11};
            UpdateQuality(item);
            Assert.Equal(item.Quality, 9);
            Assert.Equal(item.SellIn, 2);
            UpdateQuality(item);
            Assert.Equal(item.Quality, 7);
            Assert.Equal(item.SellIn, 1);
            UpdateQuality(item);
            Assert.Equal(item.Quality, 5);
            Assert.Equal(item.SellIn, 0);                 
            UpdateQuality(item);
            Assert.Equal(item.Quality, 3);
            Assert.Equal(item.SellIn, -1);          
            // double degradation, but no negative qualitys         
            UpdateQuality(item);
            Assert.Equal(item.Quality, 0);
            Assert.Equal(item.SellIn, -2);
        }

        [Fact]
        public void StandardItemTestDegradation()
        {
            var item = new Item { Name = "Chicken", SellIn = 2, Quality = 5 };
            UpdateQuality(item);
            Assert.Equal(item.Quality, 4);
            Assert.Equal(item.SellIn, 1);
            UpdateQuality(item);
            Assert.Equal(item.Quality, 3);
            Assert.Equal(item.SellIn, 0);
            UpdateQuality(item);
            Assert.Equal(item.Quality, 2);
            Assert.Equal(item.SellIn, -1);
            // now double degradation comes into play because of negative sell in.
            UpdateQuality(item);
            Assert.Equal(item.Quality, 0);
            Assert.Equal(item.SellIn, -2);        
            UpdateQuality(item);
            Assert.Equal(item.Quality, 0);  // quality should not fall bellow 0
            Assert.Equal(item.SellIn, -3);

        }

        void UpdateQuality(Item i)
        {
            _p.UpdateQuality(new List<Item>() { i });
        }
        
    }
}