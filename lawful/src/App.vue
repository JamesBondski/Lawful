<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import axios from 'axios'
import { VBtn, VCard, VCardItem, VCardTitle, VCardActions } from 'vuetify/components'

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
}

interface StoredSectionDto {
  Id: string;
  Title: string;
  Description: string;
  UserDescription: string;
}

const laws = ref<LawDto[]>([])
const selectedLawId = ref<number | null>(null)
const sections = ref<SectionDto[]>([])
const storedSections = ref<StoredSectionDto[]>([]) // New reactive variable for stored sections

onMounted(async () => {
  try {
    const response = await axios.get('/api/v1/lawful/law')
    laws.value = response.data
    await fetchStoredSections(); // Fetch stored sections on mount
  } catch (error) {
    console.error('Error fetching laws:', error)
  }
})

const fetchSections = async (id: string | number) => {
  try {
    const response = await axios.get(`/api/v1/lawful/section?LawId=${id}`)
    sections.value = response.data
  } catch (error) {
    console.error('Error fetching sections:', error)
    sections.value = []
  }
}

const storeSection = async (lawId: number, sectionIndex: number) => {
  try {
    const response = await axios.post('/api/v1/lawful/store', null, {
      params: {
        lawId,
        sectionIndex
      }
    })
    console.log('Section stored successfully:', response.data)
    // You can add additional logic here, such as showing a success message to the user

    fetchStoredSections();
  } catch (error) {
    console.error('Error storing section:', error)
    // You can add error handling logic here, such as showing an error message to the user
  }
}

const fetchStoredSections = async () => {
  try {
    const response = await axios.get('/api/v1/lawful/store') // Call the endpoint to fetch stored sections
    storedSections.value = response.data // Assign the response data to storedSections
  } catch (error) {
    console.error('Error fetching stored sections:', error)
  }
}

watch(selectedLawId, (newId) => {
  if (newId) {
    fetchSections(newId)
  } else {
    sections.value = []
  }
})
</script>

<template>
  <v-container>
    <v-row>
      <v-col cols="6">
        <header>
          <v-select
            v-if="laws.length > 0"
            v-model="selectedLawId"
            :items="laws"
            item-title="Title"
            item-value="Id"
            label="Select a law"
          ></v-select>
          <p v-else>No laws available</p>
        </header>

        <main>
          <h1>Current Laws</h1>
          <div v-if="sections.length > 0">
            <h2>Sections:</h2>
              <v-card v-for="(section, index) in sections" :key="index" class="mt-2">
                <v-card-title>{{ section.Title }}</v-card-title>
                <v-card-text>{{ section.Description }}</v-card-text>
                <v-card-text>{{ section.UserDescription }}</v-card-text>
                <v-card-actions>
                  <v-btn @click="storeSection(Number(selectedLawId), section.Index)" color="primary">Store</v-btn>
                </v-card-actions>
              </v-card>
          </div>
          <p v-else-if="selectedLawId">Loading sections...</p>
          <p v-else>Select a law to view its sections</p>
        </main>
      </v-col>
      <v-col cols="6">
        <h1>Stored Sections</h1>
        <v-btn @click="fetchStoredSections" color="primary" class="mb-2">Refresh List</v-btn> <!-- Refresh button -->
        <div v-if="storedSections.length > 0">
          <v-card v-for="(section, index) in storedSections" :key="section.Id" class="mt-2">
            <v-card-title>{{ section.Title }}</v-card-title>
            <v-card-text>{{ section.Description }}</v-card-text>
            <v-card-text>{{ section.UserDescription }}</v-card-text>
          </v-card>
        </div>
        <p v-else>No stored sections available</p>
      </v-col>
    </v-row>
  </v-container>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
