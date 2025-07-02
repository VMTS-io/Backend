using System.Reflection;
using System.Runtime.Serialization;
using VMTS.API.Dtos.Enum;

public static class EnumHelper
{
    public static List<EnumResponseDto> GetEnumValues<T>()
        where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e =>
            {
                var memberInfo = typeof(T).GetMember(e.ToString()).FirstOrDefault();
                var enumMemberAttr = memberInfo?.GetCustomAttribute<EnumMemberAttribute>();
                return new EnumResponseDto
                {
                    Name = e.ToString(),
                    Value = enumMemberAttr?.Value ?? e.ToString(),
                };
            })
            .ToList();
    }
}

