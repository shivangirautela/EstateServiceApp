using System.Collections.Generic;

namespace EstateServiceApp.Models

{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
    public static class ConnectedUsers
    {
        public static List<string> Ids = new List<string>();
    }
}