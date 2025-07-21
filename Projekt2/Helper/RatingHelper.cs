using System.Text;

namespace Projekt2.Helper
{
    public static class RatingHelper
    {
        public static string GenerateStars(double? rating)
        {
            var fullStars = Math.Floor(rating ?? 0);
            var halfStars = Math.Round(rating ?? 0) > fullStars ? 1 : 0;
            var emptyStars = 5 - fullStars - halfStars;

            var result = new StringBuilder();
            for (int i = 0; i < fullStars; i++)
            {
                result.Append("<i class='fas fa-star text-warning'></i>");
            }
            for (int i = 0; i < halfStars; i++)
            {
                result.Append("<i class='fas fa-star-half-alt text-warning'></i>");
            }
            for (int i = 0; i < emptyStars; i++)
            {
                result.Append("<i class='far fa-star text-warning'></i>");
            }

            return result.ToString();
        }
    }
}
