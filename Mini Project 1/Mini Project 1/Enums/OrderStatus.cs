using System.Runtime.Serialization;

namespace Mini_Project_1.Enums
{
    internal enum OrderStatus
    {
        Pending = 1,
        Confirmed,
        Completed,
        Cancelled
    }

    internal enum ProductCategory
    {
        [EnumMember(Value = "Elektronika")]
        Elektronika,

        [EnumMember(Value = "Maşın")]
        Maşın,

        [EnumMember(Value = "Digər")]
        Digər
    }
}