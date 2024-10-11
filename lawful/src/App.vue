<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import axios from 'axios'
import { VBtn, VCard, VCardItem, VCardTitle, VCardActions, VAlert } from 'vuetify/components'
import SectionCard from './components/SectionCard.vue'; // Import the new component

interface LawDto {
  Id: number;
  Title: string;
  Description: string;
  Creator: string;
  State: string;
  Settlement: string;
  HostObject: string;
}

interface SectionDto {
  LawId: number;
  Index: number;
  Title: string;
  Description: string;
  UserDescription: string;
  CanStore: boolean; // Added CanStore field
}

interface StoredSectionDto {
  Id: number;
  Title: string;
  Description: string;
  UserDescription: string;
  CanImport: boolean; // Added CanImport field
}

const laws = ref<LawDto[]>([])
const selectedLawId = ref<number | null>(null)
const sections = ref<SectionDto[]>([])
const storedSections = ref<StoredSectionDto[]>([]) // New reactive variable for stored sections

const getHeadersFromStorage = () => {
    const worldTicket = localStorage.getItem("worldTicket");
    if (!worldTicket) return {};
    return {
        'X-Auth-Token': worldTicket,
    }
}

const alertMessage = ref<string | null>(null) // New reactive variable for alert message
const alertType = ref<'success' | 'error' | null>(null) // New reactive variable for alert type

const showAlert = (message: string, type: 'success' | 'error') => {
    alertMessage.value = message;
    alertType.value = type;
    setTimeout(() => {
        alertMessage.value = null; // Clear alert after a timeout
    }, 3000); // Adjust timeout duration as needed
}

onMounted(async () => {
  try {
    const response = await axios.get('/api/v1/lawful/law', { headers: getHeadersFromStorage() }) // Added headers
    laws.value = response.data
    await fetchStoredSections(0); // Fetch stored sections on mount
  } catch {
    showAlert('Error fetching laws', 'error'); // Show error alert
  }
})

const fetchSections = async (id: string | number) => {
  try {
    const response = await axios.get(`/api/v1/lawful/section?LawId=${id}`, { headers: getHeadersFromStorage() }) // Added headers
    sections.value = response.data
  } catch (error) {
    showAlert('Error fetching sections:', 'error');
    sections.value = []
  }
}

const storeSection = async (lawId: number, sectionIndex: number) => {
  try {
    const response = await axios.post('/api/v1/lawful/store', null, {
      params: {
        lawId,
        sectionIndex
      },
      headers: getHeadersFromStorage() // Added headers
    })
    showAlert('Section stored successfully', 'success'); // Show success alert
    // You can add additional logic here, such as showing a success message to the user

    fetchStoredSections(lawId);
  } catch {
    showAlert('Error storing section', 'error'); // Show error alert
    // You can add error handling logic here, such as showing an error message to the user
  }
}

const fetchStoredSections = async (lawId: number) => {
  try {
    const response = await axios.get('/api/v1/lawful/store', { 
      params: { selectedLawId: selectedLawId.value || 0 }, // Pass selectedLawId as a parameter
      headers: getHeadersFromStorage() 
    }) // Added headers
    storedSections.value = response.data // Assign the response data to storedSections
  } catch (error) {
    console.error('Error fetching stored sections:', error)
  }
}

const importSection = async (sectionId: number) => {
  if (selectedLawId.value) {
    try {
      const response = await axios.post('/api/v1/lawful/import', null, {
        params: {
          lawId: selectedLawId.value,
          sectionId: sectionId
        },
        headers: getHeadersFromStorage() // Added headers
      });
      fetchStoredSections(selectedLawId.value)
      showAlert('Section imported successfully', 'success'); // Show success alert
      // You can add additional logic here, such as showing a success message to the user
    } catch {
      showAlert('Error importing section', 'error'); // Show error alert
      // You can add error handling logic here, such as showing an error message to the user
    }
  } else {
    showAlert('No law selected for import', 'error'); // Show error alert
    // Optionally, show a message to the user indicating that no law is selected
  }
}

watch(selectedLawId, (newId) => {
  if (newId) {
    fetchSections(newId)
    fetchStoredSections(newId)
  } else {
    sections.value = []
  }
})
</script>

<template>
  <v-container>
    <v-alert v-if="alertMessage" :type="alertType ?? 'info'" dismissible>{{ alertMessage }}</v-alert> <!-- Add alert component -->
    <v-row>
      <v-col cols="6">
        <h1>Current Laws</h1>
        <v-select v-if="laws.length > 0" v-model="selectedLawId" :items="laws" item-title="Title" item-value="Id"
          label="Select a law"></v-select>
        <p v-else>No laws available</p>

        <div v-if="sections.length > 0">
          <h2>Sections:</h2>
          <SectionCard 
            v-for="(section, index) in sections" 
            :key="index" 
            :section="section" 
            :storeSection="() => storeSection(Number(selectedLawId), section.Index)" 
          /> <!-- Use the new SectionCard component -->
        </div>
        <p v-else-if="selectedLawId">Loading sections...</p>
        <p v-else>Select a law to view its sections</p>
      </v-col>
      <v-col cols="6">
        <h1>Stored Sections</h1>
        <v-btn @click="fetchStoredSections" color="primary" class="mb-2">Refresh List</v-btn>
        <div v-if="storedSections.length > 0">
          <v-card v-for="(section, index) in storedSections" :key="section.Id" class="mt-2">
            <v-card-title>{{ section.Title }}</v-card-title>
            <v-card-text>{{ section.Description }}</v-card-text>
            <v-card-text>{{ section.UserDescription }}</v-card-text>
            <v-card-actions>
              <v-btn v-if="section.CanImport" @click="importSection(section.Id)" color="success">Import</v-btn>
            </v-card-actions>
          </v-card>
        </div>
        <p v-else>No stored sections available</p>
      </v-col>
    </v-row>
  </v-container>
</template>

<style scoped>

</style>
