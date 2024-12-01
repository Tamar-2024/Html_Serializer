using Htmlserializer;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

var html = await Load("https://www.w3schools.com/");

// סריאליזציה של HTML לעץ אלמנטים
static HtmlElement HtmiSerializer(string html)                                           
{
    var cleanHtml = new Regex("\\s").Replace(html, " ");
    var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);     
    htmlLines = htmlLines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList();

    List<string> allTags = HtmlHelper.Instance.HtmlTags;
    List<string> voidTags = HtmlHelper.Instance.HtmlVoidTags;

    HtmlElement root = new HtmlElement() { };
    HtmlElement currentElement = root;

    foreach (var line in htmlLines)
    {
        string firstWord = line.Split(" ")[0];
        if (firstWord == "html")
        {
            currentElement.Name = firstWord;
            var myAttributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
            foreach (Match attribute in myAttributes)
            {
                string attributeName = attribute.Groups[1].Value;
                string attributeValue = attribute.Groups[2].Value;
                if (attributeName == "class")
                {
                    currentElement.Classes = attributeValue.Split(' ').ToList();
                }
                else if (attributeName == "id")
                {
                    currentElement.Id = attributeValue;
                }
                else
                {
                    currentElement.Attributes.Add(attribute.Name + " = " + attribute.Value);
                }
            }
        }
        else if (firstWord == "html/")
        {
            Console.WriteLine("we finish ");
        }
        else if (firstWord.StartsWith("/"))
        {
            if (currentElement.Parent != null)
            {
                currentElement = currentElement.Parent;
            }
        }
        else if (allTags.Contains(firstWord))
        {
            HtmlElement newElement = new HtmlElement();
            newElement.Parent = currentElement;
            currentElement.Children.Add(newElement);
            newElement.Name = firstWord;
            var myAttributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
            foreach (Match attribute in myAttributes)
            {
                string attributeName = attribute.Groups[1].Value;
                string attributeValue = attribute.Groups[2].Value;
                if (attributeName == "class")
                {
                    newElement.Classes = attributeValue.Split(' ').ToList();
                }
                else if (attributeName == "id")
                {
                    newElement.Id = attributeValue;
                }
                else
                {
                    newElement.Attributes.Add(attribute.Name + " = " + attribute.Value);
                }
            }
            if (!(line.EndsWith("/") || voidTags.Contains(firstWord)))
            {
                currentElement = newElement;
            }
        }
        else
        {
            currentElement.InnerHtml = line;
        }
    }
    return root;
}

HtmlElement htmlTree = HtmiSerializer(html);

// טען תוכן HTML מ-URL
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}


Selector sel = new Selector();
string search = "script";
//string search = "div .md-search__icon";
//string search = "div.class-name";
//string search = ".md-typeset";
//string search = ".md-header__topic";


Selector rooti = Selector.selectorElement(search);
List<HtmlElement> result = htmlTree.func1(rooti);
Console.WriteLine("number = " + result.Count);

// שיטות הרחבה לחיפוש רכיבי HTML
public static class exmentionMetod
{
    // מצא אלמנטים התואמים ל - selector
    public static List<HtmlElement> func1(this HtmlElement element, Selector selector)
    {
        HashSet<HtmlElement> matcheSet = new HashSet<HtmlElement>();
        func2(element, selector, matcheSet);
        return matcheSet.ToList();
    }

    // פונקציה רקורסיבית למציאת אלמנטים תואמים
    public static void func2(HtmlElement currentElement, Selector selector, HashSet<HtmlElement> set)
    {
        IEnumerable<HtmlElement> children = currentElement.Descendants();
        List<HtmlElement> listOfMatches = new List<HtmlElement>();
        foreach (var child in children)
        {
            if (selector.Equals(child))
            {
                listOfMatches.Add(child);
            }
        }
        if (selector.Child.Child == null)
        {
            set.UnionWith(listOfMatches);
            return;
        }
        else
        {
            foreach (var match in listOfMatches)
            {
                func2(match, selector.Child, set);
            }
        }
    }
}