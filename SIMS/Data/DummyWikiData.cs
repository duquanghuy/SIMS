using SIMS.Models;

namespace SIMS.Data
{
    public static class DummyWikiData
    {
        /// <summary>
        /// Simulates an API call returning 100 items.
        /// </summary>
        public static List<WikiItem> GetAll()
        {
            return Enumerable
              .Range(1, 100)
              .Select(i => new WikiItem
              {
                  Id = i,
                  Name = $"Component {i}",
                  Description = $"This is the description for Component {i}."
              })
              .ToList();
        }
    }
}