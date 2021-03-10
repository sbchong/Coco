namespace Coco.Server.Hosting.Values
{
    public static class MessagePersistenceType
    {
        public static string None { get; } = "none";
        public static string LocalFile { get; } = "file";
        public static string DataBase { get; } = "database";
    }
}
