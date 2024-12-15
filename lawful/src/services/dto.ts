
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
    possibleValues: string[];
}