// lawful/src/api.ts
import axios from 'axios';
import { ref } from 'vue';

const getHeadersFromStorage = () => {
    const worldTicket = localStorage.getItem("worldTicket");
    if (!worldTicket) return {};
    return {
        'X-Auth-Token': worldTicket,
    }
}

export const fetchLaws = async () => {
    const response = await axios.get('/api/v1/lawful/law', { headers: getHeadersFromStorage() });
    return response.data;
}

export const fetchSections = async (id: string | number) => {
    const response = await axios.get(`/api/v1/lawful/law/${id}/sections`, { headers: getHeadersFromStorage() });
    return response.data;
}

export const storeSection = async (lawId: number, sectionIndex: number) => {
    const response = await axios.post('/api/v1/lawful/section', null, {
        params: { lawId, sectionIndex },
        headers: getHeadersFromStorage()
    });
    return response.data;
}

export const fetchStoredSections = async (lawId: number) => {
    const response = await axios.get('/api/v1/lawful/section', {
        params: { selectedLawId: lawId },
        headers: getHeadersFromStorage()
    });
    return response.data;
}

export const importSection = async (selectedLawId: number, sectionId: number) => {
    const response = await axios.post(`/api/v1/lawful/law/${selectedLawId}/sections/import/${sectionId}`, null, {
        headers: getHeadersFromStorage()
    });
    return response.data;
}

export const deleteStoredSection = async (sectionId: number) => {
    await axios.delete(`/api/v1/lawful/section/${sectionId}`, {
        headers: getHeadersFromStorage()
    });
}

export const sectionJson = async (sectionId: number) => {
    const response = await axios.get(`/api/v1/lawful/section/${sectionId}/json`, {
        headers: getHeadersFromStorage()
    });
    return response.data;
}

