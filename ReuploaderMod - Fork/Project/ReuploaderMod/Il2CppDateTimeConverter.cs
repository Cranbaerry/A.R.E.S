namespace ReuploaderMod
{
    public static class Il2CppDateTimeConverter
    {
        public static System.DateTime ToManagedDateTime(this Il2CppSystem.DateTime unmanagedDateTime)
        {
            return new System.DateTime(unmanagedDateTime.Ticks);
        }
    }
}