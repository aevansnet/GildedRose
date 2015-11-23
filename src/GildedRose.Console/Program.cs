using System;
using System.Collections.Generic;

namespace GildedRose.Console
{
    public class Program
    {
        IList<Item> Items;
        static void Main(string[] args)
        {
            System.Console.WriteLine("OMGHAI!");

            var app = new Program()
                          {
                              Items = new List<Item>
                                          {
                                              new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                              new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                              new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                              new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                              new Item
                                                  {
                                                      Name = "Backstage passes to a TAFKAL80ETC concert",
                                                      SellIn = 15,
                                                      Quality = 20
                                                  },
                                              new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                                          }

                          };

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void UpdateQuality(IList<Item> items = null)
        {
            if (items == null)
                items = Items;

            foreach (var i in items)
            {
                var updater = QualityUpdater.GetUpdaterForItem(i);
                updater.UpdateQuality(i);                
            }           
        }
    }

    public abstract class QualityUpdater
    {
        public static QualityUpdater GetUpdaterForItem(Item i)
        {
            // select the correct update strategy
            if (i.Name == "Aged Brie")
                return new BrieQualityUpdater();
            else if (i.Name == "Sulfuras, Hand of Ragnaros")
                return new LegendaryQualityUpdater();
            else if (i.Name == "Backstage passes to a TAFKAL80ETC concert")
                return new BackstagePassQualityUpdater();
            else if (i.Name.StartsWith("Conjured"))
                return new ConjuredQualityUpdater();
            else
                return new StandardQualityUpdater();
        }


        public void UpdateQuality(Item i)
        {
            var degradation = GetDegradation(i);

            if (degradation != 0)
            {
                var degradationFactor = i.SellIn < 0 ? 2 : 1;
                var newQuality = i.Quality + (degradation * degradationFactor);
                i.Quality = Math.Max(0, Math.Min(newQuality, 50));  //maxium of 50, minimum of 0
            }

            if(!(this is LegendaryQualityUpdater))  // dont like this
                i.SellIn--;
        }

        /// <summary>
        /// Calculate quality degradation after 1 day
        /// </summary>
        /// <param name="i">Item</param>
        /// <returns>degradation amount</returns>
        protected abstract int GetDegradation(Item i);
    }

    public class StandardQualityUpdater : QualityUpdater
    {
        protected override int GetDegradation(Item i)
        {
            return -1;
        }
    }

    public class BrieQualityUpdater : QualityUpdater
    {
        protected override int GetDegradation(Item i)
        {
            return +1;
        }
    }

    public class BackstagePassQualityUpdater : QualityUpdater
    {
        protected override int GetDegradation(Item i)
        {
            if (i.SellIn < 1)
            {
                i.Quality = i.Quality + (i.Quality * -1); //force to zero
                return 0; // we've handled it
            }
            else if (i.SellIn <= 5 && i.Quality < 48)
                return 3;
            else if (i.SellIn <= 10 && i.Quality < 49)
                return 2;
            else
                return 1;
        }
    }

    public class LegendaryQualityUpdater : QualityUpdater
    {
        protected override int GetDegradation(Item i)
        {
            return 0;
        }        
    }

    public class ConjuredQualityUpdater : QualityUpdater
    {
        protected override int GetDegradation(Item i)
        {
            return -2;            
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }
    }

}
