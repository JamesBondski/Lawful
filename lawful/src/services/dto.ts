export interface LawDto {
    Id: number;
    Title: string;
    Description: string;
    Creator: string;
    State: string;
    Settlement: string;
    HostObject: string;
}

export interface SectionDto {
    LawId: number;
    Index: number;
    Title: string;
    Description: string;
    UserDescription: string;
    CanStore: boolean; // Added CanStore field
}

export interface StoredSectionDto {
    Id: number;
    Title: string;
    Description: string;
    UserDescription: string;
    CanImport: boolean; // Added CanImport field
    CanDelete: boolean; // Added CanDelete field
}

export interface ReferenceDto {
    Type: string;
    Name: string;
    PossibleValues: string[];
    MappedName?: string;
}

export interface UserDto {
    IsAdmin: boolean; // Indicates if the user is an admin
    Settlements: string[]; // Array of settlements the user is associated with
}