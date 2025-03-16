namespace WebServerPrograming.Extensions
{
    public static class UserCartStorage
    {
        // Dictionary to store user-specific carts to empty it after logout
        public static Dictionary<int, List<int>> UserCarts = new Dictionary<int, List<int>>();
    }
}
