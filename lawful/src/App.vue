<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import * as lawful from '@/services/lawful';
import { VBtn, VCard, VCardItem, VCardTitle, VCardActions, VAlert } from 'vuetify/components'
import SectionCard from './components/SectionCard.vue';
import StoredSectionCard from './components/StoredSectionCard.vue';
import ImportSectionDialog from './components/ImportSectionDialog.vue'; // Import the new dialog component
import useClipboard from 'vue-clipboard3'
import type { LawDto, SectionDto, StoredSectionDto, ReferenceDto } from '@/services/dto';

const laws = ref<LawDto[]>([])
const selectedLawId = ref<number | null>(null)
const sections = ref<SectionDto[]>([])
const storedSections = ref<StoredSectionDto[]>([]) // New reactive variable for stored sections
const clipboard = useClipboard();

const alertMessage = ref<string | null>(null) // New reactive variable for alert message
const alertType = ref<'success' | 'error' | null>(null) // New reactive variable for alert type

const showAlert = (message: string, type: 'success' | 'error') => {
    alertMessage.value = message;
    alertType.value = type;
    setTimeout(() => {
        alertMessage.value = null; // Clear alert after a timeout
    }, 3000); // Adjust timeout duration as needed
}

const dialogVisible = ref(false); // New reactive variable for dialog visibility
const selectedSectionId = ref<number | null>(null); // New reactive variable for selected section ID

const openImportDialog = (sectionId: number) => {
    selectedSectionId.value = sectionId; // Set the selected section ID
    dialogVisible.value = true; // Open the dialog
}

onMounted(async () => {
  try {
    laws.value = await lawful.fetchLaws(); // Use the new API method
    await lawful.fetchStoredSections(0); // Fetch stored sections on mount
  } catch {
    showAlert('Error fetching laws', 'error'); // Show error alert
  }
})

const fetchSections = async (id: string | number) => {
  try {
    sections.value = await lawful.fetchSections(id); // Use the new API method
  } catch (error) {
    showAlert('Error fetching sections:', 'error');
    sections.value = []
  }
}

const storeSection = async (lawId: number, sectionIndex: number) => {
  try {
    await lawful.storeSection(lawId, sectionIndex); // Use the new API method
    showAlert('Section stored successfully', 'success'); // Show success alert
    fetchStoredSections(lawId);
  } catch {
    showAlert('Error storing section', 'error'); // Show error alert
  }
}

const fetchStoredSections = async (lawId: number) => {
  try {
    storedSections.value = await lawful.fetchStoredSections(lawId); // Use the new API method
  } catch (error) {
    console.error('Error fetching stored sections:', error)
  }
}

const importSection = async (sectionId: number) => {
  if (selectedLawId.value) {
    openImportDialog(sectionId); // Open the dialog instead of directly importing
  } else {
    showAlert('No law selected for import', 'error'); // Show error alert
  }
}

const deleteStoredSection = async (sectionId: number) => {
  try {
    await lawful.deleteStoredSection(sectionId); // Use the new API method
    showAlert('Section deleted successfully', 'success'); // Show success alert
    fetchStoredSections(selectedLawId.value || 0); // Refresh stored sections
  } catch {
    showAlert('Error deleting section', 'error'); // Show error alert
  }
}

const sectionJson = async (sectionId: number) => {
  try {
    const data = await sectionJson(sectionId); // Use the new API method
    await clipboard.toClipboard(JSON.stringify(data));
  } catch {
    showAlert('Error fetching JSON', 'error'); // Show error alert
  }
}

const doImportSection = async (params: { lawId: number, sectionId: number, references: ReferenceDto[] | null }) => {
  try {
    await lawful.importSection(params.lawId, params.sectionId, params.references);
    showAlert('Section imported successfully', 'success'); // Show success alert
    await fetchSections(params.lawId);
  } catch {
    showAlert('Error importing section', 'error'); // Show error alert
  }
}

watch(selectedLawId, (newId, oldId) => {
  if (newId !== oldId) {
    if (newId) {
      fetchSections(newId);
    } else {
      sections.value = [];
    }
    fetchStoredSections(newId || 0);
  }
})

const isAdmin = ref(false);

const refreshData = async () => {
  laws.value = await lawful.fetchLaws(); 
  await fetchStoredSections(selectedLawId.value || 0);
  if (selectedLawId.value) {
    await fetchSections(selectedLawId.value);
  }
}
</script>

<template>
  <v-container>
    <v-toolbar>
      <v-btn @click="refreshData" color="primary">Refresh</v-btn>
      <v-spacer></v-spacer>
      <v-switch v-model="isAdmin" label="Admin Mode" hide-details="auto" id="admin-switch"></v-switch>
    </v-toolbar>
    
    <v-alert v-if="alertMessage" :type="alertType ?? 'info'" dismissible>{{ alertMessage }}</v-alert>
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
          />
        </div>
        <p v-else-if="selectedLawId">Loading sections...</p>
        <p v-else>Select a law to view its sections</p>
      </v-col>
      <v-col cols="6">
        <h1>Stored Sections</h1>
        
        <div v-if="storedSections.length > 0">
          <StoredSectionCard 
            v-for="(section, index) in storedSections" 
            :key="section.Id" 
            :section="section" 
            :importSection="importSection" 
            :deleteSection="deleteStoredSection"
            :sectionJson="sectionJson"
          />
        </div>
        <p v-else>No stored sections available</p>
      </v-col>
    </v-row>

    <ImportSectionDialog 
      :isVisible="dialogVisible" 
      :lawId="selectedLawId ?? 0" 
      :sectionId="selectedSectionId ?? 0" 
      @close="dialogVisible = false" 
      @import="lawful.importSection" 
    />
  </v-container>
</template>

<style scoped>
  .toolbar {
    display: flex;
    align-items: center;
  }

  #admin-switch {
    padding-right: 10px;
  }
</style>
