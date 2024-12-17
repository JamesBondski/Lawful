namespace LawfulMod.API
{
    public record ReferenceDto(string Type, string Name, string[] possibleValues);

    public record LawDto(int Id, string Title, string Description, string Creator, String State, string Settlement, string? HostObject);

    public record LawSectionDto(int LawId, int Index, string Title, string Description, string UserDescription, bool CanStore);

    public record SectionDto(int Id, string Title, string Description, string UserDescription, bool CanImport, bool CanDelete);
}