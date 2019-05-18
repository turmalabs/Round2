using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace Interview.ServiceInterface.Scraping
{
    /// <summary>
    /// Class to get website HTML and return some basic data from it
    /// </summary>
    public class DataScraper : IDataScraper
    {
        private readonly IBrowsingContext _context;

        public DataScraper()
        {
            var config = Configuration.Default.WithDefaultLoader();
            _context = BrowsingContext.New(config);
        }

        /// <summary>
        /// Gets the HTML document that can then be used with other functions to
        /// discover data.
        /// </summary>
        /// <param name="url">Url of page you want to gather data on</param>
        /// <returns></returns>
        public async Task<IDocument> GetDocument(string url)
        {
            return await _context.OpenAsync(url);
        }

        /// <summary>
        /// Get number of links on page
        /// </summary>
        /// <param name="doc">HTML document to gather data on</param>
        /// <returns></returns>
        public int CountLinks(IDocument doc)
        {
            return doc.QuerySelectorAll("a").Length;
        }

        /// <summary>
        /// Get number of images on page
        /// </summary>
        /// <param name="doc">HTML document to gather data on</param>
        /// <returns></returns>
        public int CountImages(IDocument doc)
        {
            return doc.QuerySelectorAll("img").Length;
        }

        /// <summary>
        /// Get number of words on page
        /// </summary>
        /// <param name="doc">HTML document to gather data on</param>
        /// <returns></returns>
        public int CountWords(IDocument doc)
        {
            var text = doc.Body.TextContent;
            int wordCount = 0, index = 0;

            // skip whitespace until first word
            while (index < text.Length && char.IsWhiteSpace(text[index]))
                index++;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
            }

            return wordCount;
        }

        /// <summary>
        /// Get page title
        /// </summary>
        /// <param name="doc">HTML document to gather data on</param>
        /// <returns></returns>
        public string GetTitle(IDocument doc)
        {
            return doc.Title;
        }
    }
}
