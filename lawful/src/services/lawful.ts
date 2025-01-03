// lawful/src/api.ts
import axios from 'axios';
import { ref } from 'vue';
import type { LawDto, ReferenceDto, SectionDto, StoredSectionDto, UserDto } from './dto';

const getHeadersFromStorage: () =>
    | {}
    | {
    'X-Auth-Token': string;
} = () => {
    const rawStorage = localStorage.getItem('worldTicketData');
    if (rawStorage === '' || rawStorage === null) {
        return {};
    }

    try {
        const worldTicketData: { worldTicket: string; expiration: number } = JSON.parse(rawStorage)
        if (worldTicketData.worldTicket) {
            return {
                'X-Auth-Token': worldTicketData.worldTicket,
            };
        }
        return {};
    } catch (e) {
        return {};
    }
};

export const fetchUser = async (): Promise<UserDto | null> => {
    // Fetch user data
    const userResponse = await axios.get('/api/v1/lawful/user', { headers: getHeadersFromStorage() });
    if (userResponse.status === 200) {
      const userData: UserDto = userResponse.data;
      return userData;
    }
    return null;
}

export const fetchLaws = async (): Promise<LawDto[]> => {
    const response = await axios.get('/api/v1/lawful/law', { headers: getHeadersFromStorage() });
    return response.data;
}

export const fetchSections = async (id: string | number, adminMode: boolean | null): Promise<SectionDto[]> => {
    const response = await axios.get(`/api/v1/lawful/law/${id}/sections`, { headers: getHeadersFromStorage(), params: { adminMode } });
    return response.data;
}

export const storeSection = async (lawId: number, sectionIndex: number, adminMode: boolean | null): Promise<SectionDto> => {
    const response = await axios.post('/api/v1/lawful/section', null, {
        params: { lawId, sectionIndex, adminMode },
        headers: getHeadersFromStorage()
    });
    return response.data;
}

export const fetchStoredSections = async (lawId: number, adminMode: boolean | null): Promise<StoredSectionDto[]> => {
    const response = await axios.get('/api/v1/lawful/section', {
        params: { selectedLawId: lawId, adminMode },
        headers: getHeadersFromStorage()
    });
    return response.data;
}

export const importSection = async (lawId: number, sectionId: number, references: ReferenceDto[] | null) => {
    // Remove unnecessary properties from ReferenceDto
    const filteredReferences = references?.map(({ Type, Name, MappedName }) => ({ Type, Name, MappedName })) || null; // Exclude PossibleValues

    const response = await axios.post(`/api/v1/lawful/law/${lawId}/sections/import/${sectionId}`, filteredReferences, {
        headers: getHeadersFromStorage()
    });
    return response.data;
}

export const deleteStoredSection = async (sectionId: number, adminMode: boolean | null): Promise<void> => {
    await axios.delete(`/api/v1/lawful/section/${sectionId}`, {
        params: { adminMode },
        headers: getHeadersFromStorage()
    });
}

export const sectionJson = async (sectionId: number): Promise<SectionDto> => {
    const response = await axios.get(`/api/v1/lawful/section/${sectionId}/json`, {
        headers: getHeadersFromStorage()
    });
    return response.data;
}

export const fetchReferences = async (sectionId: number): Promise<ReferenceDto[]> => {
    const response = await axios.get(`/api/v1/lawful/section/${sectionId}/references`, { headers: getHeadersFromStorage() });
    return response.data; // Ensure this matches the ReferenceDto structure
}
