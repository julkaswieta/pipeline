namespace Pipeline
{
    public class TestService
    {
        /**Checks if an integer is greater than 0,
         * If int > 1 then returns a string ending in 's'
         * Else returns a string without 's'
         * 
         * @param count integer to check
         */
        
        public string PluralChecker(int count)
        {
            if (count == 1)
                return $"Clicked {count} time";
            else
                return $"Clicked {count} times";
        }

        // Returns a list of all books from bookdb
        public List<Book> GetBooks()
        {
            using (var context = new BookDb())
            {
                return context.Books.ToList();
            }

        }
            
    }
}
