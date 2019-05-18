using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Interview.ServiceInterface.Scraping
{
    public interface IDataScraper
    {
        Task<IDocument> GetDocument(string url);
        int CountLinks(IDocument doc);
        int CountImages(IDocument doc);
        int CountWords(IDocument doc);
        string GetTitle(IDocument doc);
    }
}
