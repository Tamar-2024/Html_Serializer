using Htmlserializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Htmlserializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector()
        {
            TagName = null;
            Id = null;
            Classes = new List<string>();
            Child = null;
            Parent = null;
        }

        // מנתח את מחרוזת ה - selector לעץ selector
        public static Selector selectorElement(string select)                
        {
            List<string> levels = select.Split(' ').ToList();
            Selector rootSelector = new Selector();
            Selector currentSelector = rootSelector;
            List<string> allTags = HtmlHelper.Instance.HtmlTags;
            foreach (var level in levels)
            {
                string[] filters = Regex.Split(level, @"(?=[#.])").Where(s => s.Length > 0).ToArray();
                List<string> classes = new List<string>();
                string id = null;
                string tagName = null;
                foreach (var filter in filters)
                    {
                    if (filter.StartsWith("#"))
                    {
                        currentSelector.Id = filter.Substring(1);
                    }
                    else if (filter.StartsWith("."))
                    {
                        currentSelector.Classes.Add(filter.Substring(1));
                    }
                    else if (allTags.Contains(filter))
                    {
                        currentSelector.TagName = filter;
                    }
                    else
                    {
                        Console.WriteLine("eror the filter not right ");
                    }
                    Selector newSelector = new Selector();
                    newSelector.Parent = currentSelector; 
                    currentSelector.Child = newSelector;
                    currentSelector = newSelector;
                }
            }
            return rootSelector;
        }

        // בודק אם ה - selector הנוכחי תואם ל - HtmlElement
        public override bool Equals(object obj)
        {
            if (obj is HtmlElement)
            {
                HtmlElement element = (HtmlElement)obj;
                if ((this.Id == null || this.Id != null && this.Id.Equals(element.Id)) && (this.TagName == null || this.TagName != null && this.TagName.Equals(element.Name)))
                {
                    if (this.Classes.Count == 0)
                        return true;
                    else
                    {
                        foreach (var c in this.Classes)
                        {
                            if (!element.Classes.Contains(c))
                            {
                                return false;
                            }
                            return true;
                        }
                    }
                }
                return false;
            }
            return false;
        }
    }
}