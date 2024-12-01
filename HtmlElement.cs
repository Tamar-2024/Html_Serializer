using Htmlserializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Htmlserializer
{
    public class HtmlElement
    {
        // מאפיינים של אלמנט HTML
        public string Name { get; set; }
        public string Id { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        // אתחול ברירת מחדל של המאפיינים
        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Parent = null;
            Children = new List<HtmlElement>();
        }

        // מתודה לקבלת כל הצאצאים של האלמנט
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                HtmlElement currentElement = queue.Dequeue();
                foreach (var child in currentElement.Children)
                {
                    queue.Enqueue(child);
                }
                yield return currentElement;
            }
        }

        // מתודה לקבלת כל האבות של האלמנט
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement child = this;
            while (child.Parent != null)
            {
                yield return child.Parent;
                child = child.Parent;
            }
        }
    }
}
