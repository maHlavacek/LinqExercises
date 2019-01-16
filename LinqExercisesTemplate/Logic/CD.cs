namespace Logic
{
    public class Cd
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Country { get; set; }
        public string Company { get; set; }
        public double Price { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            return $"{Title,-25} {Artist,-15} {Country,-10} {Company,-10} {Price,6} {Year,5}";
        }
    }
}