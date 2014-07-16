namespace ExpressionTrees
{
    class Book
    {
        public Book(string code, string title, string author, int numerOfPages)
        {
            Code = code;
            Title = title;
            Author = author;
            NumerOfPages = numerOfPages;
        }

        public Book(string code, string title, string subtitle, string author, int numerOfPages)
        {
            Code = code;
            Title = title;
            Subtitle = subtitle;
            Author = author;
            NumerOfPages = numerOfPages;
        }

        public string Code { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string FullTitle
        {
            get { return Subtitle == null ? Title : string.Format("{0}: {1}", Title, Subtitle); }
        }
        public string Author { get; set; }
        public int NumerOfPages { get; set; }
    }
}