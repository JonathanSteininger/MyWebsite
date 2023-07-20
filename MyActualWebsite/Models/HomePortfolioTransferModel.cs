using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Generic;

namespace MyActualWebsite.Models
{
    public class HomePortfolioTransferModel
    {
        public List<Project> Projects { get; set; } = new();
        public Dictionary<int, TagCheckBoxStorage> TagsSelection { get; set; } = new();

        static string LANGUAGE = "Language";
        static string PLATFORM = "Platform";
        static string FRAMEWORK = "Framework";
        static string OTHER = "Other";

        public List<KeyValuePair<int, TagCheckBoxStorage>> LanguageTags => GetList(LANGUAGE);
        public List<KeyValuePair<int, TagCheckBoxStorage>> PlatformTags => GetList(PLATFORM);
        public List<KeyValuePair<int, TagCheckBoxStorage>> FrameworkTags => GetList(FRAMEWORK);
        public List<KeyValuePair<int, TagCheckBoxStorage>> OtherTags => GetList(OTHER);

        public bool ContainsLanguageTags => ContainsTag(LANGUAGE);
        public bool ContainsPlatformTags => ContainsTag(PLATFORM);
        public bool ContainsFrameWorkTags => ContainsTag(FRAMEWORK);
        public bool ContainsOtherTags => ContainsTag(OTHER);

        private bool ContainsTag(string tagCatagory)
        {
            string catagoryUpper = tagCatagory.ToUpper();
            foreach(KeyValuePair<int, TagCheckBoxStorage> kvp in TagsSelection)
            {
                if (kvp.Value == null || kvp.Value.Tag == null || kvp.Value.Tag.TagCatagory == null)
                    continue;
                if (kvp.Value.Tag.TagCatagory.CatagoryName.ToUpper().Contains(catagoryUpper))
                    return true;
            }
            return false;

        }

        private List<KeyValuePair<int, TagCheckBoxStorage>> GetList(string Catagory)
        {
            string CatagoryUP = Catagory.ToUpper();
            List<KeyValuePair<int, TagCheckBoxStorage>> temp =  new List<KeyValuePair<int, TagCheckBoxStorage>>(TagsSelection.Where(item => item.Value.Tag != null && item.Value.Tag.TagCatagory.CatagoryName.ToUpper() == CatagoryUP));
            SortList(temp);
            return temp;
        }

        private void SortList(List<KeyValuePair<int, TagCheckBoxStorage>> listToSort)
        {
            listToSort.Sort((a, b) =>
            {
                if (b.Value.IsChecked.CompareTo(a.Value.IsChecked) == 0)
                {
                    return a.Key.CompareTo(b.Key);
                }
                return b.Value.IsChecked.CompareTo(a.Value.IsChecked);
            });
        }

    }
    public class TagCheckBoxStorage
    {
        public Tag Tag { get; set; }
        public bool IsChecked { get; set; }
    }
}
